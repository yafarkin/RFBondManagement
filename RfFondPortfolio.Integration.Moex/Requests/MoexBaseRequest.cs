using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal abstract class MoexBaseRequest<TMoex>
    {
        protected const string BASE_MOEX_URL = "https://iss.moex.com/iss";

        protected RestClient _client;

        protected virtual Method _method => Method.GET;

        protected abstract string _requestUrl { get; }

        protected virtual IEnumerable<Tuple<string, string>> _additionalParams { get; }

        protected ILogger _logger;

        protected MoexBaseRequest(ILogger logger)
        {
            _logger = logger;
            _client = new RestClient(BASE_MOEX_URL);
        }

        public virtual async Task<TMoex> Read(IEnumerable<Tuple<string, string>> getParams = null)
        {
            var request = new RestRequest($"{BASE_MOEX_URL}{_requestUrl}.json", _method) {RequestFormat = DataFormat.Json};

            var ap = _additionalParams;
            if (ap != null)
            {
                foreach (var p in ap)
                {
                    request.AddParameter(p.Item1, p.Item2, ParameterType.QueryString);
                }
            }

            if (getParams != null)
            {
                foreach (var p in getParams)
                {
                    request.AddParameter(p.Item1, p.Item2, ParameterType.QueryString);
                }
            }

            try
            {
                var uri = _client.BuildUri(request);
                _logger.Trace($"Request to {uri}");

                var response = await _client.ExecuteAsync(request);

                _logger.Trace($"Success, R = {(string.IsNullOrWhiteSpace(response.Content) ? "NULL" : response.Content.Substring(0, 50) + "..., len = " + response.Content.Length)}");

                var result = JsonConvert.DeserializeObject<TMoex>(response.Content);

                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Fail");
                throw;
            }
        }
    }
}