import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eOrderPolicyNames } from '../enums/policy-names';
import { eOrderRouteNames } from '../enums/route-names';

export const ORDER_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      //Sales Order
      {
        path: '/order/sales-order',
        name: eOrderRouteNames.SalesOrder,
        requiredPolicy: eOrderPolicyNames.SalesOrder,
        iconClass: 'keypad-outline',
        layout: eLayoutType.application,
        order: 51,
      },
      // {
      //   path: '/order/sales-order/sales-order-new',
      //   name: eOrderRouteNames.SalesOrderNew,
      //   parentName: eOrderRouteNames.SalesOrder,
      //   requiredPolicy: eOrderPolicyNames.SalesOrderNew,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 1,
      // },
      {
        path: '/order/sales-order/sales-order-list',
        name: eOrderRouteNames.SalesOrderList,
        parentName: eOrderRouteNames.SalesOrder,
        requiredPolicy: eOrderPolicyNames.SalesOrderList,
        iconClass: 'keypad-outline',
        layout: eLayoutType.application,
        order: 2,
      },
      //Purchase Order
      {
        path: '/order/purchase-order',
        name: eOrderRouteNames.PurchaseOrder,
        requiredPolicy: eOrderPolicyNames.PurchaseOrder,
        iconClass: 'keypad-outline',
        layout: eLayoutType.application,
        order: 50,
      },
      // {
      //   path: '/order/purchase-order/purchase-order-new',
      //   name: eOrderRouteNames.PurchaseOrderNew,
      //   parentName: eOrderRouteNames.PurchaseOrder,
      //   requiredPolicy: eOrderPolicyNames.PurchaseOrderNew,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 1,
      // },
      {
        path: '/order/purchase-order/purchase-order-list',
        name: eOrderRouteNames.PurchaseOrderList,
        parentName: eOrderRouteNames.PurchaseOrder,
        requiredPolicy: eOrderPolicyNames.PurchaseOrderList,
        iconClass: 'keypad-outline',
        layout: eLayoutType.application,
        order: 2,
      },
    ]);
  };
}