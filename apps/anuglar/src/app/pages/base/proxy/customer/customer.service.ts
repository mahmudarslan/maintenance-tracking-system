import { Injectable } from '@angular/core';
import { RestService, ABP } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CreateUpdateCustomerWithVehicleDto, CustomerDto, CustomerSearch, CustomerVehicleDto } from './models';

@Injectable({
    providedIn: 'root'
})
export class CustomersService {
    http: HttpClient;
    apiName = 'base';

    constructor(private restService: RestService) { }

    create = (input: CreateUpdateCustomerWithVehicleDto) =>
        this.restService.request<any, CreateUpdateCustomerWithVehicleDto>({
            method: 'POST',
            url: 'rest/api/latest/vms/base/customer',
            body: input,
        },
            { apiName: this.apiName })

    update = (id: string, input: CreateUpdateCustomerWithVehicleDto) =>
        this.restService.request<any, CreateUpdateCustomerWithVehicleDto>({
            method: 'PUT',
            url: `rest/api/latest/vms/base/customer/${id}`,
            body: input,
        },
            { apiName: this.apiName })

    delete(id: string): Observable<any> {
        return this.restService.request<void, null>({
            method: 'DELETE',
            url: `rest/api/latest/vms/base/customer/${id}`,
        });
    }

    undo(id: string): Observable<any> {
        return this.restService.request<CreateUpdateCustomerWithVehicleDto, null>({
            method: 'POST',
            url: `rest/api/latest/vms/base/customer/undo?Id=${id}`,
        });
    }

    undoVehicle(id: string): Observable<any> {
        return this.restService.request<CreateUpdateCustomerWithVehicleDto, null>({
            method: 'POST',
            url: `rest/api/latest/vms/base/customer/undoVehicle?Id=${id}`,
        });
    }

    get = (id: string, isDeleted: boolean) =>
        this.restService.request<any, CreateUpdateCustomerWithVehicleDto>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/${id}/${isDeleted}`,
        },
            { apiName: this.apiName })

    getList = () =>
        this.restService.request<any, CustomerDto[]>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/`,
        },
            { apiName: this.apiName })

    getAll = () =>
        this.restService.request<any, CustomerDto[]>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/all`,
        },
            { apiName: this.apiName })

    bySearchContent = (params) =>
        this.restService.request<any, any>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/BySearchContent?nameSurnameGsmPlateNo=${params}`,
        },
            { apiName: this.apiName })

    getCustomerVehicles = (params) =>
        this.restService.request<any, CustomerVehicleDto>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/CustomerVehicles?customerId=${params}`,
        },
            { apiName: this.apiName })

    getCustomerVehicle = (id) =>
        this.restService.request<any, CustomerVehicleDto>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/CustomerVehicle?userVehicleId=${id}`,
        },
            { apiName: this.apiName })

    searchCustomerById = (userVehicleId: string) =>
        this.restService.request<any, CustomerSearch>({
            method: 'GET',
            url: `rest/api/latest/vms/base/customer/byVehicleId/${userVehicleId}`,
        },
            { apiName: this.apiName })
}
