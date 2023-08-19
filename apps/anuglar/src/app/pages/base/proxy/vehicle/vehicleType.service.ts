import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { VehicleBrandModel, VehicleBrand } from '../customer';
import { CreateUpdateBrandModelDto, VehicleTypeDto } from '.';
import { CustomRestService } from 'src/app/pages/utils';
@Injectable({
  providedIn: 'root'
})
export class VehicleTypeService {
  apiName = 'Base';

  constructor(private restService: RestService,
    private http: HttpClient,
    private customService: CustomRestService) { }

  create = (input: VehicleTypeDto) =>
    this.restService.request<any, VehicleTypeDto>({
      method: 'POST',
      url: 'rest/api/latest/vms/base/vehicletype',
      body: input,
    },
      { apiName: this.apiName });

  update = (id: string, input: VehicleTypeDto) =>
    this.restService.request<any, VehicleTypeDto>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/vehicletype/${id}`,
      body: input,
    },
      { apiName: this.apiName });

  delete(id: string): Observable<any> {
    return this.restService.request<void, null>({
      method: 'DELETE',
      url: `rest/api/latest/vms/base/vehicletype/${id}`,
    })
  }

  undo(id: string): Observable<any> {
    return this.restService.request<VehicleTypeDto, null>({
      method: 'POST',
      url: `rest/api/latest/vms/base/vehicletype/undo/${id}`,
    })
  }

  get = (id: string, isDeleted: boolean) =>
    this.restService.request<any, VehicleTypeDto>({
      method: 'GET',
      url: `rest/api/latest/vms/base/vehicletype/${id}/${isDeleted}`,
    },
      { apiName: this.apiName });

  getList1 = (loadOptions: any) =>
    this.customService.requestList<any, any>(
      {
        method: 'GET',
        url: `rest/api/latest/vms/base/vehicletype/`
      },
      loadOptions,
      { apiName: this.apiName },
    );

  getList = (loadOptions: any) =>
    this.customService.requestList<any, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/vehicletype/',
    },
      loadOptions,
      { apiName: this.apiName });

  getBrands = () =>
    this.restService.request<any, VehicleBrand[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/vehicletype/brands',
    },
      { apiName: this.apiName });

  getBrandModels = (brandId: string) =>
    this.restService.request<any, VehicleBrandModel[]>({
      method: 'GET',
      url: `rest/api/latest/vms/base/vehicletype/modelsByBrand/${brandId}`,
    },
      { apiName: this.apiName });

  getAllBrandModel = () =>
    this.restService.request<any, VehicleBrandModel[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/vehicletype/allmodels',
    },
      { apiName: this.apiName });

  getAllWithDeleteds(): Observable<CreateUpdateBrandModelDto[]> {
    return this.restService.request<void, CreateUpdateBrandModelDto[]>({
      method: 'GET',
      url: `rest/api/latest/vms/base/vehicletype/withDeleteds`
    });
  }


}