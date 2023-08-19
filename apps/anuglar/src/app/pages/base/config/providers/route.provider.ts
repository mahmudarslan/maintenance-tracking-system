import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eBasePolicyNames } from '../enums/policy-names';
import { eBaseRouteNames } from '../enums/route-names';

export const BASE_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      //Employee
      {
        path: '/base/employee',
        name: eBaseRouteNames.Employees,
        requiredPolicy: eBasePolicyNames.Employees,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 10,
      },
      // {
      //   path: '/base/employee/employee-new',
      //   name: eBaseRouteNames.EmployeesNew,
      //   parentName: eBaseRouteNames.Employees,
      //   requiredPolicy: eBasePolicyNames.EmployeesNew,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 1,
      // },
      {
        path: '/base/employee/employee-list',
        name: eBaseRouteNames.EmployeeList,
        parentName: eBaseRouteNames.Employees,
        requiredPolicy: eBasePolicyNames.EmployeeList,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 2,
      },
      //Customer
      {
        path: '/base/customer',
        name: eBaseRouteNames.Customers,
        requiredPolicy: eBasePolicyNames.Customers,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 12,
      },
      // {
      //   path: '/base/customer/customer-new',
      //   name: eBaseRouteNames.CustomerNew,
      //   parentName: eBaseRouteNames.Customers,
      //   requiredPolicy: eBasePolicyNames.CustomerNew,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 1,
      // },
      {
        path: '/base/customer/customer-list',
        name: eBaseRouteNames.CustomerList,
        parentName: eBaseRouteNames.Customers,
        requiredPolicy: eBasePolicyNames.CustomerList,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 2,
      },
      {
        path: '/base/customer/vehicle-list',
        name: eBaseRouteNames.VehicleList,
        parentName: eBaseRouteNames.Customers,
        requiredPolicy: eBasePolicyNames.CustomerVehicleList,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 2,
      },
      //Vendor
      {
        path: '/base/vendor',
        name: eBaseRouteNames.Vendor,
        requiredPolicy: eBasePolicyNames.Vendor,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 13,
      },
      // {
      //   path: '/base/vendor/vendor-new',
      //   name: eBaseRouteNames.VendorNew,
      //   parentName: eBaseRouteNames.Vendor,
      //   requiredPolicy: eBasePolicyNames.VendorNew,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 1,
      // },
      {
        path: '/base/vendor/vendor-list',
        name: eBaseRouteNames.VendorList,
        parentName: eBaseRouteNames.Vendor,
        requiredPolicy: eBasePolicyNames.VendorList,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 2,
      },
      //Identity Management
      {
        path: '/base',
        name: eBaseRouteNames.IdentityManagement,
        parentName: eBaseRouteNames.IdentityManagement,
        requiredPolicy: eBasePolicyNames.IdentityManagement,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 3,
      },
      {
        path: '/base/company',
        name: eBaseRouteNames.Company,
        parentName: eBaseRouteNames.IdentityManagement,
        requiredPolicy: eBasePolicyNames.Company,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 3,
      },
      {
        path: '/base/address/address-tree',
        name: eBaseRouteNames.Address,
        parentName: eBaseRouteNames.IdentityManagement,
        requiredPolicy: eBasePolicyNames.Address,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 4,
      }
      ,
      // {
      //   path: '/base/vehicle/vehicle-new',
      //   name: eBaseRouteNames.VehicleNew,
      //   parentName: eBaseRouteNames.IdentityManagement,
      //   requiredPolicy: eBasePolicyNames.VehicleType,
      //   iconClass: './assets/media/svg/icons/Home/Library.svg',
      //   layout: eLayoutType.application,
      //   order: 5,
      // },
      {
        path: '/base/doc-numbers',
        name: eBaseRouteNames.DocNumbers,
        parentName: eBaseRouteNames.IdentityManagement,
        requiredPolicy: eBasePolicyNames.DocNumbers,
        iconClass: 'layout-outline',
        layout: eLayoutType.application,
        order: 60,
      },

    ]);
  };
}