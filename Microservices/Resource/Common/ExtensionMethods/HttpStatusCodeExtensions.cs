using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
	public static class HttpStatusCodeExtensions
	{
		public static int ToInt(this HttpStatusCode statusCode)
		{
			return (int)statusCode;
		}
	}
}
