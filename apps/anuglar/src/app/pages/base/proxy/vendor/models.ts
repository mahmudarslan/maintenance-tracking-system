import { Address } from '../address';

export class VendorRole {
    name: string;
    value: string;
}

export class CreateUpdateVendor {
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
    homePhoneNumber: string;
    birthDate: Date;
    faxNumber: string;
    roles: string[];
    role: string;
    isCustomerRole: boolean;
    isDeleted: boolean;
}

export class VendorSearch {
    id: string; 
    name: string;
    surname: string;
    phoneNumber: string; 
    selectionField: string;
  }