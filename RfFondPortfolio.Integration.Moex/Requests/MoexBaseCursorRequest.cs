using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal abstract class MoexBaseCursorRequest : MoexBaseRequest<JsonHistoryData>
    {
        protected IList<Tuple<string, string>> _addGetParams;

        public virtual async Task<JsonBase> CursorRead(long start = 0, long count = 0)
        {
            var firstPage = await CursorReadHistory(start);

            var result = firstPage.History;
            var lastItemsCount = firstPage.History.Data.Count;

            var total = firstPage.Cursor?.Total ?? 0;
            if (count > 0)
            {
                total = count;
            }

            while (0 == total || start + lastItemsCount < total)
            {
                start += lastItemsCount;
                var nextPage = await CursorReadHistory(start);
                result.Data.AddRange(nextPage.History.Data);

                lastItemsCount = nextPage.History.Data.Count;
                if (0 == lastItemsCount)
                {
                    _logger.Trace("No cursor data more");
                    break;
                }
            }

            _logger.Trace($"Finish read cursor data, {result.Data.Count} item(s)");
            return result;
        }

        protected async Task<JsonHistoryData> CursorReadHistory(long start)
        {
            var getParams = new List<Tuple<string, string>>();
            if (_addGetParams != null)
            {
                getParams.AddRange(_addGetParams);
            }

            if (start > 0)
            {
                getParams.Add(new Tuple<string, string>("start", start.ToString()));
            }

            var result = await Read(getParams);

            return result;
        }

        protected MoexBaseCursorRequest(ILogger logger)
            : base(logger)
        {
        }
    }
}