using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Channels;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public static class StockPaperConverter
    {
        private static T Map<T>(JsonPaperDefinition paper) where T : BaseStockPaper, new()
        {
            var result = new T
            {
                Id = Guid.NewGuid()
            };

            result.SecId = paper.Description.GetDataForString("name", "SECID", "value");
            result.Name = paper.Description.GetDataForString("name", "NAME", "value");
            result.ShortName = paper.Description.GetDataForString("name", "SHORTNAME", "value");
            result.Isin = paper.Description.GetDataForString("name", "ISIN", "value");
            result.FaceValue = paper.Description.GetDataForDecimal("name", "FACEVALUE", "value");
            result.IssueDate = paper.Description.GetDataForDateTime("name", "ISSUEDATE", "value");
            result.TypeName = paper.Description.GetDataForString("name", "TYPENAME", "value");
            result.Group = paper.Description.GetDataForString("name", "GROUP", "value");
            result.Type = paper.Description.GetDataForString("name", "TYPE", "value");

            return result;
        }

        public static BaseSharePaper Map(JsonPaperDefinition paper)
        {
            return Map<BaseSharePaper>(paper);
        }

        public static BaseBondPaper Map(JsonPaperDefinition paper, JsonBondization coupons)
        {
            var result = Map<BaseBondPaper>(paper);

            result.Coupons = new List<BondCoupon>(coupons.Coupons.Data.Count);

            foreach (var jsonCoupon in coupons.Coupons.Data)
            {
                var coupon = new BondCoupon();
                coupon.Date = Convert.ToDateTime(jsonCoupon["coupondate"]);
                coupon.Value = Convert.ToDecimal(jsonCoupon["value"], CultureInfo.InvariantCulture);

                result.Coupons.Add(coupon);
            }

            return result;
        }
    }
}