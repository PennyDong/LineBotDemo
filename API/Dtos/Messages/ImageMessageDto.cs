
using API.Enum;
using LineBotMessage.Dtos;

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