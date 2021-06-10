using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Данные по дивиденду
    /// </summary>
    public class ShareDividend
    {
        /// <summary>
        /// Дата закрытия реестра
        /// </summary>
        public DateTime RegistryCloseDate { get; set; }

        /// <summary>
        /// Сумма дивиденда на 1 бумагу
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }
    }
}