using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace RfBondManagement.Engine.Integration.Moex
{
    public abstract class MoexBaseRequest<TMoex>
    {
        protected const string BASE_MOEX_URL = "https://iss.moex.com/iss";

        protected RestClient _client;

        protected virtual Method _method => Method.GET;

        protected abstract string _requestUrl { get; }

        protected MoexBaseRequest()
        {
            _client = new RestClient(BASE_MOEX_URL);
        }

        public virtual TMoex Read(IEnumerable<Tuple<string, string>> getParams = null)
        {
            var request = new RestRequest($"{BASE_MOEX_URL}{_requestUrl}.json", _method) {RequestFormat = DataFormat.Json};
            if (getParams != null)
            {
                foreach (var getParam in getParams)
                {
                    request.AddParameter(getParam.Item1, getParam.Item2, ParameterType.QueryString);
                }
            }

            var response = _client.Execute(request);

            var result = JsonConvert.DeserializeObject<TMoex>(response.Content);

            return result;
        }
    }
}