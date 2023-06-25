using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Services
{
    public class ServiceResponse
    {
        public bool Success { get; set; } // result of operation
        public string Message { get; set; } = string.Empty; // message of the result
        public object? Payload { get; set; } // received result's object
        public IEnumerable<string>? Errors { get; set; } // list of errors if the result is failed
    }
}
