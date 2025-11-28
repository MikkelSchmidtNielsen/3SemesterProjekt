using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.InternalApiCalls
{
	public class ApiErrorException : Exception
	{
		public int StatusCode { get; }
		public string? ApiErrorMessage { get; }
		public ApiException OriginalException { get; }

		public ApiErrorException(string? apiErrorMessage, int statusCode, ApiException original)
			: base(apiErrorMessage ?? original.Message, original)
		{
			StatusCode = statusCode;
			ApiErrorMessage = apiErrorMessage;
			OriginalException = original;
		}
	}

}
