using System;
using System.Collections.Generic;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Данные по акции
    /// </summary>
    public class SharePaper : AbstractPaper
    {
        /// <summary>
        /// Номер государственной регистрации
        /// </summary>
        public string RegNumber { get; set; }

        /// <summary>
        /// Список дивидендов
        /// </summary>
        public IList<ShareDividend> Dividends;

        /// <summary>
        /// Является ли бумага привелигированной акцией
        /// </summary>
        public bool IsPreferedShare => string.Equals("preferred_share", Type, StringComparison.InvariantCultureIgnoreCase);
    }
}