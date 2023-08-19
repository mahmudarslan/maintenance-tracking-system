import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxDataGridModule } from 'devextreme-angular';
import { DxFormModule } from 'devextreme-angular';
import { DxButtonModule } from 'devextreme-angular';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { BaseRoutingModule } from './base-routing.module';
import { NgxsModule } from '@ngxs/store';
import { BaseState } from './states/base.state';
//import { CoreModule as VmsCore } from '@arslan/vms.base';
import {
  CustomerNewComponent,
  CustomerListComponent,
  EmployeeNewComponent,
  EmployeeListComponent,
  VehicleTypeTreeComponent,
  VehicleListComponent,
  VendorListComponent,
  VendorNewComponent,
  AddressComponent,
  AddressTreeComponent,
  VehicleComponent,
  UserComponent,
  DocNumbersComponent,  
  ProductNewComponent,
  ProductListComponent
} from './components';
import { LocationComponent } from './components/location/location.component';
import { CategoryComponent } from './components/product/category/category.component';
import {
  DxTabPanelModule,
  DxCheckBoxModule,
  DxTemplateModule,
  DxTabsModule,
  DxSelectBoxModule,
  DxTextBoxModule,
  DxDateBoxModule,
  DxValidatorModule,
  DxValidationSummaryModule,
  DxAccordionModule,
  DxSliderModule,
  DxTagBoxModule,
  DxBoxModule,
  DxTextAreaModule,
  DxLoadIndicatorModule,
  DxLoadPanelModule,
  DxToastModule,
  DxTooltipModule,
  DxTreeListModule,
  DxFileUploaderModule,
  DxValidationGroupModule,
  DxPopupModule,
  DxScrollViewModule,
  DxNumberBoxModule,
} from 'devextreme-angular';
import { CompanyComponent } from './components/company/company.component';
import { SecureModule } from './secure.module';
import { SecurePipe } from '@arslan/vms.base';
import { SharedServiceModule } from '../utils/shared-service.module';

@NgModule({
  exports: [
    SecureModule
  ],
  declarations: [
    CustomerNewComponent,
    CustomerListComponent,
    EmployeeNewComponent,
    EmployeeListComponent,
    VendorListComponent,
    VendorNewComponent,
    VehicleListComponent,
    VehicleTypeTreeComponent,
    AddressComponent,
    AddressTreeComponent,
    VehicleComponent,
    UserComponent,
    CompanyComponent,
    DocNumbersComponent,
    ProductNewComponent,
    ProductListComponent,
    LocationComponent,
    CategoryComponent
  ],
  imports: [
    CommonModule,
    BaseRoutingModule,
    DxDataGridModule,
    DxFormModule,
    DxButtonModule,
    DxTabPanelModule,
    DxCheckBoxModule,
    DxTemplateModule,
    DxTabsModule,
    DxSelectBoxModule,
    DxTextBoxModule,
    DxDateBoxModule,
    DxValidatorModule,
    DxValidationSummaryModule,
    DxAccordionModule,
    DxSliderModule,
    DxTagBoxModule,
    DxBoxModule,
    DxTextAreaModule,
    DxLoadIndicatorModule,
    DxLoadPanelModule,
    DxToastModule,
    DxTooltipModule,
    DxTreeListModule,
    DxFileUploaderModule,
    DxValidationGroupModule,
    DxPopupModule,
    CoreModule,
    DxScrollViewModule,
    DxNumberBoxModule,
    NgxsModule.forFeature([BaseState]),
    SecureModule,
    SharedServiceModule
    //VmsCore
  ],
 // providers: [SecurePipe],
})
export class BaseModule {
  static forChild(): ModuleWithProviders<BaseModule> {
    return {
      ngModule: BaseModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<BaseModule> {
    return new LazyModuleFactory(BaseModule.forChild());
  }
}