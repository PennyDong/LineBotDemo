using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.Webhook;
using API.Enum;

namespace API.Dtos.Messages
{
    public class ImageMessageDto : BaseMessageDto
    {
        public ImageMessageDto()
        {
            Type = MessageTypeEnum.Image;
        }

        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get;set; }
        
    }
}