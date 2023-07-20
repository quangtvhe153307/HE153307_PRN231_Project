using System.Text.Json.Serialization;

namespace Client.Models
{
    public class ODataReponseModel<T>
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }
        public List<T> Value { get; set; }
        [JsonPropertyName("@odata.nextLink")]
        public string NextLink { get; set; }
    }
}
