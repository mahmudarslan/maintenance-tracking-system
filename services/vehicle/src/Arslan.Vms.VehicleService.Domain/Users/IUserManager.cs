using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.VehicleService.Users
{
    public interface IUserManager : IDomainService
    {
        Task<User> CreateAsync([NotNull] User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<User> GetAsync(Guid id);
    }
}
