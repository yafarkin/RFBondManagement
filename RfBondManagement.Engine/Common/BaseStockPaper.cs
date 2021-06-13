using System;
using System.Collections.Generic;
using System.Linq;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Common
{
    public class BaseStockPaper
    {
        /// <summary>
        /// Внутренний идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Код ценной бумаги
        /// </summary>
        public string SecId { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// ISIN код
        /// </summary>
        public string Isin { get; set; }

        /// <summary>
        /// Номинальная стоимость
        /// </summary>
        public decimal FaceValue { get; set; }

        /// <summary>
        /// Дата начала торгов
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Объем выпуска
        /// </summary>
        public long IssueSize { get; set; }

        /// <summary>
        /// Вид/категория ценной бумаги
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Код типа инструмента
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Тип бумаги
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Тип инструмента
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Дата погашения
        /// </summary>
        public DateTime? MatDate { get; set; }

        /// <summary>
        /// Первоначальная номинальная стоимость
        /// </summary>
        public decimal? InitialFaceValue { get; set; }

        /// <summary>
        /// Периодичность выплаты купона в год
        /// </summary>
        public long? CouponFrequency { get; set; }

        /// <summary>
        /// Возможен досрочный выкуп
        /// </summary>
        public bool EarlyRepayment { get; set; }

        public bool IsShare => string.Equals("stock_shares", Group, StringComparison.InvariantCultureIgnoreCase);

        public bool IsPreferedShare => IsShare && string.Equals("preferred_share", Type, StringComparison.InvariantCultureIgnoreCase);

        public bool IsEtf => string.Equals("stock_etf", Group, StringComparison.InvariantCultureIgnoreCase);

        public bool IsBond => string.Equals("stock_bonds", Group, StringComparison.InvariantCultureIgnoreCase);

        public bool IsOfzBond => IsBond && string.Equals("ofz_bond", Type, StringComparison.InvariantCultureIgnoreCase);

        public bool IsDR => string.Equals("stock_dr", Group, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Список досок
        /// </summary>
        public IList<PaperBoard> Boards { get; set; }

        public PaperBoard PrimaryBoard => Boards?.Single(b => b.IsPrimary);

        /// <summary>
        /// Список офферов (по выкупу), для облигаций
        /// </summary>
        public IList<BondOffer> Offers { get; set; }

        public DateTime? OfferDate => Offers != null && Offers.Count > 0 ? Offers[0].OfferDate : (DateTime?)null;

        /// <summary>
        /// Список выплат по купонам, для облигаций
        /// </summary>
        public List<BondCoupon> Coupons { get; set; }


        [Obsolete]
        public List<PriceOnDate> Price { get; set; }

        [Obsolete]
        public PriceOnDate LastPrice => Price?.OrderByDescending(p => p.Date).FirstOrDefault();

        public override string ToString() => Name;
    }
}
