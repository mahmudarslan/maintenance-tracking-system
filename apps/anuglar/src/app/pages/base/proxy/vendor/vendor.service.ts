import { Injectable } from '@angular/core';
import { RestService, ABP } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CreateUpdateVendor, VendorSearch } from './models';

@Injectable({
    providedIn: 'root'
})
export class VendorService {
    http: HttpClient;
    apiName = 'Base';

    constructor(private restService: RestService) { }

    create = (input: CreateUpdateVendor) =>
        this.restService.request<any, CreateUpdateVendor>({
            method: 'POST',
            url: 'rest/api/latest/vms/base/vendor',
            body: input,
        },
            { apiName: this.apiName })

    update = (id: string, input: CreateUpdateVendor) =>
        this.restService.request<any, CreateUpdateVendor>({
            method: 'PUT',
            url: `rest/api/latest/vms/base/vendor/${id}`,
            body: input,
        },
            { apiName: this.apiName })

    delete(id: string): Observable<any> {
        return this.restService.request<void, null>({
            method: 'DELETE',
            url: `rest/api/latest/vms/base/vendor/${id}`,
        });
    }

    undo(id: string): Observable<any> {
        return this.restService.request<CreateUpdateVendor, null>({
            method: 'POST',
            url: `rest/api/latest/vms/base/vendor/undo?Id=${id}`,
        });
    }

    get = (id: string, isDeleted: boolean) =>
        this.restService.request<any, CreateUpdateVendor>({
            method: 'GET',
            url: `rest/api/latest/vms/base/vendor/${id}/${isDeleted}`,
        },
            { apiName: this.apiName })

    bySearchContent = (params) =>
        this.restService.request<any, any>({
            method: 'GET',
            url: `rest/api/latest/vms/base/vendor/BySearchContent?nameSurnameGsm=${params}`,
        },
            { apiName: this.apiName })


    searchVendorById = (id: string) =>
        this.restService.request<any, VendorSearch>({
            method: 'GET',
            url: `rest/api/latest/vms/base/vendor/ByVendorId?id=${id}`,
        },
            { apiName: this.apiName })

}
