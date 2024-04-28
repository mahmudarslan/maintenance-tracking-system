import { FileAttachmentDto } from "@arslan/vms.base";

export class State {
  products: Response;
}

export class Response {
  items: ProductDto[];
  totalCount: number;
}

export class ServiceState {
  services: ProductDto[];
}

export class ProductValueDto {
  id: string;
  name: string;
}

export class ProductDto {
  id?: string;
  name: string;
  productType?: string;
  productCategoryId?: string;
  barcode?: String;
  cost?: number;
  defaultPrice?: number;
  defaultStockCount?: number;
  reorderPoint?: number;
  prices?: Price[];
  stockAdjustmentLines?: stockAdjustmentLine[];
  defaultLocationId?: string;
  creationTime?: Date;
  lastModificationTime?: Date;
  files?: FileAttachmentDto[];
  isDeleted?: boolean;
}

export class CreateUpdateProductDto {
  id: string;
  fakeId?: string;
  name: string;
  productType: string;
  barcode: String;
  cost: number;
  reorderPoint: number;
  defaultPrice: number;
  defaultStockCount: number;
  prices: Price[] = [];
  stockAdjustmentLines: stockAdjustmentLine[] = [];
  defaultLocationId?: string;
  productCategoryId: string;
  isDeleted?: boolean;
}

export class Price {
  unitPrice: number;
  id?: string;
}

export class stockAdjustmentLine {
  id?: string;
  locationId: string;
  quantityAfter: number;
  isDeleted?: boolean;
}

export class StockedProductState {
  stockedProduct: ProductDto[];
}

export namespace ProductType {
  export interface State {
    productType: ProductType[];
  }

  export interface Response {
    items: ProductType[];
    totalCount: number;
  }

  export interface ProductType {
    id: string;
    productTypeName: string;
    // description:string;      
  }
}

export class CategoryDto {
  id?: string;
  parentId?: string;
  name: string;
  isDeleted: boolean;
}

export class CreateUpdateCategoryDto {
  id: string;
  parentId?: string;
  name: string;
  isDeleted: boolean;
}