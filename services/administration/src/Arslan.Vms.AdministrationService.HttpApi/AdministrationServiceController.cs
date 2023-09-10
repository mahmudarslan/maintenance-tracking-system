﻿using Arslan.Vms.AdministrationService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.AdministrationService;

public abstract class AdministrationServiceController : AbpControllerBase
{
    protected AdministrationServiceController()
    {
        LocalizationResource = typeof(AdministrationServiceResource);
    }
}
