import { ModuleWithProviders, NgModule } from '@angular/core';
import { PAYMENT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class PaymentConfigModule {
  static forRoot(): ModuleWithProviders<PaymentConfigModule> {
    return {
      ngModule: PaymentConfigModule,
      providers: [PAYMENT_ROUTE_PROVIDERS],
    };
  }
}
