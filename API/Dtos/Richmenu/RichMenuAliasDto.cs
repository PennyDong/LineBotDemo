using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Richmenu
{
    public class RichMenuAliasListDto
    {
        public RichMenuAliasDto[] Aliases { get; set; }
    }

    public class RichMenuAliasDto
    {
        public string RichMenuAliasId { get; set; }
        public string RichMenuId { get; set; }
    }
}