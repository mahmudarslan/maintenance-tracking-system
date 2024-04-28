using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;
using Volo.Abp.Features;
using Volo.Abp.FeatureManagement;
using Volo.Abp.SettingManagement;
using Arslan.Vms.AdministrationService.Saas;

namespace Arslan.Vms.AdministrationService;

internal class AdministrationServiceIntegration
{
	private readonly IRepository<Tenant> _tenantRepository;
	private readonly IRepository<PermissionGrant> _permissionGrantRepository;
	private readonly IRepository<PermissionDefinitionRecord> _permissionDefinitionRecordRepository;
	private readonly IRepository<PermissionGroupDefinitionRecord> _permissionGroupDefinitionRecordRepository;
	private readonly IDataFilter _dataFilter;
	private readonly IGuidGenerator _guidGenerator;
	private readonly IUnitOfWorkManager _unitOfWorkManager;
	IDbContextProvider<AdministrationServiceDbContext> _dbContextProvider;
	private readonly IConfiguration _configuration;
    private readonly ISettingManager _settingManager;
    private readonly IFeatureManagementStore _featureManagementStore;
    private readonly IRepository<SaasEdition> _editionRepository;

    public AdministrationServiceIntegration(IAbpApplicationWithInternalServiceProvider application)
	{
		_tenantRepository = application.ServiceProvider.GetRequiredService<IRepository<Tenant>>();
		_permissionGrantRepository = application.ServiceProvider.GetRequiredService<IRepository<PermissionGrant>>();
		_permissionDefinitionRecordRepository = application.ServiceProvider.GetRequiredService<IRepository<PermissionDefinitionRecord>>();
		_permissionGroupDefinitionRecordRepository = application.ServiceProvider.GetRequiredService<IRepository<PermissionGroupDefinitionRecord>>();
		_guidGenerator = application.ServiceProvider.GetRequiredService<IGuidGenerator>();
		_dataFilter = application.ServiceProvider.GetRequiredService<IDataFilter>();
		_unitOfWorkManager = application.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
		_dbContextProvider = application.ServiceProvider.GetRequiredService<IDbContextProvider<AdministrationServiceDbContext>>();
		_configuration = application.ServiceProvider.GetRequiredService<IConfiguration>();
		_settingManager = application.ServiceProvider.GetRequiredService<ISettingManager>();
        _featureManagementStore = application.ServiceProvider.GetRequiredService<IFeatureManagementStore>();

        _editionRepository = application.ServiceProvider.GetRequiredService<IRepository<SaasEdition>>();
    }

    public async Task SeedDataAsync()
	{
		using (_dataFilter.Disable<IMultiTenant>())
		{
			await CreateDemoTenantAsync();

			using (_dataFilter.Disable<ISoftDelete>())
			{
				if (_configuration["Migration:DeleteAllData"] == "true")
				{
					using (var uow = _unitOfWorkManager.Begin(true))
					{
						//await _permissionGrantRepository.DeleteDirectAsync(d => 1 == 1);
						//await _permissionDefinitionRecordRepository.DeleteDirectAsync(d => 1 == 1);
						//await _permissionGroupDefinitionRecordRepository.DeleteDirectAsync(d => 1 == 1);

						await uow.CompleteAsync();
					}
				}

                await CreateEditionAsync();

				await CreateFeaturesAsync();

                await CreateDemoTenantAsync();
            }
		}
	}

    public async Task CreateEditionAsync()
    {
        var editions = await _editionRepository.ToListAsync();

        if (!editions.Any(w => w.Name != EditionConsts.Enterprise))
            await _editionRepository.InsertAsync(new SaasEdition(id: _guidGenerator.Create(), name: EditionConsts.Enterprise, isStatic: true), true);

        if (!editions.Any(w => w.Name != EditionConsts.Premium))
            await _editionRepository.InsertAsync(new SaasEdition(id: _guidGenerator.Create(), name: EditionConsts.Premium, isStatic: true), true);

        if (!editions.Any(w => w.Name != EditionConsts.Standard))
            await _editionRepository.InsertAsync(new SaasEdition(id: _guidGenerator.Create(), name: EditionConsts.Standard, isStatic: true), true);
    }

    public async Task CreateFeaturesAsync()
    {
        var editions = await _editionRepository.ToListAsync();

        var enterprise = editions.FirstOrDefault(f => f.NormalizedName == EditionConsts.Enterprise.ToUpperInvariant());
        var premium = editions.FirstOrDefault(f => f.NormalizedName == EditionConsts.Premium.ToUpperInvariant());
        var standard = editions.FirstOrDefault(f => f.NormalizedName == EditionConsts.Standard.ToUpperInvariant());
    }

    public async Task CreateDemoTenantAsync()
	{
		var tenants = await _tenantRepository.ToListAsync();
		var environmentName = _configuration["EnvironmentName"];

		var tenantName = "demo";

		if (!string.IsNullOrEmpty(environmentName))
		{
			if (environmentName == "dev")
			{
				tenantName = "dev-demo";
			}
			if (environmentName == "stage")
			{
				tenantName = "stage-demo";
			}
		}

		try
		{
			if (!tenants.Any(a => a.Name == tenantName))
			{
				var query = "INSERT INTO [VmsAdministration].[dbo].[AbpTenants]" +
"([Id],[Name],[EntityVersion],[ExtraProperties],[ConcurrencyStamp],[CreationTime],[CreatorId],[LastModificationTime],[LastModifierId],[IsDeleted],[DeleterId],[DeletionTime])" +
"VALUES" +
"('" + MigrationConst.DemoTenantId.ToString() + "','" + tenantName + "',0,'{{\"Edition\":\"Enterprise\",\"Host\":\"demo.arslan.io\"}}','800152D8-9DFC-69CE-FE28-3A1237310092','2024-01-12 09:15:00.6303327',null,null,null,0,null,null)";

				var result2 = await (await _dbContextProvider.GetDbContextAsync())
					.Database
					.ExecuteSqlRawAsync(query);
			}
		}
		catch (System.Exception ex)
		{
			Log.Error("CreateDemoTenant->" + " " + ex.Message);
		}
	}
}