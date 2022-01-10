using LoopheadsExplorer.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

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

        public async Task<IpfsData> GetDirectoryContents(string ipfsHash)
        {
            var request = new RestRequest("api/v0/dag/get")
                .AddParameter("arg", ipfsHash)
                .AddParameter("output-codec", "dag-json")
                ;
            var response = await _client.GetAsync(request);
            var data = JsonConvert.DeserializeObject<IpfsData>(response.Content);
            return data;
        }

        public async Task<LoopheadMetadata> GetLoopheadMetadata(string ipfsHash)
        {
            var request = new RestRequest("api/v0/dag/get")
                .AddParameter("arg", ipfsHash)
                .AddParameter("output-codec", "dag-json")
                ;
            var response = await _client.GetAsync(request);
            var data = JsonConvert.DeserializeObject<IpfsData>(response.Content);
            var metadataBase64String = data.data;
            byte[] metaDataByteArray = Convert.FromBase64String(metadataBase64String);
            var metadataAsString = Encoding.UTF8.GetString(metaDataByteArray);
            var metadataAsStringCleaned = new string(metadataAsString.Where(c => !char.IsControl(c)).ToArray());
            metadataAsStringCleaned = Regex.Replace(metadataAsStringCleaned, @"[^\u0000-\u007F]+", string.Empty);
            var loopheadMetaData = JsonConvert.DeserializeObject<LoopheadMetadata>(metadataAsStringCleaned);

            return loopheadMetaData;
        }
    }
}
