using System;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonCursor : JsonBase
    {
        public long Index => Convert.ToInt64(GetDataFor("INDEX"));
        public long Total => Convert.ToInt64(GetDataFor("TOTAL"));
        public long PageSize => Convert.ToInt64(GetDataFor("PAGESIZE"));
    }
}