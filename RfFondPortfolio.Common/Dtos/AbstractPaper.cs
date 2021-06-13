using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Тип бумаги
        /// </summary>
        public PaperType PaperType { get; set; }

        /// <summary>
        /// Список досок
        /// </summary>
        public IList<PaperBoard> Boards { get; set; }

        /// <summary>
        /// Основная доска
        /// </summary>
        public PaperBoard PrimaryBoard => Boards?.Single(b => b.IsPrimary);
    }
}