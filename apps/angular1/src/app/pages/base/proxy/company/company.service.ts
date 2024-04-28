import { Injectable } from '@angular/core';
import { RestService, ABP } from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { CompanyDto, CreateUpdateCompany } from '.';

@Injectable({
    providedIn: 'root'
})
export class CompanyService {
    http: HttpClient;
    apiName = 'base';

    update = (id: string, input: CreateUpdateCompany) =>
    this.restService.request<any, CreateUpdateCompany>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/company/`,
      body: input,
    },
    { apiName: this.apiName });


    get = () =>
        this.restService.request<any, CompanyDto>({
            method: 'GET',
            url: `rest/api/latest/vms/base/company/`,
        },
            { apiName: this.apiName });

    constructor(private restService: RestService) { }
}