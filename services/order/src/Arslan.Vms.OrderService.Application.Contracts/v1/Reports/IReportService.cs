using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.OrderService.v1.Reports
{
    public interface IReportAppService : IApplicationService, IRemoteService
    {
        Task<MemoryStream> Download(string workorderNo, string basePath);
    }
}