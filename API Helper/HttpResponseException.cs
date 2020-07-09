using System;
using System.Collections.Generic;
using System.Text;

namespace API_Helper
{
    /// <summary>
    /// This exception gets thrown if there is an error with the api requests.
    /// </summary>
    public class HttpResponseException : ApplicationException
    {
        public HttpResponseException()
            : base()
        {
        }

        public HttpResponseException(string msg)
            : base(msg)
        {
        }

        public HttpResponseException(string msg, Exception inner)
            : base(msg, inner)
        {
        }

    }
}
