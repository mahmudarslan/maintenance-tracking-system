import { PagedResultDto } from '@abp/ng.core';
import { CityDto, CountryDto, DistrictDto } from '../proxy/address';
import { VehicleBrand, VehicleBrandModel } from '../proxy/customer';
import { EmployeeRole } from '../proxy/employee';
import { RoleUserDto } from '../proxy/user';


export namespace Base {
  export interface State {
    country: CountryDto[];
    city: CityDto[];
    district: DistrictDto[];
    technitionUser: RoleUserDto[];
    headTechnitionUser: RoleUserDto[];
    role: EmployeeRole[];
    brands: VehicleBrand[];
    brandModels: VehicleBrandModel[];
    allBrandModels: VehicleBrandModel[];
  }
}