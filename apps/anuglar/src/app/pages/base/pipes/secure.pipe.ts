import { HttpClient } from '@angular/common/http';
import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { RestService, ABP } from '@abp/ng.core';
import { Observable } from 'rxjs'; 

@Injectable()
@Pipe({
  name: 'secure',
})
export class SecurePipe implements PipeTransform {

  constructor(private sanitizer: DomSanitizer, private restService: RestService) { }

  transform(url): Observable<any> {

    if (!url) { return; }

    return this.restService.request(
      {
        method: 'GET',
        url: url,
        responseType: 'blob'
      },
    )//.map(val => this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(val)));

  }
}
