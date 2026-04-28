using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common
{
    
 
        public class Result<T>
        {
            public bool IsSuccess { get; set; }  // ✅ Add setters
            public T? Data { get; set; }
            public string? ErrorMessage { get; set; }
            public IReadOnlyList<string> Errors { get; set; } = new List<string>();


            public Result()
            {
                Errors = new List<string>();
            }


            public Result(bool isSuccess, T? data, string? errorMessage, List<string>? errors)
            {
                IsSuccess = isSuccess;
                Data = data;
                ErrorMessage = errorMessage;
                Errors = errors ?? new List<string>();
            }

            public static Result<T> Success(T data)
            {
                if (data == null) throw new ArgumentNullException(nameof(data));
                return new Result<T>(true, data, null, null);
            }

            public static Result<T> Failure(string errorMessage)
                => new Result<T>(false, default, errorMessage, null);

            public static Result<T> Failure(List<string> errors)
                => new Result<T>(false, default, string.Join("; ", errors), errors);
        }
    }

