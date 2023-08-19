export class CreateUpdateVehicleDto {
  id?: string;
  userVehicleId?: string;
  plate: string;
  color: string;
  motor: string;
  chassis: string;
  modelId: string;
  brandId?: string;
  customerId?: string;
  isDeleted?: boolean;
}


export class CreateUpdateBrandModelDto {
  id?: string;
  parentId: string;
  modelId?: string;
  brandId?: string;
  isDeleted?: boolean;
  models: CreateUpdateBrandModelDto[];
}

export class VehicleTypeDto {
  id?: string;
  parentId?: string;
  name: string;
  isDeleted: boolean;
}