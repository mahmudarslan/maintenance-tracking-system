import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryRoutingModule } from './inventory-routing.module';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
// import { CoreModule as VmsCore } from '@arslan/vms.base';
import {
  WorkorderComponent,
  ReorderStockComponent,
  CurrentStockComponent,
  AdjustStockNewComponent
} from './components'
import {
  DxDataGridModule,
  DxLookupModule,
  DxTemplateModule,
  DxSelectBoxModule,
  DxTextBoxModule,
  DxBoxModule,
  DxTabPanelModule,
  DxCheckBoxModule,
  DxTabsModule,
  DxDateBoxModule,
  DxValidatorModule,
  DxValidationSummaryModule,
  DxAccordionModule,
  DxSliderModule,
  DxTagBoxModule,
  DxFormModule,
  DxButtonModule,
  DxNumberBoxModule,
  DxLoadPanelModule,
  DxLoadIndicatorModule,
  DxTextAreaModule,
  DxAutocompleteModule,
  DxContextMenuModule,
  DxFileUploaderModule,
  DxDropDownBoxModule,
  DxProgressBarModule,
  DxTreeListModule,
  DxTreeViewModule,
  DxPopupModule
} from 'devextreme-angular';
import { NgxsModule } from '@ngxs/store';
import { InventoryState } from './states/inventory.state';
import { AdjustStockListComponent } from './components/adjust-stock-list/adjust-stock-list.component';

import {BaseModule} from '@arslan/vms.base';

@NgModule({
  declarations: [
    WorkorderComponent,
    ReorderStockComponent,
    AdjustStockNewComponent,
    CurrentStockComponent,
    AdjustStockListComponent,
],
  imports: [
    CommonModule,
    CoreModule,
    InventoryRoutingModule,
    DxDataGridModule,
    DxLookupModule,
    DxTemplateModule,
    DxSelectBoxModule,
    DxTextBoxModule,
    DxBoxModule,
    DxTabPanelModule,
    DxCheckBoxModule,
    DxTabsModule,
    DxDateBoxModule,
    DxValidatorModule,
    DxValidationSummaryModule,
    DxAccordionModule,
    DxSliderModule,
    DxTagBoxModule,
    DxFormModule,
    DxButtonModule,
    DxNumberBoxModule,
    DxLoadPanelModule,
    DxLoadIndicatorModule,
    DxTextAreaModule,
    DxAutocompleteModule,
    DxDropDownBoxModule,
    DxContextMenuModule,
    DxFileUploaderModule,
    DxProgressBarModule,
    DxTreeListModule,
    DxTreeViewModule,
    NgxsModule.forFeature([InventoryState]),
    // VmsCore,
    BaseModule,
    DxPopupModule
  ]
})
export class InventoryModule {
  static forChild(): ModuleWithProviders<InventoryModule> {
    return {
      ngModule: InventoryModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<InventoryModule> {
    return new LazyModuleFactory(InventoryModule.forChild());
  }
}
