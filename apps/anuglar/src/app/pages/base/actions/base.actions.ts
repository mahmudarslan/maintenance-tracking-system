

export class GetCountries {
    static readonly type = '[Base] Get GetCountries ';
}

export class GetCitiesByCountryId {
    static readonly type = '[Base] Get GetCities ';
    constructor(public payload: string) { }
}

export class GetDistrictsByCityId {
    static readonly type = '[Base] Get GetDistricts ';
    constructor(public payload: string) { }
}

export class GetTechnicianUsers {
    static readonly type = '[Base] Get GetTechnicianUsers ';
}

export class GetHeadTechnicianUsers {
    static readonly type = '[Base] Get GetHeadTechnicianUsers ';
}

export class GetRoles {
    static readonly type = '[Base] Get GetRoles ';
}

export class GetBrands {
    static readonly type = '[Base] Get GetBrands ';
}

export class GetAllBrandModel {
    static readonly type = '[Base] Get GetAllBrandModel ';
}

export class GetModelsByBrandId {
    static readonly type = '[Base] Get GetBrandModels ';
    constructor(public payload: string) { }
}

