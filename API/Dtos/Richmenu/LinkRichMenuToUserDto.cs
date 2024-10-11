using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Richmenu
{
    public class LinkRichMenuToMultipleUserDto
    {
        public string? RichMenuId { get; set; }
        public List<string> UserIds { get; set; }
    }
}