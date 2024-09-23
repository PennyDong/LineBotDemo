using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enum;

namespace API.Dtos.Messages
{
    public class StickerMessageDto:BaseMessageDto
    {
        public StickerMessageDto()
        {
            Type = MessageTypeEnum.Sticker;
        }

        public string PackageId { get; set; }
        public string StickerId { get; set; }
        
    }
}