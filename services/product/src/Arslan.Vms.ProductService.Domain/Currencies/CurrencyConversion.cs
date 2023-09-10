using System;
using Volo.Abp.Domain.Entities;

namespace Arslan.Vms.ProductService.Currencies
{
    public class CurrencyConversion : BasicAggregateRoot<Guid>
    {
        public virtual int CurrencyConversionId { get; protected set; }
        public virtual Guid CurrencyId { get; protected set; }
        public virtual int ExchangeRate { get; protected set; }

        protected CurrencyConversion() { }

        public CurrencyConversion(Guid id, int currencyConversionId, Guid currencyId, int exchangeRate) : base(id)
        {
            CurrencyConversionId = currencyConversionId;
            CurrencyId = currencyId;
            ExchangeRate = exchangeRate;
        }
    }
}