import { FileAttachmentDto } from '@arslan/vms.base';
import { RoleUserDto, } from '@arslan/vms.base';
import { ProductDto } from '@arslan/vms.base/proxy/product';

export namespace PurchaseOrder {

    export interface PurchaseOrder {
        vendorId: string;
        orderNumber: string;
        orderDate: Date;
        inventoryStatus: number;
        paymentStatus: number;
        discount: number;
        total: number;
        orderRemarks: string;
        isDeleted?: boolean;
        files: FileAttachmentDto[];
        productLines: PurchaseOrderProductLine[];
        serviceLines: PurchaseOrderServiceLine[];
        deletedProducts: ProductDto[];
        deletedServices: ProductDto[];
    }

    export interface PurchaseOrderProductLine {
        id?: string;
        productId: string;
        description: string;
        productName: string;
        productIsDeleted: boolean;
        locationId: string;
        quantity: number;
        unitPrice: number;
        orderLineId: string;
        receiveLineId: string;
        lineNum: number;
    }

    export interface PurchaseOrderServiceLine {
        id?: string;
        productId: string;
        description: string;
        productName: string;
        productIsDeleted: boolean;
        locationId: string;
        quantity: number;
        unitPrice: number;
        orderLineId: string;
        receiveLineId: string;
        lineNum: number;
    }

    export class CreateUpdatePurchaseOrderDto {
        id?: string;
        vendorId: string;
        orderNumber: string;
        orderDate: Date;
        inventoryStatus: number;
        paymentStatus: number;
        discount: number;
        total: number;
        orderRemarks: string;
        isDeleted?: boolean;
        fakeId?: boolean;
        files: FileAttachmentDto[];
        productLines: CreateUpdatePurchaseOrderProductLineDto[];
        serviceLines: CreateUpdatePurchaseOrderServiceLineDto[];
    }

    export class CreateUpdatePurchaseOrderProductLineDto {
        id?: string;
        productId?: string;
        description?: string;
        locationId?: string;
        quantity?: number;
        unitPrice?: number;
        subTotal?: number;
        salesorderId?: string;     
        isDeleted?: boolean;
        orderLineId?: string;
        receiveLineId?: string;
        lineNum: number;
    }

    export class CreateUpdatePurchaseOrderServiceLineDto {
        id?: string;
        productId?: string;
        description?: string;
        unitPrice?: number;
        subTotal?: number;
        isDeleted?: boolean;
        orderLineId?: string;
        receiveLineId?: string;
        lineNum: number;
    }
}