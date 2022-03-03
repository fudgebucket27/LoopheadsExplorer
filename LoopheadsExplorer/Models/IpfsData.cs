using Newtonsoft.Json;

namespace LoopheadsExplorer.Models
{
    public class IpfsData
    {
        public Data Data { get; set; }
        public List<IpfsLink> Links { get; set; }
    }

    public class Data
    {
        [JsonProperty(PropertyName = "/")]
        public string bytes { get; set; }
    }
}
