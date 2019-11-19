using System;

namespace webapi.core.UseCases
{
    public class DataResult
    {
        public int Error { get; set; } = 0; // Success
        public Object Data { get; set; }
    }
}