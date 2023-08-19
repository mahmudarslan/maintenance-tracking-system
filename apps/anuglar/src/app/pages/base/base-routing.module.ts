import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import {
  CustomerListComponent,
  CustomerNewComponent,
  EmployeeNewComponent,
  EmployeeListComponent,
  VehicleTypeTreeComponent,
  VehicleListComponent,
  VendorNewComponent,
  VendorListComponent,
  AddressComponent,
  DocNumbersComponent,
  ProductListComponent,
  ProductNewComponent,
  VehicleComponent,
} from './components';
import { eBaseComponents } from './enums';
import { CompanyComponent } from './components/company/company.component';
import { AddressTreeComponent } from './components/address/address-tree/addresstree.component';
import { CategoryComponent } from './components/product/category/category.component';
import { LocationComponent } from './components/location/location.component';

const routes: Routes = [
  { path: '', redirectTo: '', pathMatch: 'full' },
  {
    path: '',
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      //Customer
      {
        path: 'customer/customer-new',
        component: CustomerNewComponent,
        data: {
          requiredPolicy: 'Base.Customer',
        },
      },
      {
        path: 'customer/customer-new/:id',
        component: CustomerNewComponent,
        data: {
          requiredPolicy: 'Base.Customer',
        },
      },
      {
        path: 'customer/customer-list',
        component: CustomerListComponent,
        data: {
          requiredPolicy: 'Base.Customer.List',
        },
      },
      {
        path: 'customer/vehicle-list',
        component: VehicleComponent,
        data: {
          requiredPolicy: 'Base.Vehicle.List',
        },
      },
      //Employee
      {
        path: 'employee/employee-new',
        component: EmployeeNewComponent,
        data: {
          requiredPolicy: 'Base.Employee',
        },
      },
      {
        path: 'employee/employee-new/:id',
        component: EmployeeNewComponent,
        data: {
          requiredPolicy: 'Base.Employee',
        },
      },
      {
        path: 'employee/employee-list',
        component: EmployeeListComponent,
        data: {
          requiredPolicy: 'Base.Employee.List',
        },
      },
      //Vendor
      {
        path: 'vendor/vendor-new',
        component: VendorNewComponent,
        data: {
          requiredPolicy: 'Base.Vendor',
        },
      },
      {
        path: 'vendor/vendor-new/:id',
        component: VendorNewComponent,
        data: {
          requiredPolicy: 'Base.Vendor',
        },
      },
      {
        path: 'vendor/vendor-list',
        component: VendorListComponent,
        data: {
          requiredPolicy: 'Base.Vendor.List',
        },
      },
      //Product
      {
        path: 'product/product-new',
        component: ProductNewComponent,
        data: {
          requiredPolicy: 'Product.Product',
        },
      },
      {
        path: 'product/product-list',
        component: ProductListComponent,
        data: {
          requiredPolicy: 'Product.Product.List',
        },
      },
      //Inventory Management
      //Category
      {
        path: 'product/category',
        component: CategoryComponent,
        data: {
          requiredPolicy: 'Product.Category.Update',
        },
      },
      //Location
      {
        path: 'location',
        component: LocationComponent,
        data: {
          requiredPolicy: 'Product.Location.Update',
        },
      },
      //Identity Management
      {
        path: 'vehicle/vehicle-new',
        component: VehicleTypeTreeComponent,
        data: {
          requiredPolicy: 'Base.Vehicle.Update',
        },
      },
      {
        path: 'vehicle/vehicle-new/:id',
        component: VehicleTypeTreeComponent,
        data: {
          requiredPolicy: 'Base.Vehicle.Update',
        },
      },
      {
        path: 'company',
        component: CompanyComponent,
        data: {
          requiredPolicy: 'Base.Company.Update',
        },
      },
      {
        path: 'company/:id',
        component: CompanyComponent,
        data: {
          requiredPolicy: 'Base.Company.Update',
        },
      },
      {
        path: 'address/address-tree',
        component: AddressComponent,
        data: {
          requiredPolicy: 'Base.Address.Update',
        },
      },
      {
        path: 'doc-numbers',
        component: DocNumbersComponent,
        data: {
          requiredPolicy: 'Base.DocNumber',
        },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BaseRoutingModule { }