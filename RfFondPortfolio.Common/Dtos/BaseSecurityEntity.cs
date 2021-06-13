namespace RfFondPortfolio.Common.Dtos
{
    public abstract class BaseSecurityEntity : BaseEntity
    {
        /// <summary>
        /// Тикер бумаги (если применимо)
        /// </summary>
        public string SecId { get; set; }
    }
}