import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { LocationDto } from '.';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  http: HttpClient;
  constructor(private restService: RestService) {
  }

  create(createProductInput: LocationDto): Observable<any> {
    return this.restService.request<LocationDto, null>({
      method: 'POST',
      url: 'rest/api/latest/vms/base/location',
      body: createProductInput
    });
  }

  update(updateProductInput: LocationDto, id: string): Observable<any> {
    return this.restService.request<LocationDto, null>({
      method: 'PUT',
      url: `rest/api/latest/vms/base/location/${id}`,
      body: updateProductInput
    });
  }

  delete(id: string): Observable<any> {
    return this.restService.request<any, void>({
      method: 'DELETE',
      url: `rest/api/latest/vms/base/location/${id}`
    });
  }

  undo(id: string): Observable<any> {
    return this.restService.request<any, void>({
      method: 'POST',
      url: `rest/api/latest/vms/base/location/undo/${id}`
    });
  }

  getAll(): Observable<LocationDto[]> {
    return this.restService.request<void, LocationDto[]>({
      method: 'GET',
      url: `rest/api/latest/vms/base/location`
    });
  }

  getAllWithDeleteds(): Observable<LocationDto[]> {
    return this.restService.request<void, LocationDto[]>({
      method: 'GET',
      url: `rest/api/latest/vms/base/location/true`
    });
  }

  getLocations(): Observable<LocationDto> {
    return this.restService.request<void, LocationDto>({
      method: 'GET',
      url: `rest/api/latest/vms/base/location/false`
    });
  }

  getLocation(id: string): Observable<LocationDto> {
    return this.restService.request<void, LocationDto>({
      method: 'GET',
      url: `rest/api/latest/vms/base/location/${id}`
    });
  }
}
