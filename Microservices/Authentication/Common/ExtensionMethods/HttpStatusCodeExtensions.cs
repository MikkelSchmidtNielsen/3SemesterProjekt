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
        /// <summary>
        /// Converts the HttpStatusCode enum to an int
        /// </summary>
        public static int ToInt(this HttpStatusCode statusCode)
        {
            return (int)statusCode;
        }
    }
}
