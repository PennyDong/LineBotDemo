using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enum;
using LineBotMessage.Dtos;

namespace API.Dtos.Messages.Request
{
     public class AudioMessageDto :BaseMessageDto
    {
        public AudioMessageDto()
        {
            Type = MessageTypeEnum.Audio;
        }

        public string OriginalContentUrl { get; set; }
        public int Duration { get; set; }
    }

}