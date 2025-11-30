using Common.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middleware
{
	public class ApiExceptionHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			// Initiate the specific NSwag-generated object
			BadResponse badResponse = new BadResponse();
			int statusCode;

			switch (exception)
			{
				case ConflictException:
					statusCode = StatusCodes.Status409Conflict;
					badResponse.Message = exception.Message;
					break;

				case ArgumentException:
					statusCode = StatusCodes.Status400BadRequest;
					badResponse.Message = exception.Message;
					break;

				case NotFoundException:
					statusCode = StatusCodes.Status404NotFound;
					badResponse.Message = exception.Message;
					break;

				default:
					statusCode = StatusCodes.Status500InternalServerError;
					badResponse.Message = "En uventet fejl skete";
					break;
			}

			// Sets the status code
			httpContext.Response.StatusCode = statusCode;

			// Writes and sends the reponse as JSON back to the client
			await httpContext.Response.WriteAsJsonAsync(badResponse, cancellationToken);

			// Tells the program that the reponse has been sent
			return true;
		}
	}
}
