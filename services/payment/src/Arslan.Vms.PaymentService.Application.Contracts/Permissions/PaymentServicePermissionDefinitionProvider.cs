﻿using Arslan.Vms.PaymentService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Arslan.Vms.PaymentService.Permissions;

public class PaymentServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PaymentServicePermissions.GroupName, L("Permission:PaymentService"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PaymentServiceResource>(name);
    }
}
