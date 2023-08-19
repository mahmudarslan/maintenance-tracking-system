import { NgModule, Component, ViewChild, enableProdMode, ChangeDetectionStrategy, ModuleWithProviders, NgModuleFactory } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentRoutingModule } from './payment-routing.module';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import {
  DxNumberBoxModule, DxCheckBoxModule, DxSelectBoxModule,
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
} from 'devextreme-angular';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CoreModule,
    PaymentRoutingModule,
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
  ]
})

export class PaymentModule {
  static forChild(): ModuleWithProviders<PaymentModule> {
    return {
      ngModule: PaymentModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<PaymentModule> {
    return new LazyModuleFactory(PaymentModule.forChild());
  }
}