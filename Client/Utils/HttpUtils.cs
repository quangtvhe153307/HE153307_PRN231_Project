using Newtonsoft.Json;

namespace Client.Utils
{
    public class HttpUtils
    {
        private static HttpClient httpClient;
        public static void Initialize(HttpClient client)
        {
            httpClient = client;
        }

        public static async Task<T> PostAsync<T>(string uri, HttpContent content)
        {
            var response = await httpClient.PostAsync(uri, content);
            try
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            } catch(Exception ex)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

        }

        public static Task<Dictionary<string, object>> PostAsync(string uri, HttpContent content)
        {
            return PostAsync<Dictionary<string, object>>(uri, content);
        }

        public static Task<T> PostFormAsync<T>(string uri, Dictionary<string, string> data)
        {
            return PostAsync<T>(uri, new FormUrlEncodedContent(data));
        }

        public static Task<Dictionary<string, object>> PostFormAsync(string uri, Dictionary<string, string> data)
        {
            return PostFormAsync<Dictionary<string, object>>(uri, data);
        }

        public static async Task<T> GetJson<T>(string uri)
        {
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            } else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }

        }
        public static Task<T> GetObject<T>(string uri)
        {
            return GetJson<T>(uri);
        }        
        public static Task<List<T>> GetList<T>(string uri)
        {
            return GetJson<List<T>>(uri);
        }
    }
}
