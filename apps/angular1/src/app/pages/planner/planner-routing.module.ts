import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SchedulerComponent } from './components/scheduler/scheduler.component';
import { ePlannerComponents } from './enums/components';

const routes: Routes = [
  { path: '', redirectTo: '', pathMatch: 'full' },
  {
    path: '',
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      {
        path: 'scheduler/scheduler',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Order.SalesOrder',
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PlannerRoutingModule {}
