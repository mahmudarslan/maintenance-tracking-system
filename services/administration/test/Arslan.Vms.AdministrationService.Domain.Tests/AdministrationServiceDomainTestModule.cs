﻿using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.AdministrationService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(AdministrationServiceEntityFrameworkCoreTestModule)
    )]
public class AdministrationServiceDomainTestModule : AbpModule
{

}
