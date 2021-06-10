using System;
using System.Collections.Generic;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Данные по облигации
    /// </summary>
    public class BondPaper : AbstractPaper
    {
        /// <summary>
        /// Дата погашения
        /// </summary>
        public DateTime MatDate { get; set; }

        /// <summary>
        /// Периодичность выплаты купона в год
        /// </summary>
        public int CouponFrequency { get; set; }

        /// <summary>
        /// Список записей по амортизации
        /// </summary>
        public IList<BondAmortization> Amortizations { get; set; }

        /// <summary>
        /// Список купонных выплат
        /// </summary>
        public IList<BondCoupon> Coupons { get; set; }

        /// <summary>
        /// Список офферов
        /// </summary>
        public IList<BondOffer> Offers { get; set; }
    }
}