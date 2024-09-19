using System.Net.Http.Headers;
using System.Text;
using API.Dtos.Messages;
using API.Dtos.Messages.Request;
using API.Dtos.Webhook;
using API.Enum;
using API.Providers;

namespace API.Domain
{
    public class LineBotService
    {
        // (將 LineBotController 裡宣告的 ChannelAccessToken & ChannelSecret 移到 LineBotService中)
        // 貼上 messaging api channel 中的 accessToken & secret
        private readonly string channelAccessToken = "ZmY9zXQfKh6ne6Idvi1D/lGqZB1RVER1v+MyMyWwS7fyis1FSiOo2JNh5nTrXXoefvE7MtHO0LlU/2y9LKaZvN08meRovOBU028q/Dq8jMrvRfBBDCuBmB6yzliI6TOBKbc+2OqFdBKIHLcwpTMicgdB04t89/1O/w1cDnyilFU=";
        private readonly string channelSecrect ="5821cf68195653888a23d30e0645c5ac";

        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";
        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest
        private readonly JsonProvider _jsonProvider = new JsonProvider(); 
    

        public LineBotService()
        {
        }

        public void ReceiveWebhook(WebhookRequestBodyDto requestBody)
        {
            foreach(var eventObject in requestBody.Events)
            {
                switch (eventObject.Type)
                {
                    case WebhookEventTypeEnum.Message:
                        var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                        {
                            ReplyToken = eventObject.ReplyToken,
                            Messages = new List<TextMessageDto>
                            {
                                new TextMessageDto()
                                {
                                    Text = $"您好，您傳送了\"{eventObject.Message.Text}\"!"
                                }
                            }
                        };
                        ReplyMessageHandler("text",replyMessage);
                        break;
                    case WebhookEventTypeEnum.Unsend:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}在聊天室收回訊息！");
                        break;
                    case WebhookEventTypeEnum.Follow:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}將我們新增為好友！");
                        break;
                    case WebhookEventTypeEnum.Unfollow:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}封鎖了我們！");
                        break;
                    case WebhookEventTypeEnum.Join:
                        Console.WriteLine("我們被邀請進入聊天室了！");
                        break;
                    case WebhookEventTypeEnum.Leave:
                        Console.WriteLine("我們被聊天室踢出了");
                        break;
                    case WebhookEventTypeEnum.MemberJoined:
                        string joinedMemberIds = "";
                        foreach(var member in eventObject.Joined.Members)
                        {
                            joinedMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine($"使用者{joinedMemberIds}加入了群組！");
                        break;
                    case WebhookEventTypeEnum.MemberLeft:
                        string leftMemberIds = "";
                        foreach (var member in eventObject.Left.Members)
                        {
                            leftMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine($"使用者{leftMemberIds}離開了群組！");
                        break;
                    case WebhookEventTypeEnum.Postback:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}觸發了postback事件");
                        break;
                    case WebhookEventTypeEnum.VideoPlayComplete:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}");
                        break;
                   
                }
            }   
        }

        public void BroadcastMessageHandler(string messageType, object requestBody)
        {
            string strBody = requestBody.ToString();
            switch (messageType)
            {
                case MessageTypeEnum.Text:
                    var messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<TextMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
            }
        }

        public async void BroadcastMessage<T>(BroadcastMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //帶入 channel access token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(broadcastMessageUri),
                Content = new StringContent(json,Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public void ReplyMessageHandler<T>(string messageType, ReplyMessageRequestDto<T> requestBody)
        {
            ReplyMessage(requestBody);
        }

        public async void ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
