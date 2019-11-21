using System.Collections.Generic;
using System;
namespace webapi.Models.Response
{
    public class ResponseData
    {
        public Object BadRequest { get; set; }
        public Object NotFound { get; set; }
        public string Forbid { get; set; } = "";
        public Object Data { get; set; }
    }
}