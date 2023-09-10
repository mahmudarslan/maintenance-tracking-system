import { NgModule } from '@angular/core';
import { SecurePipe } from './pipes/secure.pipe';

@NgModule({
  exports: [SecurePipe],
  declarations: [SecurePipe],
})
export class SecureModule {}
