using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Exceptions
{
  
        public class ApiValidationException : Exception
        {
            public int StatusCode { get; }
            public IEnumerable<string> Errors { get; }

            public IDictionary<string, string[]> errors { get; }
        public ApiValidationException(
        string message,
       // int statusCode = 400,
        IDictionary<string, string[]> err)
        : base(message)
        {
            errors = err;
         //   StatusCode = statusCode;
        }
        public ApiValidationException(
                string message,
                IEnumerable<string> errors,
                int statusCode = 400)
                : base(message)
            {
                StatusCode = statusCode;
                Errors = errors;
            }
        }
    }

