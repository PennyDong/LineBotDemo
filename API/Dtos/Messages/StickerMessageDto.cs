

using API.Enum;
using LineBotMessage.Dtos;

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