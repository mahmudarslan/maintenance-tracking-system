import { Address } from '../address';

export class EmployeeRole {
    name: string;
    value: string;
}

export class CreateUpdateEmployee {
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