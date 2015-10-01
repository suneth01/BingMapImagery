
namespace BingMapImagery.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;

    internal interface IMapRESTApiClient<T>
    {
        string Endpoint { get; set; }

        Task<Image> DownloadImage(string baseUrl, SortedList<int, string> pathParams, List<KeyValuePair<string, string>> queryParams);

        Task<T> GETResponse(string baseUrl, SortedList<int, string> pathParams, List<KeyValuePair<string, string>> queryParams);
    }
}