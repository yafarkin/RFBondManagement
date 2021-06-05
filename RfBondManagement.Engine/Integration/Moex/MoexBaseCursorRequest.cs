using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public abstract class MoexBaseCursorRequest : MoexBaseRequest<JsonHistoryData>
    {
        protected IList<Tuple<string, string>> _addGetParams;

        public virtual JsonBase CursorRead(long start = 0, long count = 0)
        {
            var firstPage = CursorReadHistory(start);

            var result = firstPage.History;
            var lastItemsCount = firstPage.History.Data.Count;

            var total = firstPage.Cursor.Total;
            if (count > 0)
            {
                total = count;
            }

            while (start + lastItemsCount < total)
            {
                start += lastItemsCount;
                var nextPage = CursorReadHistory(start);
                result.Data.AddRange(nextPage.History.Data);

                lastItemsCount = nextPage.History.Data.Count;
                if (0 == lastItemsCount)
                {
                    break;
                }
            }

            return result;
        }

        protected JsonHistoryData CursorReadHistory(long start)
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

            var result = Read(getParams);

            return result;
        }
    }
}