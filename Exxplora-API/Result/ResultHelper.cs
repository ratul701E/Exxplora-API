using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exxplora_API.Result
{
    public static class ResultHelper
    {
        public static Result<T> SuccessResponse<T>(string message, T data)
        {
            return new Result<T> { IsError = false, Messages = new List<string> { message }, Data = data };
        }

        public static Result<T> ErrorResponse<T>(string message = "Something is wrong", T data = default)
        {
            return new Result<T> { IsError = true, Messages = new List<string> { message }, Data = default };
        }

        public static Result<T> ErrorResponse<T>(List<string> messages, T data = default)
        {
            return new Result<T> { IsError = true, Messages = messages, Data = default };
        }
    }
}