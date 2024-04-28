export class CreateUpdateStockAdjustmentDto {
    id: string;
    remarks: string;
    status: string;
    adjustmentNumber: string;
    creationTime?: Date;
    isDeleted?: boolean;
    lines: CreateUpdateStockAdjustmentLine[] = [];
}

export class CreateUpdateStockAdjustmentLine {
    id?: string;
    productId: string;
    locationId: string;
    quantityBefore: number;
    quantityAfter: number;
    difference?: number;
    isDeleted?: boolean;
    fakeId?: number;
}
