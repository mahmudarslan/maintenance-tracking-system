import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eInventoryPolicyNames } from '../enums/policy-names';
import { eInventoryRouteNames } from '../enums/route-names';

export const INVENTORY_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      //Inventory
      {
        path: '/inventory',
        name: eInventoryRouteNames.Inventory,
        requiredPolicy: eInventoryPolicyNames.AdjustStock,
        iconClass: 'edit-2-outline',
        layout: eLayoutType.application,
        order: 40,
      },
      //Product
      // {
      //   path: '/inventory/product/product-new',
      //   name: eInventoryRouteNames.NewProduct,
      //   parentName: eInventoryRouteNames.Inventory,
      //   requiredPolicy: eInventoryPolicyNames.NewProduct,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 1,
      // },
      // {
      //   path: '/inventory/product/product-list',
      //   name: eInventoryRouteNames.ProductList,
      //   parentName: eInventoryRouteNames.Inventory,
      //   requiredPolicy: eInventoryPolicyNames.ProductList,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 2,
      // },
      //Stock Adjustment
      // {
      //   path: '/inventory/adjust-stock-new',
      //   name: eInventoryRouteNames.AdjustStockNew,
      //   parentName: eInventoryRouteNames.Inventory,
      //   requiredPolicy: eInventoryPolicyNames.AdjustStockNew,
      //   iconClass: './assets/media/icons/duotune/communication/com012.svg',
      //   layout: eLayoutType.application,
      //   order: 2,
      // },
      {
        path: '/inventory/adjust-stock-list',
        name: eInventoryRouteNames.AdjustStockList,
        parentName: eInventoryRouteNames.Inventory,
        requiredPolicy: eInventoryPolicyNames.AdjustStockList,
        iconClass: 'edit-2-outline',
        layout: eLayoutType.application,
        order: 3,
      },
      {
        path: '/inventory/current-stock',
        name: eInventoryRouteNames.CurrentStock,
        parentName: eInventoryRouteNames.Inventory,
        requiredPolicy: eInventoryPolicyNames.CurrentStock,
        iconClass: 'edit-2-outline',
        layout: eLayoutType.application,
        order: 4,
      },

      //Inventory Management
      {
        path: '/inventoryManagement',
        name: eInventoryRouteNames.InventoryManagement,
        parentName: eInventoryRouteNames.Inventory,
        requiredPolicy: eInventoryPolicyNames.InventoryManagement,
        iconClass: 'edit-2-outline',
        layout: eLayoutType.application,
        order: 2,
      },
      {
        path: '/inventory/product/category',
        name: eInventoryRouteNames.ProductCategory,
        parentName: eInventoryRouteNames.InventoryManagement,
        requiredPolicy: eInventoryPolicyNames.Category,
        iconClass: 'edit-2-outline',
        layout: eLayoutType.application,
        order: 1,
      },
      {
        path: '/inventory/location',
        name: eInventoryRouteNames.Location,
        parentName: eInventoryRouteNames.InventoryManagement,
        requiredPolicy: eInventoryPolicyNames.Location,
        iconClass: 'edit-2-outline',
        layout: eLayoutType.application,
        order: 2,
      },
    ]);
  };
}