using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using API.Dtos.Richmenu;
using API.Providers;
using Microsoft.AspNetCore.Mvc;

namespace API.Domain
{
    public class RichMenuService
    {
        private readonly string channelAccessToken = "MBtZnhz4fyaOemgcFfIHLzkAXpFCIU0tt+5kXFsUsSddR0i506eqiwCmiN2lqBmufvE7MtHO0LlU/2y9LKaZvN08meRovOBU028q/Dq8jMpgOG8Nmz1FtOqgwLpddZmIDY+oUOen+paCLQY8WPTCbgdB04t89/1O/w1cDnyilFU=";
        private readonly string channelSecrect ="5821cf68195653888a23d30e0645c5ac";

        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest
        private readonly JsonProvider _jsonProvider = new JsonProvider(); 

        private readonly string validateRichMenuUri = "https://api.line.me/v2/bot/richmenu/validate";
        private readonly string createRichMenuUri = "https://api.line.me/v2/bot/richmenu";
        private readonly string getRichMenuListUri = "https://api.line.me/v2/bot/richmenu/list";
        // {0} 的位置要帶入 richMenuId
        private readonly string richMenuImageUri = "https://api-data.line.me/v2/bot/richmenu/{0}/content";
        // {0} 的位置要帶入 richMenuId
        private readonly string setDefaultRichMenuUri = "https://api.line.me/v2/bot/user/all/richmenu/{0}";

        // richMenu 切換
        private readonly string createRichMenuAliasUri = "https://api.line.me/v2/bot/richmenu/alias";
        private readonly string commonRichMenuAliasUri = "https://api.line.me/v2/bot/richmenu/alias/{0}";
        private readonly string getRichMenuAliasListUri = "https://api.line.me/v2/bot/richmenu/alias/list";

        private readonly string getRichMenuUri = "https://api.line.me/v2/bot/richmenu/{0}";
        private readonly string defaultRichMenuUri = "https://api.line.me/v2/bot/user/all/richmenu";
        // 個人的 linked rich menu 操作
        private readonly string linkedRichMenuOfUserUri = "https://api.line.me/v2/bot/user/{0}/richmenu";
        // 多人的 linked rich menu 操作
        private readonly string linkedRichMenuOfMultipleUserUri = "https://api.line.me/v2/bot/richmenu/bulk/";

        public RichMenuService()
        {
            
        }


        //將傳入的 rich menu 物件送到 Line 去驗證其格式是否正確
        public async Task<string> ValidateRichMenu(RichMenuDto richMenu)
{
    var jsonBody = new StringContent(_jsonProvider.Serialize(richMenu), Encoding.UTF8, "application/json");
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Post,
        RequestUri = new Uri(validateRichMenuUri),
        Content = jsonBody,
    };
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
    var response = await client.SendAsync(request);

    return await response.Content.ReadAsStringAsync();
}
        /*
        將傳入的格式送到 Line 然後建立 rich menu，並且其格式內容會儲存在 Line 平台中，
        一支 Line Bot 最多可以儲存 1000 張 rich menu，建立成功後會收到建立好的 richmenuId。
        */
        public async Task<string> CreateRichMenu(RichMenuDto richMenu)
        {
            var jsonBody = new StringContent(_jsonProvider.Serialize(richMenu),Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(createRichMenuUri),
                Content = jsonBody,
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);
            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }


