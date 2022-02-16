using Newtonsoft.Json;

namespace LoopheadsExplorer.Models
{
    public class LoopheadTrait
    {
        [JsonProperty(PropertyName = "trait_type")]
        public string traitType { get; set; }
        public string value { get; set; }
    }
}