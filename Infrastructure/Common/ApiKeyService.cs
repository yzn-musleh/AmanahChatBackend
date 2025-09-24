using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Infrastructure.Common
{
    public class ApiKeyService : IApikeyService
    {
        private readonly ApplicationDbContext _context;
        private readonly TenantContext _tenantContext;

        public ApiKeyService(ApplicationDbContext context, TenantContext tenantContext)
        {
            _context = context;
            _tenantContext = tenantContext;
        }

        public string GenerateApiKey(Guid tenantId)
        {
            // Generate short KeyId
            var keyId = Convert.ToBase64String(RandomNumberGenerator.GetBytes(8));
            keyId = keyId.Replace("+", "").Replace("/", ""); // URL-safe

            // Generate secret
            var secretBytes = RandomNumberGenerator.GetBytes(32);
            var secret = Convert.ToBase64String(secretBytes);

            // Generate salt
            var salt = RandomNumberGenerator.GetBytes(16);

            // Hash secret
            var hash = HashApiKey(secret, salt);

            // Save to DB
            var accessKey = new AccessKeys
            {
                TenantId = tenantId,
                KeyId = keyId,
                KeyHash = hash,
                Salt = salt,
                IssuedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.AccessKeys.Add(accessKey);
            _context.SaveChanges();

            // Return full API key
            return $"{keyId}.{secret}";
        }

        private string HashApiKey(string secret, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: secret,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32
            ));
        }

        public bool ValidateApiKey(string apiKeyFromRequest, out AccessKeys matchedKey)
        {
            matchedKey = null;

            // Split KeyId and Secret
            var parts = apiKeyFromRequest.Split('.', 2);
            if (parts.Length != 2) return false;

            var keyId = parts[0];
            var secret = parts[1];

            // Lookup by KeyId
            var keyRecord = _context.AccessKeys
                .Include(k => k.Tenant)
                .FirstOrDefault(k => k.KeyId == keyId && k.IsActive);
            if (keyRecord == null) return false;

            // Hash secret with stored salt
            var hash = HashApiKey(secret, keyRecord.Salt);

            if (hash != keyRecord.KeyHash) return false;

            _tenantContext.SetTenant(keyRecord.TenantId);

            matchedKey = keyRecord;
            return true;
        }

    }

}
