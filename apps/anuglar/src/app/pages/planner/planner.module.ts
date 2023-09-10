import { NgModule, Component, ViewChild, enableProdMode, ChangeDetectionStrategy, ModuleWithProviders, NgModuleFactory } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlannerRoutingModule } from './planner-routing.module';
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
  DxSchedulerModule
} from 'devextreme-angular';
import { SchedulerComponent } from './components/scheduler/scheduler.component';

@NgModule({
  declarations: [SchedulerComponent],
  imports: [
    CommonModule,
    CoreModule,
    PlannerRoutingModule,
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
    DxSchedulerModule
  ]
})

export class PlannerModule {
  static forChild(): ModuleWithProviders<PlannerModule> {
    return {
      ngModule: PlannerModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<PlannerModule> {
    return new LazyModuleFactory(PlannerModule.forChild());
  }
}