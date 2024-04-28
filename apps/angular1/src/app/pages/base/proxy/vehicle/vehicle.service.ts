import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { VehicleBrandModel, VehicleBrand, CustomerVehicleDto } from '../customer';
import { CreateUpdateBrandModelDto, CreateUpdateVehicleDto } from '.';
import { LoadOptions } from 'devextreme/data/load_options';
import { CustomRestService } from 'src/app/pages/utils';
@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  apiName = 'base';

  constructor(private restService: RestService,
    private http: HttpClient,
    private customService: CustomRestService) { }


  create = (input: CreateUpdateVehicleDto) =>
    this.restService.request<any, CreateUpdateVehicleDto>({
      method: 'POST',
      url: 'rest/api/latest/vms/base/vehicle',
      body: input,
    },
      { apiName: this.apiName })

  update = (id: string, input: CreateUpdateVehicleDto) =>
    this.restService.request<any, CreateUpdateVehicleDto>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/vehicle/${id}`,
      body: input,
    },
      { apiName: this.apiName })

  delete(id: string): Observable<any> {
    return this.restService.request<void, null>({
      method: 'DELETE',
      url: `rest/api/latest/vms/base/vehicle/${id}`,
    });
  }

  undo(id: string): Observable<any> {
    return this.restService.request<CreateUpdateVehicleDto, null>({
      method: 'POST',
      url: `rest/api/latest/vms/base/vehicle/undo/${id}`,
    });
  }

  get = (id: string, isDeleted: boolean) =>
    this.restService.request<any, CreateUpdateVehicleDto>({
      method: 'GET',
      url: `rest/api/latest/vms/base/vehicle/${id}/${isDeleted}`,
    },
      { apiName: this.apiName })

  getList = (loadOptions: any) =>
    this.customService.requestList<any, any>(
      {
        method: 'GET',
        url: `rest/api/latest/vms/base/vehicle/`
      },
      loadOptions,
      { apiName: this.apiName },
    )

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
}
