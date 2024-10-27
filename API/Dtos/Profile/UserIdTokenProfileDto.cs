using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Profile
{
    public class UserIdTokenProfileDto
    {
        public string? Iss { get; set; }
        public string? Sub { get; set; }
        public string? Aud { get; set; }
        public int? Exp { get; set; }
        public int? Auth_time { get; set; }
        public string? Nonce { get; set; }
        public string[]? Amr { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Email { get; set; }
    }
}