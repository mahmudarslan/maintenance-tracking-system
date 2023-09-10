using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.OrderService.DocumentNoFormats
{
    public class DocNoFormatManager : DomainService, IDocNoFormatManager
    {
        private readonly IRepository<OrderDocNoFormat, Guid> _docNoFormatRepository;

        public DocNoFormatManager(IRepository<OrderDocNoFormat, Guid> docNoFormatRepository)
        {
            _docNoFormatRepository = docNoFormatRepository;
        }

        public async Task<string> GenerateNumber(int docType)
        {
            var docNo = await _docNoFormatRepository.FirstOrDefaultAsync(w => w.DocNoType == docType);
            docNo.IncreaseNumber();
            await _docNoFormatRepository.UpdateAsync(docNo);

            return docNo.ToString();
        }

        public async Task Update(Guid id, string prefix, int nextNumber, string suffix)
        {
            var data = await _docNoFormatRepository.FirstOrDefaultAsync(f => f.Id == id);
            data.SetPrefix(prefix);
            data.SetNextNumber(nextNumber);
            data.SetSuffix(suffix);
            await _docNoFormatRepository.UpdateAsync(data, true);
        } 
    }
}