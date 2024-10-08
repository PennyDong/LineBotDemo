using API.Enum;
using LineBotMessage.Dtos;

namespace API.Dtos.Messages
{
    public class VideoMessageDto : BaseMessageDto
    {
        public VideoMessageDto()
        {
            Type = MessageTypeEnum.Video;
        }
        
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
        public string? TrackingId { get; set; }
    }
}