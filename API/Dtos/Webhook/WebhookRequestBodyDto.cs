using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Webhook
{
    public class WebhookRequestBodyDto
    {
        public string? Destination { get; set; }
        public List<WebhookEventsDto> Events { get; set; }
        
        public string? ReplyToken { get; set; } //回覆此事件所使用的token
        public MessageEventDto? Message { get; set; } //收到訊息的事件，可收到 text、sticker、image、file、video、audio、location 訊息
        public ContentProviderDto? ContentProvider { get; set; }
        public ImageMessageEventImageSetDto? ImageSet { get; set; }
    }

}