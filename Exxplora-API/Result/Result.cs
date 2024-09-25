using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exxplora_API.Result
{
    public class Result<T>
    {

        public bool IsError { get; set; }
        public List<string> Messages { get; set; }
        public T Data { get; set; }

        public Result() { }

        public Result(bool isError, List<string> messages, T data)
        {
            IsError = isError;
            Messages = messages;
            Data = data;
        }
    }
}