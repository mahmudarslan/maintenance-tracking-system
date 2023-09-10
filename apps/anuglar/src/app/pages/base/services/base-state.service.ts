import { ABP } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { BaseState } from '../states/base.state';
import {
    GetAllBrandModel,
    GetBrands,
    GetCitiesByCountryId,
    GetCountries,
    GetDistrictsByCityId,
    GetHeadTechnicianUsers,
    GetRoles,
    GetTechnicianUsers
} from '../actions/base.actions';


@Injectable({
    providedIn: 'root',
})
export class BaseStateService {
    constructor(private store: Store) { }

    getCountries() {
        return this.store.selectSnapshot(BaseState.getCountries);
    }

    getCities() {
        return this.store.selectSnapshot(BaseState.getCities);
    }

    getDistricts() {
        return this.store.selectSnapshot(BaseState.getDistricts);
    }

    getTechnicianUsers() {
        return this.store.selectSnapshot(BaseState.getTechnicianUsers);
    }

    getHeadTechnicianUsers() {
        return this.store.selectSnapshot(BaseState.getHeadTechnicianUsers);
    }

    getRoles() {
        return this.store.selectSnapshot(BaseState.getRoles);
    }

    getBrands() {
        return this.store.selectSnapshot(BaseState.getBrands);
    }

    getAllBrandModel() {
        return this.store.selectSnapshot(BaseState.getAllBrandModel);
    }


    dispatchGetBrands() {
        return this.store.dispatch(new GetBrands());
    }
    dispatchGetAllBrandModel() {
        return this.store.dispatch(new GetAllBrandModel());
    }

    dispatchGetCountries() {
        return this.store.dispatch(new GetCountries());
    }

    dispatchGetCityByCountryId(...args: ConstructorParameters<typeof GetCitiesByCountryId>) {
        return this.store.dispatch(new GetCitiesByCountryId(...args));
    }

    dispatchGetDistrictByCityId(...args: ConstructorParameters<typeof GetDistrictsByCityId>) {
        return this.store.dispatch(new GetDistrictsByCityId(...args));
    }

    dispatchgetTechnicianUsers() {
        return this.store.dispatch(new GetTechnicianUsers());
    }

    dispatchGetHeadTechnicianUsers() {
        return this.store.dispatch(new GetHeadTechnicianUsers());
    }

    dispatchGetRoles() {
        return this.store.dispatch(new GetRoles());
    }
}