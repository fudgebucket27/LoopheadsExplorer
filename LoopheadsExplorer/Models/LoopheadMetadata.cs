using Newtonsoft.Json;

namespace LoopheadsExplorer.Models
{
    public class LoopheadMetadata
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }

        [JsonProperty(PropertyName = "external_uri")]
        public string externalUri { get; set; }
        [JsonProperty(PropertyName = "cache_expiry_seconds")]
        public int cacheExpirySeconds { get; set; }
        List<LoopheadTrait> attributes { get; set; }
    }
}
