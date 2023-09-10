/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from './@core/core.module';
import { ThemeModule } from './@theme/theme.module';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {
  NbChatModule,
  NbDatepickerModule,
  NbDialogModule,
  NbMenuModule,
  NbSidebarModule,
  NbToastrModule,
  NbWindowModule,
} from '@nebular/theme';


import { CoreModule as abpCoreModule } from '@abp/ng.core';
import { registerLocale } from '@abp/ng.core/locale';
import { environment } from '../environments/environment';
import { OrderConfigModule } from '@arslan/vms.order/config/order-config.module';
import { BaseConfigModule } from '@arslan/vms.base/config/base-config.module';
import { InventoryConfigModule } from '@arslan/vms.inventory/config/inventory-config.module';
import { PaymentConfigModule } from '@arslan/vms.payment/config/payment-config.module';
import { PlannerConfigModule } from '@arslan/vms.planner/config/planner-config.module';
import { NgxsModule } from '@ngxs/store';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    NbSidebarModule.forRoot(),
    NbMenuModule.forRoot(),
    NbDatepickerModule.forRoot(),
    NbDialogModule.forRoot(),
    NbWindowModule.forRoot(),
    NbToastrModule.forRoot(),
    NbChatModule.forRoot({
      messageGoogleMapKey: 'AIzaSyA_wNuCzia92MAmdLRzmqitRGvCF7wCZPY',
    }),
    CoreModule.forRoot(),
    ThemeModule.forRoot(),

    NgxsModule.forRoot(),
    abpCoreModule.forRoot({
      environment,
      registerLocaleFn: registerLocale(),
    }),
    OrderConfigModule.forRoot(),
    BaseConfigModule.forRoot(),
    InventoryConfigModule.forRoot(),
    PaymentConfigModule.forRoot(),
    PlannerConfigModule.forRoot(),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
