using System;

namespace RfFondPortfolio.Common.Dtos
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Уникальный номер сущности
        /// </summary>
        public Guid Id { get; set; }
    }
}