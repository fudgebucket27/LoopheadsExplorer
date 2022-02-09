using Nethereum.ABI.FunctionEncoding.Attributes;

namespace LoopheadsExplorer.Models
{
    public class ContractParameter
    {
        [Parameter("uint256", "tokenId", 1)]
        public string TokenId { get; set; }
    }
}
