using System;
using System.Collections.Generic;
using System.Globalization;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Integration.Moex.JsonDto;

namespace RfFondPortfolio.Integration.Moex.Requests
{
    internal static class StockPaperConverter
    {
        public static PaperType GetPaperType(JsonPaperDefinition definition)
        {
            var group = definition.Description.GetDataForString("name", "GROUP", "value");

            var paperType = PaperType.Unknown;
            switch (group)
            {
                case "stock_shares":
                    paperType = PaperType.Share;
                    break;
                case "stock_etf":
                    paperType = PaperType.Etf;
                    break;
                case "stock_ppif":
                    paperType = PaperType.Ppif;
                    break;
                case "stock_bonds":
                    paperType = PaperType.Bond;
                    break;
                case "stock_dr":
                    paperType = PaperType.DR;
                    break;
            }

            return paperType;
        }

        private static T Map<T>(JsonPaperDefinition definition) where T : AbstractPaper, new()
        {
            var paper = new T
            {
                Id = Guid.NewGuid()
            };

            paper.SecId = definition.Description.GetDataForString("name", "SECID", "value");
            paper.Name = definition.Description.GetDataForString("name", "NAME", "value");
            paper.ShortName = definition.Description.GetDataForString("name", "SHORTNAME", "value");
            paper.Isin = definition.Description.GetDataForString("name", "ISIN", "value");
            paper.FaceValue = definition.Description.GetDataForDecimal("name", "FACEVALUE", "value").GetValueOrDefault();
            paper.IssueDate = definition.Description.GetDataForDateTime("name", "ISSUEDATE", "value");
            paper.IssueSize = definition.Description.GetDataForLong("name", "ISSUESIZE", "value").GetValueOrDefault();
            paper.Type = definition.Description.GetDataForString("name", "TYPE", "value");
            paper.TypeName = definition.Description.GetDataForString("name", "TYPENAME", "value");
            paper.Group = definition.Description.GetDataForString("name", "GROUP", "value");
            paper.GroupName = definition.Description.GetDataForString("name", "GROUPNAME", "value");
            paper.PaperType = GetPaperType(definition);

            paper.Boards = new List<PaperBoard>(definition.Boards.Data.Count);
            foreach (var board in definition.Boards.Data)
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

                paper.Boards.Add(paperBoard);
            }

            return paper;
        }

        public static EtfPaper MapEtf(JsonPaperDefinition definition)
        {
            var paper = Map<EtfPaper>(definition);
            return paper;
        }

        public static SharePaper MapShare(JsonPaperDefinition definition, JsonDividends dividends)
        {
            var paper = Map<SharePaper>(definition);

            paper.RegNumber = definition.Description.GetDataForString("name", "REGNUMBER", "value");

            paper.Dividends = new List<ShareDividend>(dividends.Dividends.Data.Count);
            foreach (var jsonDividend in dividends.Dividends.Data)
            {
                var dividend = new ShareDividend();
                dividend.RegistryCloseDate = Convert.ToDateTime(jsonDividend["registryclosedate"]);
                dividend.Value = Convert.ToDecimal(jsonDividend["value"], CultureInfo.InvariantCulture);
                dividend.Currency = jsonDividend["currencyid"];

                paper.Dividends.Add(dividend);
            }

            return paper;
        }

        public static BondPaper MapBond(JsonPaperDefinition definition, JsonBondization coupons)
        {
            var paper = Map<BondPaper>(definition);

            paper.MatDate = definition.Description.GetDataForDateTime("name", "MATDATE", "value").GetValueOrDefault();
            paper.InitialFaceValue = definition.Description.GetDataForDecimal("name", "INITIALFACEVALUE", "value");
            paper.CouponFrequency = definition.Description.GetDataForLong("name", "COUPONFREQUENCY", "value").GetValueOrDefault();
            paper.CouponPercent = definition.Description.GetDataForDecimal("name", "COUPONPERCENT", "value").GetValueOrDefault();
            paper.EarlyRepayment = definition.Description.GetDataForString("name", "EARLYREPAYMENT", "value") == "1";

            paper.Coupons = new List<BondCoupon>(coupons.Coupons.Data.Count);
            foreach (var jsonCoupon in coupons.Coupons.Data)
            {
                var coupon = new BondCoupon();
                coupon.IssueValue = Convert.ToDecimal(jsonCoupon["issuevalue"], CultureInfo.InvariantCulture);
                coupon.CouponDate = Convert.ToDateTime(jsonCoupon["coupondate"]);
                coupon.RecordDate = Convert.ToDateTime(jsonCoupon["recorddate"]);
                coupon.StartDate = Convert.ToDateTime(jsonCoupon["startdate"]);
                coupon.InitialFaceValue = Convert.ToDecimal(jsonCoupon["initialfacevalue"], CultureInfo.InvariantCulture);
                coupon.FaceValue = Convert.ToDecimal(jsonCoupon["facevalue"], CultureInfo.InvariantCulture);
                coupon.Value = Convert.ToDecimal(jsonCoupon["value"], CultureInfo.InvariantCulture);
                coupon.ValuePercent = Convert.ToDecimal(jsonCoupon["valueprc"], CultureInfo.InvariantCulture);
                coupon.ValueRub = Convert.ToDecimal(jsonCoupon["value_rub"], CultureInfo.InvariantCulture);

                paper.Coupons.Add(coupon);
            }

            paper.Offers = new List<BondOffer>();
            foreach (var jsonOffer in coupons.Offers.Data)
            {
                var offer = new BondOffer();
                offer.OfferDate = Convert.ToDateTime(jsonOffer["offerdate"]);
                offer.OfferDateStart = string.IsNullOrWhiteSpace(jsonOffer["offerdatestart"])
                    ? null
                    : Convert.ToDateTime(jsonOffer["offerdatestart"]);
                offer.OfferDateEnd = string.IsNullOrWhiteSpace(jsonOffer["offerdateend"])
                    ? null
                    : Convert.ToDateTime(jsonOffer["offerdateend"]);
                offer.Price = Convert.ToDecimal(jsonOffer["price"], CultureInfo.InvariantCulture);
                offer.OfferType = jsonOffer["offertype"];

                paper.Offers.Add(offer);
            }

            paper.Amortizations = new List<BondAmortization>();
            foreach (var jsonAmmortization in coupons.Amortizations.Data)
            {
                var ammortization = new BondAmortization();
                ammortization.IssueValue = Convert.ToDecimal(jsonAmmortization["issuevalue"], CultureInfo.InvariantCulture);
                ammortization.AmortDate = Convert.ToDateTime(jsonAmmortization["amortdate"]);
                ammortization.FaceValue = Convert.ToDecimal(jsonAmmortization["facevalue"], CultureInfo.InvariantCulture);
                ammortization.InitialFaceValue = Convert.ToDecimal(jsonAmmortization["initialfacevalue"], CultureInfo.InvariantCulture);
                ammortization.FaceUnit = jsonAmmortization["faceunit"];
                ammortization.ValuePercent = Convert.ToDecimal(jsonAmmortization["valueprc"], CultureInfo.InvariantCulture);
                ammortization.Value = Convert.ToDecimal(jsonAmmortization["value"], CultureInfo.InvariantCulture);
                ammortization.ValueRub = Convert.ToDecimal(jsonAmmortization["value_rub"], CultureInfo.InvariantCulture);
                ammortization.DataSource = jsonAmmortization["data_source"];

                paper.Amortizations.Add(ammortization);
            }

            return paper;
        }
    }
}