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

internal class PlannerServiceIntegration
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

	public PlannerServiceIntegration(IAbpApplicationWithInternalServiceProvider application)
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
            using (_dataFilter.Disable<ISoftDelete>())
            {
                if (_configuration["Migration:DeleteAllData"] == "true")
                {
                    using (var uow = _unitOfWorkManager.Begin(true))
                    {

                        await uow.CompleteAsync();
                    }
                }
            }
        }
    }
}