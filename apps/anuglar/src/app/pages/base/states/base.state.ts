import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { pluck, tap } from 'rxjs/operators';
import { GetAllBrandModel, GetBrands, GetCitiesByCountryId, GetCountries, GetDistrictsByCityId, GetHeadTechnicianUsers, GetModelsByBrandId, GetRoles, GetTechnicianUsers } from '../actions/base.actions';
import { Base } from '../models/base';
import { AddressService, CityDto, CountryDto, DistrictDto } from '../proxy/address';
import { VehicleBrand, VehicleBrandModel } from '../proxy/customer';
import { EmployeeRole, EmployeeService } from '../proxy/employee';
import { RoleUserDto } from '../proxy/user';
import { VehicleTypeService } from '../proxy/vehicle';

@State<Base.State>({
    name: 'BaseState',
    defaults: {
        country: {},
        city: {},
        district: {},
        technitionUser: {},
        headTechnitionUser: {},
        role: {},
        brands: {},
        brandModels: {},
    } as Base.State,
})
@Injectable()
export class BaseState {
    @Selector()
    static getCountries({ country }: Base.State): CountryDto[] {
        return country || [];
    }

    @Selector()
    static getCities({ city }: Base.State): CityDto[] {
        return city || [];
    }

    @Selector()
    static getDistricts({ district }: Base.State): DistrictDto[] {
        return district || [];
    }

    @Selector()
    static getTechnicianUsers({ technitionUser }: Base.State): RoleUserDto[] {
        return technitionUser || [];
    }

    @Selector()
    static getHeadTechnicianUsers({ headTechnitionUser }: Base.State): RoleUserDto[] {
        return headTechnitionUser || [];
    }

    @Selector()
    static getRoles({ role }: Base.State): EmployeeRole[] {
        return role || [];
    }

    @Selector()
    static getBrands({ brands }: Base.State): VehicleBrand[] {
        return brands || [];
    }
    
    @Selector()
    static getAllBrandModel({ brandModels }: Base.State): VehicleBrandModel[] {
        return brandModels || [];
    }

    @Selector()
    static getModelsByBrandId({ brandModels }: Base.State): VehicleBrandModel[] {
        return brandModels || [];
    }


    @Action(GetCountries)
    getCountries({ patchState }: StateContext<Base.State>) {
        return this.addressService.getCountries().pipe(
            tap(country =>
                patchState({
                    country,
                }),
            ),
        );
    }

    @Action(GetCitiesByCountryId)
    getCity({ patchState }: StateContext<Base.State>, { payload }: GetCitiesByCountryId) {
        return this.addressService.getCities(payload).pipe(
            tap(city =>
                patchState({
                    city,
                }),
            ),
        );
    }

    @Action(GetDistrictsByCityId)
    getDistrict({ patchState }: StateContext<Base.State>, { payload }: GetDistrictsByCityId) {
        return this.addressService.getDistricts(payload).pipe(
            tap(district =>
                patchState({
                    district,
                }),
            ),
        );
    }

    @Action(GetTechnicianUsers)
    getTechnicianUsers({ patchState }: StateContext<Base.State>) {
        return this.employeeService.getTechnicianUsers().pipe(
            tap(technitionUser =>
                patchState({
                    technitionUser,
                }),
            ),
        );
    }

    @Action(GetHeadTechnicianUsers)
    getHeadTechnicianUsers({ patchState }: StateContext<Base.State>) {
        return this.employeeService.getHeadTechnicianUsers().pipe(
            tap(headTechnitionUser =>
                patchState({
                    headTechnitionUser,
                }),
            ),
        );
    }

    @Action(GetRoles)
    getRoles({ patchState }: StateContext<Base.State>) {
        return this.employeeService.getRoles().pipe(
            tap(role =>
                patchState({
                    role,
                }),
            ),
        );
    }

    @Action(GetBrands)
    getBrands({ patchState }: StateContext<Base.State>) {
        return this.vehicleService.getBrands().pipe(
            tap(brands =>
                patchState({
                    brands,
                }),
            ),
        );
    }

    @Action(GetAllBrandModel)
    getAllBrandModel({ patchState }: StateContext<Base.State>) {
        return this.vehicleService.getAllBrandModel().pipe(
            tap(brandModels =>
                patchState({
                    brandModels,
                }),
            ),
        );
    }

    @Action(GetModelsByBrandId)
    getModelsByBrandId({ patchState }: StateContext<Base.State>, { payload }: GetModelsByBrandId) {
        return this.vehicleService.getBrandModels(payload).pipe(
            tap(brandModels =>
                patchState({
                    brandModels,
                }),
            ),
        );
    }
    
    constructor(
        private addressService: AddressService,
        private employeeService: EmployeeService,
        private vehicleService: VehicleTypeService,
    ) { }
}