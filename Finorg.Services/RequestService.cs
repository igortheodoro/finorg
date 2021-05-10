using Finorg.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace Finorg.Services
{
    public class RequestService : IRequestService
    {
        public T Get<T>(string url, string endpoint)
        {
            var restClient = new RestClient(url);
            var req = new RestRequest(endpoint, Method.GET);

            return JsonConvert.DeserializeObject<T>(restClient.Execute<T>(req).Content);
        }

        public List<T> Post<T>(string url, string endpoint, object body)
        {
            var restClient = new RestClient(url);
            var req = new RestRequest(endpoint, Method.GET);

            req.AddJsonBody(body);

            return restClient.Execute<List<T>>(req).Data;
        }
    }
}
