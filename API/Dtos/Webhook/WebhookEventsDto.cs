namespace API.Dtos.Webhook
{
    public class WebhookEventsDto
    {
        public string Type { get; set; }
        public string Mode { get; set; }
        public long Timestamp { get; set; }
        public SourceDto Source { get; set; }
        public string WebhookEventId { get; set; }
        public DeliverycontextDto Deliverycontext { get; set; }
         // -------- 以下 event properties--------
        public string? ReplyToken { get; set; } // 回覆此事件所使用的 token
        public MessageEventDto? Message { get; set; } // 收到訊息的事件，可收到 text、sticker、image、file、video、audio、location 訊息
        public UnsendEventDto? Unsend { get; set; } //使用者回收訊息

        public MemberEventDto? Joined { get; set; }
        public MemberEventDto? Left { get; set; }
        public PostbackEventDto? Postback { get; set; }
        public VideoViewingCompleteEventObjectDto? VideoPlayComplete { get; set; }
    }

    public class SourceDto
    {
        public string Type { get; set; }
        public string? UserId { get; set; }
        public string? GroupId { get; set; }
        public string? RoomId { get; set; }
    }

    public class DeliverycontextDto
    {
        public bool IsRedelivery { get; set; }
    }

    public class MessageEventDto
    {
        public string Id { get; set; }
        public string Type { get; set; }

        //Text Message Event
        public string? Text { get; set; }
        public List<TextMessageEventEmojiDto>? Emojis { get; set; }
        public TextMessageEventMentionDto? MentionDto { get; set; }
        public int? Duration { get; set; } //影片或音檔 (單位:ms)

        //File Message Event
        public string? FileName { get; set; }
        public int? FileSize { get; set; } 

        //Location Message Event
        public string? Title { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

         // Sticker Message Event
        public string? PackageId { get; set; }
        public string? StickerId { get; set; }
        public string? StickerResourceType { get; set; }
        public List<string>? Keywords { get; set; }
        
    }

    public class TextMessageEventEmojiDto
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string ProductId { get; set; }
        public string EmojiId { get; set; }
    }

    public class TextMessageEventMentionDto
    {
        public List<TextMessageEventMentionDto> Mentionees { get; set; }
    }

    public class TextMessageEventMentioneeDto
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string UserId { get; set; }
    }

    public class ContentProviderDto
    {
        public string Type { get; set; }
        public string? OriginalContentUrl { get; set; }
        public string? PreviewImageUrl { get; set; }
    } 

    public class ImageMessageEventImageSetDto 
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string Total { get; set; }
    }
    
    public class UnsendEventDto
    {
        public string messageId { get; set; }
    }

    public class MemberEventDto
    {
        public List<SourceDto> Members { get; set; }
    }

    public class PostbackEventDto
    {
        public string? Data { get; set; }
        public PostbackEventParamDto? Params { get; set; }
    }

    public class PostbackEventParamDto
    {
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? DateTime { get; set; }
        public string? NewRichMenuAliasId { get; set; }
        public string? Status { get; set; }
    }

    public class VideoViewingCompleteEventObjectDto
    {
        public string TrackingId { get; set; }
    }

}