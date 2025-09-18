using System.Text;
using Domain.Common;
using Newtonsoft.Json;

namespace ChatSystem.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string errorId = context.TraceIdentifier;

            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                HandleException(context, exception, errorId);
            }
        }

        private void HandleException(HttpContext context, Exception exception, string errorId)
        {
            _logger.LogError(exception, $"Error ID: {errorId}");

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ApiResult<string>()
            {
                ErrorCode = Domain.Enums.ErrorCodeEnum.UnknownError,
                ErrorCodeLevel = Domain.Enums.ErrorCodeLevelEnum.Error,
                Message = "General error occurred, Please contact admin."
            }));

            context.Response.Body.WriteAsync(bytes);
        }
    }
}
