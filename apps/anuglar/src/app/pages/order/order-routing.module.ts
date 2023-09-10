import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { eOrderComponents } from './enums/components';
import { SalesOrderNewComponent, SalesOrderListComponent, PurchaseOrderListComponent, PurchaseOrderNewComponent } from './components';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      //Sales Order
      {
        path: '',
        redirectTo: ''
      },
      {
        path: 'sales-order/sales-order-new',
        component: SalesOrderNewComponent,
        data: {
          requiredPolicy: 'Order.SalesOrder',
        },
      },
      {
        path: 'sales-order/sales-order-new/:id',
        component: SalesOrderNewComponent,
        data: {
          requiredPolicy: 'Order.SalesOrder',
        },
      },
      {
        path: 'sales-order/sales-order-list',
        component: SalesOrderListComponent,
        data: {
          requiredPolicy: 'Order.SalesOrder.List',
        },
      },
      //Purchase Order
      {
        path: 'purchase-order/purchase-order-new',
        component: PurchaseOrderNewComponent,
        data: {
          requiredPolicy: 'Order.PurchaseOrder',
        },
      },
      {
        path: 'purchase-order/purchase-order-new/:id',
        component: PurchaseOrderNewComponent,
        data: {
          requiredPolicy: 'Order.PurchaseOrder',
        },
      },
      {
        path: 'purchase-order/purchase-order-list',
        component: PurchaseOrderListComponent,
        data: {
          requiredPolicy: 'Order.PurchaseOrder.List',
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrderRoutingModule { }