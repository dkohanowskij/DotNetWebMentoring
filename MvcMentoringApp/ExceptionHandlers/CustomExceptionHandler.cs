using Microsoft.AspNetCore.Diagnostics;

namespace MvcMentoringApp.ExceptionHandlers
{
    public class CustomExceptionHandler:IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var exceptionMessage = string.Empty;
            var exceptionHandlerPathFeature =
            httpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                exceptionMessage = "The file was not found.";
            }

            if (exceptionHandlerPathFeature?.Path == "/")
            {
                exceptionMessage ??= string.Empty;
                exceptionMessage += " Page: Home.";
            }

            var originalExceptionMessage = exception.Message;

            logger.LogError(
                $"Error Message: {originalExceptionMessage}, Exception Handler Path Feature {exceptionMessage}",
                exceptionMessage, DateTime.UtcNow);

            return ValueTask.FromResult(false);
        }
    }
}
