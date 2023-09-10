import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {
  AuthGuard,
  PermissionGuard,
} from '@abp/ng.core';
import { eInventoryComponents } from './enums';
import { eInventoryPolicyNames } from './config/enums';
import {
  AdjustStockNewComponent,
  CurrentStockComponent,
  ReorderStockComponent,
  AdjustStockListComponent
} from './components';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      {
        path: '',
        redirectTo: ''
      },
      {
        path: 'adjust-stock-new',
        component: AdjustStockNewComponent,
        data: {
          requiredPolicy: 'Inventory.AdjustStock',
        },
      },
      {
        path: 'adjust-stock-list',
        component: AdjustStockListComponent,
        data: {
          requiredPolicy: 'Inventory.AdjustStock',
        },
      },
      {
        path: 'current-stock',
        component: CurrentStockComponent,
        data: {
          requiredPolicy: 'AppInventorykManagement.CurrentStock',
        },
      },
      {
        path: 'reorder-stock',
        component: ReorderStockComponent,
        data: {
          requiredPolicy: 'AppInventoryManagement.ReorderStock',
        },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule { }