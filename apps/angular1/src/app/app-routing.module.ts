import { AuthGuard, authGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [ 
  {
    canActivate: [authGuard],
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  {
    path: 'base',
    loadChildren: () => import('@arslan/vms.base').then((m) => m.BaseModule.forLazy()),
  },
  {
    path: 'inventory',
    loadChildren: () => import('@arslan/vms.inventory').then(m => m.InventoryModule.forLazy()),
  },
  {
    path: 'order',
    loadChildren: () => import('@arslan/vms.order').then(m => m.OrderModule.forLazy()),
  },
  {
    path: 'payment',
    loadChildren: () => import('@arslan/vms.payment').then(m => m.PaymentModule.forLazy()),
  },
  {
    path: 'planner',
    loadChildren: () => import('@arslan/vms.planner').then(m => m.PlannerModule.forLazy()),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
