using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Providers
{
    public class JsonProvider
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static JsonSerializerOptions deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, serializerOptions);
        }

        public T Deserialize<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str, deserializeOptions);
        }
    }
}