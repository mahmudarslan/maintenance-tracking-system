using System;

namespace Arslan.Vms.ProductService.v1.Currencies.Dtos
{
    public class CurrencyConversionDto
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public int CurrencyConversionId { get; set; }
        public int ExchangeRate { get; set; }
    }
}