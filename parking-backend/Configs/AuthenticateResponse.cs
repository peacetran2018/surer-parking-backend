using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parking_backend.Configs
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
