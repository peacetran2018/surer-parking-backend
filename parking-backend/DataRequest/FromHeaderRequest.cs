using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parking_backend.DataRequest
{
    public class FromHeaderRequest
    {
        [FromHeader]
        public string Authorization { get; set; }
    }
}
