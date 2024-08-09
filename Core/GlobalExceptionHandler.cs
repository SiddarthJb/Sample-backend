using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Z1.Core.Exceptions;

namespace Z1.Core
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(
                $"An error occurred while processing your request: {exception.Message}");

            var errorResponse = new BaseResponse<int>
            {
                Message = exception.Message
            };

            switch (exception)
            {
                case NotFoundException:
                    errorResponse.Status = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = exception.GetType().Name;
                    break;

                case BadHttpRequestException:
                    errorResponse.Status = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = exception.GetType().Name;
                    break;

                default:
                    errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal Server Error";
                    break;
            }

            httpContext.Response.StatusCode = errorResponse.Status;

            await httpContext
                .Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
