using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MlsaBadgeMaker.App.Exceptions
{
    public class ApiException : Exception
    {
        public override string Message { get; }
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        public ApiException(HttpResponseMessage httpResponseMessage)
        {
            Message = httpResponseMessage.Content.ReadAsStringAsync().Result;
            StatusCode = httpResponseMessage.StatusCode;
        }
    }
}
