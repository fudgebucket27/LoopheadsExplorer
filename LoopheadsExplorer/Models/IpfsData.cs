using Newtonsoft.Json;

namespace LoopheadsExplorer.Models
{
    public class IpfsData
    {
        public Data data { get; set; }
        public List<IpfsLink> links { get; set; }
    }

    public class Data
    {
        [JsonProperty(PropertyName = "/")]
        public string bytes { get; set; }
    }
}
