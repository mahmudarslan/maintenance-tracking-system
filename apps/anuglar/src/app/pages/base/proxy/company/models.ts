import { FileAttachmentDto } from "@arslan/vms.base";
import { Address } from "../address";

export class CompanyDto {
    id: string;
    name: string;
    email: string;
    phoneNumber: string;
    birthDate: string;
    faxNumber: string;
    fakeId?: string;
    addresses: Address[];
    files: FileAttachmentDto[];
}

export class CreateUpdateCompany {
    id?: string;
    addresses: Address[];
    name: string;
    email: string;
    phoneNumber: string;
    birthDate: string;
    faxNumber: string;
    fakeId?: string;
}