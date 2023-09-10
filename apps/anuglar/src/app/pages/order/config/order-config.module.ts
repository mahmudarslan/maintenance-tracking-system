import { ModuleWithProviders, NgModule } from '@angular/core';
import { ORDER_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class OrderConfigModule {
  static forRoot(): ModuleWithProviders<OrderConfigModule> {
    return {
      ngModule: OrderConfigModule,
      providers: [ORDER_ROUTE_PROVIDERS],
    };
  }
}