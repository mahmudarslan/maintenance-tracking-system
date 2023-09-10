import { Injectable } from '@angular/core';
import { RestService, ABP } from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { CreateUpdateEmployee, EmployeeRole } from './models';
import { RoleUserDto } from '../user/models';
import { Observable } from 'rxjs';
import { CustomRestService } from 'src/app/pages/utils';

@Injectable({
    providedIn: 'root'
})
export class EmployeeService {
    http: HttpClient;
    apiName = 'base';
    baseUrl = '/rest/api/latest/vms/base/employee/';

    constructor(private restService: RestService,
        private customService: CustomRestService) { }

    create = (input: CreateUpdateEmployee) =>
        this.restService.request<any, CreateUpdateEmployee>({
            method: 'POST',
            url: this.baseUrl,
            body: input,
        },
            { apiName: this.apiName })

    update = (id: string, input: CreateUpdateEmployee) =>
        this.restService.request<any, CreateUpdateEmployee>({
            method: 'PUT',
            url: this.baseUrl + `${id}`,
            body: input,
        },
            { apiName: this.apiName })

    delete(id: string): Observable<any> {
        return this.restService.request<any, any>({
            method: 'DELETE',
            url: this.baseUrl + `${id}`,
        });
    }

    undo(id: string): Observable<any> {
        return this.restService.request<CreateUpdateEmployee, null>({
            method: 'POST',
            url: this.baseUrl + `undo?Id=${id}`,
        });
    }

    get = (id: string, isDeleted: boolean) =>
        this.restService.request<any, CreateUpdateEmployee>({
            method: 'GET',
            url: this.baseUrl + `${id}/${isDeleted}`,
        },
            { apiName: this.apiName })


    getList = (loadOptions: any) =>
        this.customService.requestList<any, any[]>({
            method: 'GET',
            url: this.baseUrl,
        },
            loadOptions,
            { apiName: this.apiName })

    // get = (params: ABP.PageQueryParams) =>
    //     this.restService.request<any, Response>({
    //         method: 'GET',
    //         url: 'rest/api/latest/vms/base/employee',
    //         params: params
    //     },
    //         { apiName: this.apiName });

    getUser = (id: string) =>
        this.restService.request<any, RoleUserDto>({
            method: 'GET',
            url: this.baseUrl + `employee?Id=${id}`,
        },
            { apiName: this.apiName })

    getRoles = () =>
        this.restService.request<any, EmployeeRole[]>({
            method: 'GET',
            url: this.baseUrl + 'employeeRoles',
        },
            { apiName: this.apiName })

    getTechnicianUsers = () =>
        this.restService.request<any, RoleUserDto[]>({
            method: 'GET',
            url: this.baseUrl + 'allTechnicianUsers',
        },
            { apiName: this.apiName })


    getHeadTechnicianUsers = () =>
        this.restService.request<any, RoleUserDto[]>({
            method: 'GET',
            url: this.baseUrl + 'allHeadTechnicianUsers',
        },
            { apiName: this.apiName })
}
