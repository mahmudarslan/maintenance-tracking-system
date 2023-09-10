using System;

namespace Arslan.Vms.ProductService.v1.DocumentNoFormats.Dtos
{
    public class DocumentNoFormatDto
    {
        public Guid Id { get; set; }
        public int DocNoType { get; set; }
        public int NextNumber { get; set; }
        public int MinDigits { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}