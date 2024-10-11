using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using API.Dtos.Login;
using API.Dtos.Profile;
using API.Providers;

namespace API.Domain
{
    public class LineLoginService
    {
        private static HttpClient client = new HttpClient();

        private readonly JsonProvider _jsonProvider = new JsonProvider();
        private readonly string loginUrl = "https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope={4}";
        private readonly string clientId = "clientId"; //{channel ID}
        private readonly string clientSecret  = "clientSecret";//channel secret}
        private readonly string tokenUrl = "https://api.line.me/oauth2/v2.1/token";
        private readonly string profileUrl = "https://api.line.me/v2/profile";

        public LineLoginService()
        {

        }

        //回傳 line authorization url
        public  string GetLoginUrl(string redirectUrl)
        {
            //根據想要得到的資訊填寫
            var scope = "profile&openId";

            //這個 state 是隨便打的
            var state = "1qazRTGFDY5ysg";

            var uri = string.Format(loginUrl,"code",clientId,HttpUtility.UrlEncode(redirectUrl), state,scope);

            return uri;
        }

        //取得 access token 等資料
        public async Task<TokensResponseDto> GetTokensByAuthToken(string authToken,string callbackUri)
        {
            var formContent = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        new KeyValuePair<string, string>("code", authToken),
        new KeyValuePair<string, string>("redirect_uri",callbackUri),
        new KeyValuePair<string, string>("client_id", clientId),
        new KeyValuePair<string, string>("client_secret", clientSecret),
    });

    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //添加 accept header
    var response = await client.PostAsync(tokenUrl, formContent); // 送出 post request

    Console.WriteLine("顯示回應訊息" + response);

    //檢查回應是否成功
    // if (!response.IsSuccessStatusCode)
    // {
    //     var errorContent = await response.Content.ReadAsStringAsync();
    //     Console.WriteLine("Error: " + errorContent); // 記錄或顯示錯誤
    //     throw new Exception("Error while fetching tokens: " + errorContent);
    // }

    var dto = _jsonProvider.Deserialize<TokensResponseDto>(await response.Content.ReadAsStringAsync()); //將 json response 轉成 dto

    return dto;
        }

        public async Task<UserProfileDto> GetUserProfileByAccessToken(string accessToken)
        {
            //取得 UserProfile
            var request = new HttpRequestMessage(HttpMethod.Get, profileUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);
            var profile = _jsonProvider.Deserialize<UserProfileDto>(await response.Content.ReadAsStringAsync());

            return profile;
        }
    }
}