import { AuditedEntityDto } from '@abp/ng.core';

export class Address {
  id?: string;
  country: string;
  city: string;
  district: string;
  countryId: string;
  cityId: string;
  districtId: string;
  address1: string;
}

export interface CountryDto extends AuditedEntityDto<string> {
  Name: string,
}

export interface CityDto {
  id: string,
  name: string,
  countryId: string,
}

export interface DistrictDto {
  id: string,
  name: string,
  cityId: string,
}

export class IdName {
  constructor(
    public id: string,
    public name: string,
  ) { }
}