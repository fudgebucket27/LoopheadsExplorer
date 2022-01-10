using LoopheadsExplorer.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics;

namespace LoopheadsExplorer.Data
{
    public class IpfsService
    {
        const string BaseUrl = "https://ipfs.infura.io:5001";

        readonly RestClient _client;

        public IpfsService()
        {
            _client = new RestClient(BaseUrl);
            _client.Authenticator = new HttpBasicAuthenticator("23U8HdMVy2jYDZPPGOOfbKnzGVj", "145799b9d4528e56c8d82eaba0a543d3");
        }

        public async Task<IpfsData> GetDirectoryContents(string ipfsDirectory)
        {
            var request = new RestRequest("api/v0/dag/get")
                .AddParameter("arg", ipfsDirectory)
                .AddParameter("output-codec", "dag-json")
                ;
            var response = await _client.GetAsync(request);
            var data = JsonConvert.DeserializeObject<IpfsData>(response.Content);
            return data;
        }
    }
}
