using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enum;

namespace API.Dtos.Messages
{
    public class TextMessageDto : BaseMessageDto
    {
        public TextMessageDto()
        {
            Type = MessageTypeEnum.Text;
        }

        public string Text { get; set; }
    }
}