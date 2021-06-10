using System;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Абстрактная бумага
    /// </summary>
    public abstract class AbstractPaper
    {
        /// <summary>
        /// Внутренний идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Код ценной бумаги. Основной ключ
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
        /// Объём выпуска
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
        /// Является ли бумага акцией
        /// </summary>
        public bool IsShare => string.Equals("stock_shares", Group, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Является ли бумага привелигированной акцией
        /// </summary>
        public bool IsPreferedShare => IsShare && string.Equals("preferred_share", Type, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Является ли бумага ETF
        /// </summary>
        public bool IsEtf => string.Equals("stock_etf", Group, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Является ли бумага паями ПИФов
        /// </summary>
        public bool IsPpif => string.Equals("stock_etf", Group, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Является ли бумага облигацией
        /// </summary>
        public bool IsBond => string.Equals("stock_bonds", Group, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Является ли бумага ОФЗ
        /// </summary>
        public bool IsOfzBond => IsBond && string.Equals("ofz_bond", Type, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Является ли бумага депозитарной распиской
        /// </summary>
        public bool IsDR => string.Equals("stock_dr", Group, StringComparison.InvariantCultureIgnoreCase);
    }
}