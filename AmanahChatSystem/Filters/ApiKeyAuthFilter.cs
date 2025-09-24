using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatSystem.UI.Filters
{
    public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
    {
        public readonly IConfiguration configuration;
        private readonly IApikeyService _service;

        public ApiKeyAuthFilter(IConfiguration configuration, IApikeyService service)
        {
            this.configuration = configuration;
            _service = service;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, 
                out var extractedApi))
            {
                context.Result = new UnauthorizedObjectResult("Api Key is missing");
                Console.WriteLine("extracted api", extractedApi);
                return;
            }

            var apiKey = configuration.GetValue<string>(AuthConstants.ApiSectionName);

            var isValid =  _service.ValidateApiKey(extractedApi, out var matchedKey);

            if (!isValid)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key");   
                return;
            }
        }

    }
}
