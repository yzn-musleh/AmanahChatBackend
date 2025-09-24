using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApikeyService
    {
        public string GenerateApiKey(Guid tenantId);

        public bool ValidateApiKey(string apiKeyFromRequest, out AccessKeys matchedKey);

    }
}
