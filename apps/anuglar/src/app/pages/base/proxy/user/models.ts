export class RoleUserDto {
    id: string;
    // name: string;
    // surname: string;
    nameSurname: string;
}


export class UserDto {
    id?: string;
    emailConfirmed: true;
    phoneNumberConfirmed: true;
    password: string;
    userName: string;
    name: string;
    surname: string;
    email: string;
    phoneNumber: string;
    homePhoneNumber: string;
    birthDate: Date;
    faxNumber: string;
    selectedRole: string;
    roles: string[] = [];
    isCustomerRole: boolean = false;
}