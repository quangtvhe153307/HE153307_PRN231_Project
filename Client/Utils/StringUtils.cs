using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Utils
{
    public class StringUtils
    {
        public static string GetMessageFromErrorResponse(string response)
        {
            var message = JsonConvert.DeserializeObject<JToken>(response);
            var result = message["message"];

            return result.ToString();
        }
    }
}
