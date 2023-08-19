import { NgModule, ModuleWithProviders, NgModuleFactory } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderRoutingModule } from './order-routing.module';
import { SalesOrderNewComponent, SalesOrderListComponent, PurchaseOrderNewComponent, PurchaseOrderListComponent } from './components/';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import {
  DxNumberBoxModule,
  DxCheckBoxModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxDateBoxModule,
  DxFormModule,
  DxDataGridModule,
  DxBoxModule,
  DxTagBoxModule,
  DxButtonModule,
  DxValidatorModule,
  DxValidationSummaryModule,
  DxTextBoxModule,
  DxAutocompleteModule,
  DxAccordionModule,
  DxSliderModule,
  DxTemplateModule,
  DxContextMenuModule,
  DxFileUploaderModule,
  DxDropDownBoxModule,
  DxLoadIndicatorModule,
  DxLoadPanelModule,
  DxPopupModule
} from 'devextreme-angular';
import { NgxsModule } from '@ngxs/store';
import { BaseState } from '../base/states/base.state';
import {BaseModule} from '@arslan/vms.base';
// import { CoreModule as VmsCore } from '@arslan/vms.base';

@NgModule({
  declarations: [SalesOrderNewComponent, SalesOrderListComponent, PurchaseOrderNewComponent, PurchaseOrderListComponent],
  imports: [
    CommonModule,
    CoreModule,
    OrderRoutingModule,
    DxNumberBoxModule,
    DxCheckBoxModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxDataGridModule,
    DxBoxModule,
    DxTagBoxModule,
    DxButtonModule,
    DxValidatorModule,
    DxValidationSummaryModule,
    DxTextBoxModule,
    DxAccordionModule,
    DxSliderModule,
    DxTemplateModule,
    DxAutocompleteModule,
    DxContextMenuModule,
    DxFileUploaderModule,
    DxDropDownBoxModule,
    DxLoadIndicatorModule,
    DxLoadPanelModule,
    NgxsModule.forFeature([BaseState]),
    BaseModule,
    DxPopupModule
    // VmsCore
  ]
})
export class OrderModule {
  static forChild(): ModuleWithProviders<OrderModule> {
    return {
      ngModule: OrderModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<OrderModule> {
    return new LazyModuleFactory(OrderModule.forChild());
  }
}