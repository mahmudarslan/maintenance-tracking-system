import { Injectable } from '@angular/core';
import { ABP, RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { DocNumberDto, DocTypeDto } from '.';

@Injectable({
  providedIn: 'root'
})
export class DocNumberService {
  apiName = 'Core';

  constructor(private restService: RestService) { }

  get = (params: ABP.PageQueryParams) =>
    this.restService.request<any, Response>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/documentNoFormats',
      params
    },
      { apiName: this.apiName })

  getAll = () =>
    this.restService.request<any, DocNumberDto[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/documentNoFormats/getAll'
    },
      { apiName: this.apiName })

  getDocNoTypes(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/documentNoFormats/docTypes'
    });
  }

  update = (input: DocNumberDto[]) =>
    this.restService.request<any, DocNumberDto[]>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/documentNoFormats`,
      body: input,
    },
      { apiName: this.apiName })
}
