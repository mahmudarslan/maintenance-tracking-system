export class SharedButtonModel {
    name: string;
    visible: boolean;
    isEditMode: boolean;
    actionName: string;
    hitCount: number = 0;
}

export class FileAttachmentDto {
    id?: string;
    fileName?: string;
    productId?: string;
    downloadUrl?: string;
    isDeleted: boolean;
    source: string;
} 