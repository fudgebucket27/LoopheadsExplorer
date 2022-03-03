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
            _client.Authenticator = new HttpBasicAuthenticator(System.Environment.GetEnvironmentVariable("APPSETTING_INFURAIPFSUSERNAME"), System.Environment.GetEnvironmentVariable("APPSETTING_INFURAIPFSPASSWORD"));
        }

        public async Task<IpfsData> GetDirectoryContents(string ipfsHash)
        {
            var request = new RestRequest($"api/v0/dag/get");
            request.AddQueryParameter("arg", ipfsHash);
            request.AddQueryParameter("output-codec", "dag-json");
            try
            {
                var response = await _client.PostAsync(request);
                var data = JsonConvert.DeserializeObject<IpfsData>(response.Content);
                return data;
            }
            catch(System.Net.Http.HttpRequestException hex)
            {
                Trace.WriteLine(hex.StackTrace + "\n" + hex.Message);
            }
            catch(System.Net.Sockets.SocketException sex)
            {
                Trace.WriteLine(sex.StackTrace + "\n" + sex.Message);
            }
            catch(JsonReaderException e)
            {
                Trace.WriteLine(e.StackTrace + "\n" + e.Message);
            }
            return null;

        }

        public async Task<LoopheadMetadata> GetLoopheadMetadata(string ipfsHash)
        {
            var request = new RestRequest("api/v0/dag/get");
                request.AddQueryParameter("arg", ipfsHash);
                request.AddQueryParameter("output-codec", "dag-json");
                ;
            try
            {
                var response = await _client.PostAsync(request);
                var data = JsonConvert.DeserializeObject<IpfsData>(response.Content);
                var metadataBase64String = data.Data.Slash.bytes;
                byte[] metaDataByteArray = Convert.FromBase64String(metadataBase64String);
                var metadataAsString = Encoding.UTF8.GetString(metaDataByteArray);
                var metadataAsStringCleaned = new string(metadataAsString.Where(c => !char.IsControl(c)).ToArray());
                metadataAsStringCleaned = Regex.Replace(metadataAsStringCleaned, @"[^\u0000-\u007F]+", string.Empty);
                var loopheadMetaData = JsonConvert.DeserializeObject<LoopheadMetadata>(metadataAsStringCleaned);
                return loopheadMetaData;
            }
            catch (System.Net.Http.HttpRequestException hex)
            {
                Trace.WriteLine(hex.StackTrace + "\n" + hex.Message);
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
