using Newtonsoft.Json;

namespace LoopheadsExplorer.Models
{
    public class IpfsCid
    {
        [JsonProperty(PropertyName = "/")]
        public string value { get; set;}
    }
}
