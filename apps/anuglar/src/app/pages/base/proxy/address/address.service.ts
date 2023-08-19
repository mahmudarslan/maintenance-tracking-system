import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { CityDto, CountryDto, DistrictDto } from '.';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  apiName = 'base';
  baseUrl = '/rest/api/latest/vms/base/address/';

  getCountries = () =>
    this.restService.request<any, CountryDto[]>({
      method: 'GET',
      url: this.baseUrl + 'countries',
    },
      { apiName: this.apiName })

  getCities = (countryId: string) =>
    this.restService.request<any, CityDto[]>({
      method: 'GET',
      url: this.baseUrl + `citiesByCountry/${countryId}`,
    },
      { apiName: this.apiName })

  getDistricts = (cityId: string) =>
    this.restService.request<any, DistrictDto[]>({
      method: 'GET',
      url: this.baseUrl + `districtsByCity/${cityId}`,
    },
      { apiName: this.apiName })

  constructor(private restService: RestService) { }
}
