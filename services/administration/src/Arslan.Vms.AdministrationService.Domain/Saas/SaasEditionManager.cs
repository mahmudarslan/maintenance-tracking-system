using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.AdministrationService.Saas;

public class SaasEditionManager : DomainService, ISaasEditionManager
{
    protected ISaasEditionRepository EditionRepository { get; }

    public SaasEditionManager(ISaasEditionRepository editionRepository)
    {
        EditionRepository = editionRepository;
    }

    public virtual async Task<SaasEdition> CreateAsync(string name)
    {
        Check.NotNull(name, nameof(name));
        await ValidateNameAsync(name);

        var edition = new SaasEdition(GuidGenerator.Create(), name);
        await EditionRepository.InsertAsync(edition);
        return edition;
    }

    public virtual async Task ChangeNameAsync(SaasEdition edition, string name)
    {
        Check.NotNull(edition, nameof(edition));

        await ValidateNameAsync(name, edition.Id);
        edition.SetName(name);
    }

    protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
    {
        var edition = await EditionRepository.FindByNameAsync(name);

        if (edition != null && edition.Id != expectedId)
        {
            throw new UserFriendlyException("Duplicate edition name: " + name);
        }
    }
}
