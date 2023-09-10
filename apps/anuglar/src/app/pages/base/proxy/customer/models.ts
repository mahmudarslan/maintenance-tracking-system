import { Address } from '../address/models';
import { CreateUpdateVehicleDto } from '../vehicle/models';

export class CreateUpdateCustomerWithVehicleDto {
  id?: string;
  emailConfirmed: true;
  phoneNumberConfirmed: true;
  addresses: Address[];
  password: string;
  userName: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  twoFactorEnabled: true;
  lockoutEnabled: true;
  roleNames: [];
  vehicles: CreateUpdateVehicleDto[];
  homePhoneNumber: string;
  birthDate: Date;
  faxNumber: string;
  isDeleted: boolean;
}

export class Response {
  items: Customer[];
  totalCount: number;
}

export class VehicleBrand {
  id: string;
  name: string;
}

export class VehicleBrandModel {  
  brandId: string;
  id: string;
  name: string;
}

export class Customer {
  tenantId: string;
  userName: string;
  identityNumber: string;
  name: string;
  surname: string;
  birthDate: string;
  mobilePhonemNumber: string;
  homePhoneNumber: string;
  faxNumber: string;
  address: string;
  email: string;
  cNwithPhone: string;
  id: string;
  nameSurname: string;
}

export class CustomerSearch {
  id: string;
  userVehicleId: string;
  name: string;
  surname: string;
  phoneNumber: string;
  plateNo: string;
  selectionField: string;
}

export class CustomerVehicleDto {  
  id: string;
  userVehicleId: string;
  name: string;
}

export class CustomerDto {
  id: string;
  name: string;
  surname: string;
  nameSurname: string;
}
