using RestSharp;

namespace RfBondManagement.Engine.Integration.Moex
{
    public abstract class MoexBaseRequest<T>
    {
        protected string _baseUrl;
        protected RestClient _client;

        protected virtual Method _method => Method.GET;

        protected MoexBaseRequest(string baseUrl = "https://iss.moex.com/iss")
        {
            _baseUrl = baseUrl;
            _client = new RestClient(_baseUrl);
        }

        public abstract T Read(string tiker);

        protected virtual T BaseRead(string url)
        {
            var request = new RestRequest($"{url}.json", _method) {RequestFormat = DataFormat.Json};

            var result = _client.Execute<T>(request);

            return result.Data;
        }
    }
}