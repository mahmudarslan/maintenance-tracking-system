import { ModuleWithProviders, NgModule } from '@angular/core';
import { PLANNER_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class PlannerConfigModule {
  static forRoot(): ModuleWithProviders<PlannerConfigModule> {
    return {
      ngModule: PlannerConfigModule,
      providers: [PLANNER_ROUTE_PROVIDERS],
    };
  }
}
