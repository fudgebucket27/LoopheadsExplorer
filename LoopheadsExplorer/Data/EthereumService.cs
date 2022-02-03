using Nethereum.Web3;
using System.Diagnostics;

namespace LoopheadsExplorer.Data
{
    public class EthereumService
    {
        public async Task<string>  GetLoopheadBaseUri(string contractAddress)
        {
            var web3 = new Web3("https://mainnet.infura.io/v3/ccf44b7186544d6d8d57330f862e545a");
            var contractABI = "[{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"_collectionID\",\"type\":\"uint32\"},{\"internalType\":\"string\",\"name\":\"_baseTokenURI\",\"type\":\"string\"},{\"internalType\":\"contract IUniswapV3Pool\",\"name\":\"_uniswapPool\",\"type\":\"address\"},{\"internalType\":\"uint128\",\"name\":\"_baseAmount\",\"type\":\"uint128\"},{\"internalType\":\"int256[]\",\"name\":\"_priceLevels\",\"type\":\"int256[]\"},{\"internalType\":\"int256[]\",\"name\":\"_relativeLevels\",\"type\":\"int256[]\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"CURRENT_PRICE_SECONDS_AGO\",\"outputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"PREVIOUS_PRICE_SECONDS_AGO\",\"outputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"baseAmount\",\"outputs\":[{\"internalType\":\"uint128\",\"name\":\"\",\"type\":\"uint128\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"baseTokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"collectionID\",\"outputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"currentPrice\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"basePrice\",\"type\":\"uint256\"}],\"name\":\"getBaseLevel\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"level\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"secondsAgo\",\"type\":\"uint32\"}],\"name\":\"getPrice\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"int256[]\",\"name\":\"levels\",\"type\":\"int256[]\"},{\"internalType\":\"int256\",\"name\":\"value\",\"type\":\"int256\"}],\"name\":\"getRangeLevel\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"level\",\"type\":\"uint256\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"currentPrice\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"previousPrice\",\"type\":\"uint256\"}],\"name\":\"getRelativeLevel\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"level\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"priceLevels\",\"outputs\":[{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"relativeLevels\",\"outputs\":[{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"uniswapPool\",\"outputs\":[{\"internalType\":\"contract IUniswapV3Pool\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
            try
            {
                var contract = web3.Eth.GetContract(contractABI, contractAddress);
                var function = contract.GetFunction("baseTokenURI");
                var baseUri = await function.CallAsync<string>();
                return baseUri.Remove(0,7);
            }
            catch (System.Net.Sockets.SocketException sex)
            {
                Trace.WriteLine(sex.StackTrace + "\n" + sex.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + "\n" + e.StackTrace );
            }
            return "";
        }
    }
}
