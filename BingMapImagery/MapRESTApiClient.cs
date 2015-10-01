
namespace BingMapImagery
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using BingMapImagery.Interfaces;

    using Newtonsoft.Json;

    // TODO: change to use RestSharp if/when advanced features necessory
    internal class MapRESTApiClient<T> : IMapRESTApiClient<T>
    {
        private const string AcceptContent = "*/*";

        private const string GetMethod = "GET";

        public string Endpoint { get; set; }

        public async Task<Image> DownloadImage(string baseUrl, SortedList<int, string> pathParams, List<KeyValuePair<string, string>> queryParams)
        {
            var bingEndpointUri = this.GetQueryUri(baseUrl, pathParams,queryParams);
            
            using (var httpWebResponseObj = await WebRequest.Create(bingEndpointUri).GetResponseAsync())
            {
                var httpWebResponse = (HttpWebResponse)httpWebResponseObj;

                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                using (var responseStream = httpWebResponse.GetResponseStream())
                {
                    Image image = null;
                    if (responseStream != null)
                    {
                        image = Image.FromStream(responseStream);
                    }

                    return image;
                }
            }            
        }

        public async Task<T> GETResponse(string baseUrl, SortedList<int, string> pathParams, List<KeyValuePair<string, string>> queryParams)
        {
            string responseContent;
            var bingEndpointUri = this.GetQueryUri(baseUrl, pathParams, queryParams);

            var webRequest = (HttpWebRequest)WebRequest.Create(bingEndpointUri);
            webRequest.Accept = AcceptContent;
            webRequest.Method = GetMethod;

            try
            {
                var response = await webRequest.GetResponseAsync();

                using (response)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null)
                        {
                            throw new IOException("Could not load the response stream returned from the Bing REST API.");
                        }

                        var streamReader = new StreamReader(responseStream);

                        responseContent = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException)
            {
                throw;
            }

            var desResponse = JsonConvert.DeserializeObject<T>(responseContent);

            return desResponse;
        }        

        private Uri GetQueryUri(string baseUrl, SortedList<int, string> pathParams, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            var query = new StringBuilder();

            foreach (var pathParam in pathParams.Values)
            {
                query.AppendFormat("/{0}", pathParam);
            }

            var firstQueryItem = arguments.First();
            foreach (var argument in arguments)
            {
                if (argument.Key == firstQueryItem.Key)
                {
                    query.AppendFormat("?{0}={1}", argument.Key, argument.Value);
                }
                else
                {
                    query.AppendFormat("&{0}={1}", argument.Key, argument.Value);                    
                }
            }

            return new Uri(string.Concat(baseUrl, query.ToString()));
        }
    }
}