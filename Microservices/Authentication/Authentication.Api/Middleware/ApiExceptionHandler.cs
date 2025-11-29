using Authentication.Api.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Common.Exceptions;

namespace Authentication.Api.Middleware
{
    public class ApiExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Initiate the specific NSwag-generated object
            BadResponse response = new BadResponse();
            int statusCode;

            // Mapping our exceptions
            switch (exception)
            {
                case ConflictException:
                    statusCode = StatusCodes.Status409Conflict;
                    response.Message = exception.Message;
                    break;
                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    response.Message = exception.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "En uventet fejl skete";
                    break;
            }

            // Sets the status code to the response
            httpContext.Response.StatusCode = statusCode;

            // Writes the BadResponse as an JSON
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            // Tells the program that we have sent the JSON
            return true;
        }
    }
}
