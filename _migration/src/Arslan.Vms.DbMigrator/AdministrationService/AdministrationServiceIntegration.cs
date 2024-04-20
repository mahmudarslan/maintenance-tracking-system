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
			}
		}
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
"('" + MigrationConst.DemoTenantId.ToString() + "','" + tenantName + "',0,'{{\"Edition\":\"Enterprise\",\"Host\":\"demo.arslan.io\"}}','55D067DE-FE2B-8906-F578-3A12088824B5','2023-08-16 09:15:00.6303327',null,null,null,0,null,null)";

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