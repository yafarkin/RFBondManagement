using System;
using LiteDB;

namespace RfBondManagement.Engine.Common
{
    public class CurrencyCourse
    {
        public ObjectId UsdRubCourseId { get; set; }

        public DateTime Date { get; set; }
        public decimal Course { get; set; }
    }
}