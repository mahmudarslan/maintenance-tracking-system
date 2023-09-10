using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Arslan.Vms.ProductService.Currencies
{
    public class Currency : BasicAggregateRoot<Guid>, ISoftDelete
    {
        public virtual string Code { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string Symbol { get; protected set; }
        public virtual short DecimalPlaces { get; protected set; }
        public virtual string DecimalSeparator { get; protected set; }
        public virtual string ThousandsSeparator { get; protected set; }
        public virtual short CRCurrencyPositionType { get; protected set; }
        public virtual short CRNegativeType { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        protected Currency() { }

        public Currency(Guid id, string code, string description, string symbol, short decimalPlaces,
            string decimalSeparator, string thousandsSeparator,
            short cRCurrencyPositionType, short cRNegativeType) : base(id)
        {
            Code = code;
            Description = description;
            Symbol = symbol;
            DecimalPlaces = decimalPlaces;
            DecimalSeparator = decimalSeparator;
            ThousandsSeparator = thousandsSeparator;
            CRCurrencyPositionType = cRCurrencyPositionType;
            CRNegativeType = cRNegativeType;
        }
    }
}