        //這個 function 會回傳目前儲存在 Line 上的所有 richmenu 格式。
        public async Task<RichMenuListDto> GetRichMenuList()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(getRichMenuListUri),
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);
            var response = await client.SendAsync(request);

            Console.WriteLine(await response.Content.ReadAsStringAsync());
            var list = _jsonProvider.Deserialize<RichMenuListDto>(await response.Content.ReadAsStringAsync());

            //依照名稱排序

            list.RichMenus = list.RichMenus.OrderBy((rm) => rm.Name).ToList();

            return list;
        }


        /*
        此 function 是使用剛剛 create rich menu 成功收到的 rich menu id 去上傳圖片，
        這裡傳送訊息時圖片是採用 URL 的方式不同，rich menu 上傳圖片是將整格檔案內容傳給 Line
        */
        public async Task<string> UploadRichMenuImage(string richMenuId,IFormFile imageFile)
        {
            //判斷檔案格式 須為png or jpeg
            if(!(Path.GetExtension(imageFile.FileName).Equals(".png",StringComparison.OrdinalIgnoreCase) 
            || Path.GetExtension(imageFile.FileName).Equals(".jpg",StringComparison.OrdinalIgnoreCase)))
            {
                return "圖片格式錯誤，須為 png or jpeg";
            }
            using(var stream = new MemoryStream())
            {
                //建立檔案內容

                imageFile.CopyTo(stream);
                var fileBytes = stream.ToArray();
                var content = new ByteArrayContent(fileBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                var request = new HttpRequestMessage(HttpMethod.Post, String.Format(richMenuImageUri, richMenuId))
                {
                    Content = content
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken );
                var response = await client.SendAsync(request);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> SetDefaultRichMenu(string richMenuId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,String.Format(setDefaultRichMenuUri,richMenuId));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }


        //richMenu Action

        public async Task<string> CreateRichMenuAlias(RichMenuAliasDto richMenuAlias)
        {
            var jsonBody = new StringContent(_jsonProvider.Serialize(richMenuAlias), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var request = new HttpRequestMessage(HttpMethod.Post, createRichMenuAliasUri)
            {
                Content = jsonBody
            };

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteRichMenuAlias(string richMenuAliasId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format(commonRichMenuAliasUri, richMenuAliasId));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateRichMenuAlias(string richMenuAliasId, string richMenuId)
        {
            var body = new { richMenuId = richMenuId};
            var jsonBody = new StringContent(_jsonProvider.Serialize(body),Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var request = new HttpRequestMessage(HttpMethod.Post, string.Format(commonRichMenuAliasUri,richMenuAliasId))
            {
                Content = jsonBody
            };

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<RichMenuAliasDto> GetRichMenuAliasInfo(string richMenuAliasId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(commonRichMenuAliasUri, richMenuAliasId));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
                return _jsonProvider.Deserialize<RichMenuAliasDto>(await response.Content.ReadAsStringAsync());
            else
                return new RichMenuAliasDto();
        }

        public async Task<RichMenuAliasListDto> GetRichMenuAliasListInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get , getRichMenuAliasListUri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
                return _jsonProvider.Deserialize<RichMenuAliasListDto>(await response.Content.ReadAsStringAsync());
            else
                return new RichMenuAliasListDto();
        }

        //Line 提供之 API 會將我們上傳的 rich menu image 檔案內容傳回，格式與上傳時相同，皆是 Byte[] 類別

        public async Task<FileContentResult> DownloadRichMenuImage(string richMenuId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            //直接取得回傳的 Byte Array

            var bytes = await client.GetByteArrayAsync(string.Format(richMenuImageUri,richMenuId));

            return new FileContentResult(bytes, "image/png");
        }


        //使用 Message api 的話，一個 Line Bot 最多可以存放 1000 張 rich menu 在 Line 的平台中，若存滿時，則需刪除多餘的 rich menu 才能再新增 rich menu。

        public async Task<string> DeleteRichMenu(string richMenuId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format(getRichMenuUri, richMenuId));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        //回傳該 rich menu 上傳的格式內容。
        public async Task<RichMenuDto> GetRichMenuById(string richMenuId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(getRichMenuUri, richMenuId));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);
            var richMenu = _jsonProvider.Deserialize<RichMenuDto>(await response.Content.ReadAsStringAsync());

            return richMenu;
        }

        //取得預設RichMenuID
        public async Task<RichMenuDto> GetDefaultRichMenuId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, defaultRichMenuUri);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            return _jsonProvider.Deserialize<RichMenuDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> CancelDefaultRichMenu()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, defaultRichMenuUri);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);
            
            return await response.Content.ReadAsStringAsync();
        }

        //取得目前綁定給該 User 的 richMenuId。
        public async Task<RichMenuDto> GetRichMenuIdLinkedToUser(string userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(linkedRichMenuOfUserUri,userId));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var response = await client.SendAsync(request);

            return _jsonProvider.Deserialize<RichMenuDto>(await response.Content.ReadAsStringAsync());
        }

         public async Task<string> LinkRichMenuToUser(string userId,string richMenuId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Format(linkedRichMenuOfUserUri + "/{1}",userId,richMenuId));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UnlinkRichMenuFromUser(string userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format(linkedRichMenuOfUserUri, userId));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> LinkRichMenuToMultipleUser(LinkRichMenuToMultipleUserDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, linkedRichMenuOfMultipleUserUri + "link")
            {
                Content = new StringContent(_jsonProvider.Serialize(dto), Encoding.UTF8, "application/json")
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",channelAccessToken);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UnlinkRichMenuFromMultipleUser(LinkRichMenuToMultipleUserDto dto)
        {
             var request = new HttpRequestMessage(HttpMethod.Post, linkedRichMenuOfMultipleUserUri + "unlink")
            {
                Content = new StringContent(_jsonProvider.Serialize(dto), Encoding.UTF8, "application/json")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync(); 
        }
    }
}