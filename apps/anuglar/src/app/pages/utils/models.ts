export class SharedButtonModel {
    name: string;
    visible: boolean;
    isEditMode: boolean;
    actionName: string;
    hitCount: number = 0;
}

export class EnumDto {
    id: number;
    name: string;
}

export class GuidEnumDto {
    id: string;
    name: string;
}

export class GuidEnumAndBoolDto {
    id: string;
    name: string;
    isTrue:boolean;
}

export class BoolEnumDto {
    id: boolean;
    name: string;
}

export enum NotifyType {
    "success",
    "info",
    "warning",
    "error"
}