import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CategoryDto } from '../product/models';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  http: HttpClient;
  constructor(private restService: RestService) {
  }

  create(createProductInput: CategoryDto): Observable<any> {
    return this.restService.request<CategoryDto, null>({
      method: 'POST',
      url: 'rest/api/latest/vms/base/product/category',
      body: createProductInput
    })
  }

  update(updateProductInput: CategoryDto, id: string): Observable<any> {
    return this.restService.request<CategoryDto, null>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/product/category/${id}`,
      body: updateProductInput
    })
  }

  delete(id: string): Observable<any> {
    return this.restService.request<any, void>({
      method: 'DELETE',
      url: `rest/api/latest/vms/base/product/category/${id}`
    })
  }

  undo(id: string): Observable<any> {
    return this.restService.request<any, void>({
      method: 'POST',
      url: `rest/api/latest/vms/base/product/category/undo/${id}`
    })
  }


  batchUpdate(input: CategoryDto[]): Observable<any> {
    return this.restService.request<CategoryDto[], null>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/product/category/batchUpdate`,
      body: input
    });
  }

  getAll(): Observable<CategoryDto[]> {
    return this.restService.request<void, CategoryDto[]>({
      method: 'GET',
      url: `rest/api/latest/vms/base/product/category`
    });
  }

  getAllWithDeleteds(): Observable<CategoryDto[]> {
    return this.restService.request<void, CategoryDto[]>({
      method: 'GET',
      url: `rest/api/latest/vms/base/product/category/true`
    });
  }
}