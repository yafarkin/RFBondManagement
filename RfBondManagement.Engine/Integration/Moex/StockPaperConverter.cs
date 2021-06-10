using System;
using System.Collections.Generic;
using System.Globalization;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public static class StockPaperConverter
    {
        public static BaseStockPaper Map(JsonPaperDefinition paper)
        {
            var result = new BaseStockPaper
            {
                Id = Guid.NewGuid()
            };

            result.SecId = paper.Description.GetDataForString("name", "SECID", "value");
            result.Name = paper.Description.GetDataForString("name", "NAME", "value");
            result.ShortName = paper.Description.GetDataForString("name", "SHORTNAME", "value");
            result.Isin = paper.Description.GetDataForString("name", "ISIN", "value");
            result.FaceValue = paper.Description.GetDataForDecimal("name", "FACEVALUE", "value").GetValueOrDefault();
            result.IssueDate = paper.Description.GetDataForDateTime("name", "ISSUEDATE", "value");
            result.IssueSize = paper.Description.GetDataForLong("name", "ISSUESIZE", "value").GetValueOrDefault();
            result.Type = paper.Description.GetDataForString("name", "TYPE", "value");
            result.TypeName = paper.Description.GetDataForString("name", "TYPENAME", "value");
            result.Group = paper.Description.GetDataForString("name", "GROUP", "value");
            result.GroupName = paper.Description.GetDataForString("name", "GROUPNAME", "value");
            result.MatDate = paper.Description.GetDataForDateTime("name", "MATDATE", "value");
            result.InitialFaceValue = paper.Description.GetDataForDecimal("name", "INITIALFACEVALUE", "value");
            result.CouponFrequency = paper.Description.GetDataForLong("name", "COUPONFREQUENCY", "value");
            result.EarlyRepayment = paper.Description.GetDataForString("name", "EARLYREPAYMENT", "value") == "1";

            result.Boards = new List<PaperBoard>(paper.Boards.Data.Count);
            foreach (var board in paper.Boards.Data)
            {
                var paperBoard = new PaperBoard();
                paperBoard.BoardId = board["boardid"];
                paperBoard.Title = board["title"];
                paperBoard.Market = board["market"];
                paperBoard.Engine = board["engine"];
                paperBoard.IsTraded = board["is_traded"] == "1";
                paperBoard.HistoryFrom = Convert.ToDateTime(board["history_from"]);
                paperBoard.HistoryTill = Convert.ToDateTime(board["history_till"]);
                paperBoard.IsPrimary = board["is_primary"] == "1";
                paperBoard.Currency = board["currencyid"];

                result.Boards.Add(paperBoard);
            }

            return result;
        }

        // TODO: Add dividends info
        public static BaseStockPaper MapShare(BaseStockPaper paper, JsonBase dividends)
        {
            return paper;
        }

        public static void MapBond(BaseStockPaper paper, JsonBondization coupons)
        {
            paper.Coupons = new List<BondCoupon>(coupons.Coupons.Data.Count);

            foreach (var jsonCoupon in coupons.Coupons.Data)
            {
                var coupon = new BondCoupon();
                coupon.Date = Convert.ToDateTime(jsonCoupon["coupondate"]);
                coupon.Value = Convert.ToDecimal(jsonCoupon["value"], CultureInfo.InvariantCulture);

                paper.Coupons.Add(coupon);
            }

            paper.Offers = new List<PaperOffer>();
            foreach (var jsonOffer in coupons.Offers.Data)
            {
                var offer = new PaperOffer();
                offer.OfferDate = Convert.ToDateTime(jsonOffer["offerdate"]);
                offer.OfferDateStart = string.IsNullOrWhiteSpace(jsonOffer["offerdatestart"])
                    ? (DateTime?)null
                    : Convert.ToDateTime(jsonOffer["offerdatestart"]);
                offer.OfferDateEnd = string.IsNullOrWhiteSpace(jsonOffer["offerdateend"])
                    ? (DateTime?)null
                    : Convert.ToDateTime(jsonOffer["offerdateend"]);
                offer.Price = Convert.ToDecimal(jsonOffer["price"], CultureInfo.InvariantCulture);
                offer.OfferType = jsonOffer["offertype"];

                paper.Offers.Add(offer);
            }
        }
    }
}