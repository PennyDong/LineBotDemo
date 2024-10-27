using System.Text;
using API.Dtos;
using API.Providers;

namespace API.Domain
{
    public class LinePayService
    {
        public LinePayService()
        {
            client = new HttpClient();
            _jsonProvider = new JsonProvider();
        }

        private readonly string channelId = "";//"{你的 LinePay 商家 Channel ID}";
        private readonly string channelSecretKey = ""; //"你的 LinePay 商家的 Channel Secret Key";

        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";

        private static HttpClient client;
        private static JsonProvider _jsonProvider;   
    

        public async Task<PaymentResponseDto> SendPaymentRequest(PaymentRequestDto dto)
        {
            var json = _jsonProvider.Serialize(dto);
            //產生 GUID Nonce
            var nonce = Guid.NewGuid().ToString();
            //要放入 signature 中的 requestUrl
            var requestUrl = "/v3/payments/request";

            //使用 channelSecretKey & requestUrl & jsonBody & nonce 做簽章
            var signature = SignatureProvider.HMACSHA256(channelSecretKey,channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(json,Encoding.UTF8,"application/json"),
            };

            //帶入 Headers

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce",nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            var linePayResponse = _jsonProvider.Deserialize<PaymentResponseDto>(await response.Content.ReadAsStringAsync());

            Console.WriteLine(nonce);
            Console.WriteLine(signature);

            return linePayResponse;
        }

        //取得 transactionId 後進行確定交易
        public async Task<PaymentConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentConfirmDto dto) //加上 OrderId 去找資料
        {
            var json = _jsonProvider.Serialize(dto);

            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, string.Format(linePayBaseApiUrl + requestUrl, transactionId))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId",channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            var responseDto = _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());

            return responseDto;
        }

        //使用者取消交易則會到這裏
        public async void TransactionCancel(string transactionId)
        {
            Console.WriteLine($"訂單 {transactionId} 已取消");
        }
    }
}