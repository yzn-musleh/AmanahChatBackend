using Hangfire;
using Hangfire.SqlServer;
using ChatSystem.Middleware;
using Application.Common.Interfaces;
using ChatSystem.UI;

namespace ChatSystem
{
    public static class ConfigureService
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services,
            IConfiguration configuration)
        {
//            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//            var signingKey = new X509SecurityKey(Certificate.Certificate.Get());
//            var issuer = configuration["Jwt:Issuer"];
//            var audience = configuration["Jwt:Audience"];
//            services
//                .AddAuthentication(options =>
//                {
//                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                })
//                .AddJwtBearer(options =>
//                {
//                    options.RequireHttpsMetadata = false;
//                    options.SaveToken = true;
//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        RequireExpirationTime = true,
//                        RequireSignedTokens = true,
//                        ValidateLifetime = true,
//#warning Certificate is expired renew or create a new one, certificate should be loaded from disk to allow custom cert on deployment
//                        //ValidateIssuerSigningKey = true,
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidIssuer = issuer,// authenticationConfig.Jwt.Issuer,
//                        ValidAudience = audience,//authenticationConfig.Jwt.Audience,
//                        IssuerSigningKey = signingKey// authenticationConfig.Jwt.GetSecurityKey(),
//                    };
//                    options.Events = new JwtBearerEvents
//                    {
//                        OnAuthenticationFailed = context =>
//                        {
//                            return Task.CompletedTask;
//                        },
//                        OnTokenValidated = context =>
//                        {
//                            // Note : we can add multiple roles or claims by adding new identity to he existing...
//                            //context.Principal.AddIdentity(tempIdentity);
//                            return Task.CompletedTask;
//                        }
//                    };
//                });

//            services.AddAuthorization(options =>
//            {
//            });

            // Add Hangfire services.
            services.AddHangfire(hangfire =>
            {
                hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                hangfire.UseSimpleAssemblyNameTypeSerializer();
                hangfire.UseRecommendedSerializerSettings();
                hangfire.UseColouredConsoleLogProvider();
                hangfire.UseSqlServerStorage(
                             configuration.GetConnectionString("DefaultConnection"),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    });

                var server = new BackgroundJobServer(new BackgroundJobServerOptions
                {
                    ServerName = "hangfire-test",
                });
            });

            services.AddSignalR();

            services.AddSingleton<ExceptionHandlingMiddleware>();
            services.AddSingleton<IChatHubService, ChatHubService>();

            return services;
        }
    }
}
