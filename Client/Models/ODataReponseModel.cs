using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Client.Models
{
    public class ODataReponseModel<T>
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }
        public List<T> Value { get; set; }
        [JsonProperty("@odata.nextLink")]
        public string NextLink { get; set; }
    }
}
