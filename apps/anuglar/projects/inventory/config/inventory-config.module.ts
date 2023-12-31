import { ModuleWithProviders, NgModule } from '@angular/core';
import { INVENTORY_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class InventoryConfigModule {
  static forRoot(): ModuleWithProviders<InventoryConfigModule> {
    return {
      ngModule: InventoryConfigModule,
      providers: [INVENTORY_ROUTE_PROVIDERS],
    };
  }
}
