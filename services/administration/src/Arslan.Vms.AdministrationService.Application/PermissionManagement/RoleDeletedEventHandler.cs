﻿using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.PermissionManagement;

namespace Arslan.Vms.AdministrationService.PermissionManagement;

public class RoleDeletedEventHandler :
IDistributedEventHandler<EntityDeletedEto<IdentityRoleEto>>,
ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }

    public RoleDeletedEventHandler(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    public async Task HandleEventAsync(EntityDeletedEto<IdentityRoleEto> eventData)
    {
        await PermissionManager.DeleteAsync(RolePermissionValueProvider.ProviderName, eventData.Entity.Name);
    }
}