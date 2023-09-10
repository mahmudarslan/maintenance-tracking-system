import { Injectable } from '@angular/core';
import { RestService, ABP } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CategoryDto, CreateUpdateProductDto, ProductDto, ProductType } from './models';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  http: HttpClient;
  constructor(private restService: RestService) {
  }

  create(createProductInput: CreateUpdateProductDto): Observable<any> {
    return this.restService.request<CreateUpdateProductDto, null>({
      method: 'POST',
      url: 'rest/api/latest/vms/base/product',
      body: createProductInput
    });
  }

  update(updateProductInput: CreateUpdateProductDto, id: string): Observable<any> {
    return this.restService.request<CreateUpdateProductDto, null>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/product/${id}`,
      body: updateProductInput
    });
  }

  delete(id: string): Observable<any> {
    return this.restService.request<void, null>({
      method: 'DELETE',
      url: `rest/api/latest/vms/base/product/Id=${id}`,
    });
  }

  undo(id: string): Observable<any> {
    return this.restService.request<CreateUpdateProductDto, null>({
      method: 'POST',
      url: `rest/api/latest/vms/base/product/undo?Id=${id}`,
    });
  }

  get(id: string, isDeleted: boolean): Observable<ProductDto> {
    return this.restService.request<void, ProductDto>({
      method: 'GET',
      url: `rest/api/latest/vms/base/product/${id}/${isDeleted}`
    });
  }

  getItems(): Observable<ProductDto[]> {
    return this.restService.request<void, ProductDto[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/product/'
    });
  }

  getStockTypeItems(): Observable<ProductDto[]> {
    return this.restService.request<void, ProductDto[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/product/AllStockedProduct'
    });
  }

  getStockUnstockTypeItems(): Observable<ProductDto[]> {
    return this.restService.request<void, ProductDto[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/product/AllProduct'
    });
  }

  getServiceTypeItems(): Observable<ProductDto[]> {
    return this.restService.request<void, ProductDto[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/product/AllService'
    });
  }

  getProductTypes(): Observable<ProductType.ProductType[]> {
    return this.restService.request<void, ProductType.ProductType[]>({
      method: 'GET',
      url: 'rest/api/latest/vms/base/product/productTypes'
    });
  }
}
