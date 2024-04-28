import { FileAttachmentDto } from '@arslan/vms.base';
import { RoleUserDto } from '@arslan/vms.base';
import { ProductDto } from '@arslan/vms.base/proxy/product';

export namespace SalesOrder {

    export interface SalesOrder {
        id: string;
        customerId: string;
        headTechnicianId: string;
        userVehicleId: string;
        workorderTypeId: string;
        headTechnicianName: string;
        orderDate: string;
        vehicleReceiveDate: Date;
        paymentStatus: number;
        inventoryStatus: number;
        orderNumber: string;
        orderRemarks: string;
        total: number;
        discount: number;
        description: string;
        notes: string;
        kilometrage: number;
        vehicleReceiveFrom: string;
        isDeleted?: boolean;

        productLines: SalesOrderProductLine[];
        serviceLines: SalesOrderServiceLine[];
        files: FileAttachmentDto[];
    }

    export interface SalesOrderProductLine {
        id?: string;
        orderLineId: string;
        pickLineId: string;
        productId: string;
        locationId: string;
        pickDate: string;
        lineNum: number;
        description: string;
        productName: string;
        productIsDeleted: boolean;
        quantity: number;
        unitPrice: number;
        subTotal: number;
        isDeleted?: boolean;
    }

    export interface SalesOrderServiceLine {
        id?: string;
        productId: string;
        productName: string;
        productIsDeleted: boolean;
        technicianId: string;
        technicianName: string;
        lineNum: number;
        description: string;
        unitPrice: number;
        subTotal: number;
        isDeleted?: boolean;
        technicianIsDeleted?: boolean;
    }

    export class CreateUpdateSalesOrderDto {
        orderDate: Date;
        orderNumber: string;
        customerId: string;
        headTechnicianId: string;
        userVehicleId: string;
        workorderTypeId: string;
        LocationId: string;
        fakeId?: string;
        vehicleReceiveDate: Date;
        amountPaidid: number;
        balance: number;
        total: number;
        discount: number;
        description: string;
        notes: string;
        kilometrage: number;
        vehicleReceiveFrom: string;
        paymentStatus: number;
        inventoryStatus: number;
        files: FileAttachmentDto[];
        productLines: CreateUpdateSalesOrderProductLineDto[];
        serviceLines: CreateUpdateSalesOrderServiceLineDto[];
    }

    export class CreateUpdateSalesOrderProductLineDto {
        id?: string;
        salesorderId?: string;
        orderLineId?: string;
        pickLineId?: string;
        locationId?: string;
        productId?: string;
        lineNum: number;
        description?: string;
        quantity?: number;
        unitPrice?: number;
        subTotal?: number;
        isDeleted?: boolean;
    }

    export class CreateUpdateSalesOrderServiceLineDto {
        id?: string;
        salesorderId?: string;
        productId?: string;
        technicianId?: string;
        lineNum: number;
        description?: string;
        quantity?: number;
        unitPrice?: number;
        discount?: number;
        subTotal?: number;
        isDeleted?: boolean;
    }
}