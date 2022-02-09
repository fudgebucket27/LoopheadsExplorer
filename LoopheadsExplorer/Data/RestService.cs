using LoopheadsExplorer.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace LoopheadsExplorer.Data
{
    public class RestService
    {
        const string BaseUrl = "https://loopring.mypinata.cloud/ipfs/";

        readonly RestClient _client;

        public RestService()
        {
            _client = new RestClient(BaseUrl);
        }

        public async Task<LoopheadMetadata> GetLoopheadMetadata(string link)
        {
            var request = new RestRequest(link);
            try
            {
                var response = await _client.GetAsync(request);
                return JsonConvert.DeserializeObject<LoopheadMetadata>(response.Content);
            }
            catch (System.Net.Sockets.SocketException sex)
            {
                Trace.WriteLine(sex.StackTrace + "\n" + sex.Message);
            }
            catch (JsonReaderException e)
            {
                Trace.WriteLine(e.StackTrace + "\n" + e.Message);
            }
            return null;
            ;
        }
    }
}
