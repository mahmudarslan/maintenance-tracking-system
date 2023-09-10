using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.DocumentNoFormats
{
    public interface IDocNoFormatManager : IDomainService
    {
        Task<string> GenerateNumber(int docType);
        Task Update(Guid id, string prefix, int nextNumber, string suffix);
    }
}