

using API.Enum;
using LineBotMessage.Dtos;

namespace API.Dtos.Messages
{
    public class TextMessageDto : BaseMessageDto
    {
        public TextMessageDto()
        {
            Type = MessageTypeEnum.Text;
        }

        public string Text { get; set; }

        public class TextMessageEmojiDto
        {
            public int Index { get; set; }
            public string ProductId { get; set; }
            public string EmojiId { get; set; }
        }

        public List<TextMessageEmojiDto>? Emojis { get; set; }

    }
}