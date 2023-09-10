import { Component, OnInit, ViewChild } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { ConfigStateService } from '@abp/ng.core';
import { ActivatedRoute, Router } from '@angular/router';
import { DxDataGridComponent } from 'devextreme-angular';
import { environment } from 'src/environments/environment';
import validationEngine from 'devextreme/ui/validation_engine';
import { CustomerSearch } from '@arslan/vms.base';
import { ProductDto, ProductService } from '../../../base/proxy/product';
import { SalesOrder } from '@arslan/vms.order/proxy/order';
import { AdjustStockService, CreateUpdateStockAdjustmentDto, CreateUpdateStockAdjustmentLine } from '../../proxy/adjust-stock';
import { SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject, of, throwError } from 'rxjs';
import { map } from 'rxjs/operators';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-reorder-stock',
  templateUrl: './reorder-stock.component.html',
})
export class ReorderStockComponent implements OnInit {

  url: string = environment.apis.default.url + "rest/api/latest/vms/";
  stockAdjustmentLineDataSource: CustomStore;
  @ViewChild('productGrid') productGrid: DxDataGridComponent;
  now: Date = new Date();
  minDate: Date = new Date(1900, 0, 1);
  productArrayData: ProductDto[] = [];
  locationArrayData: LocationDto[] = [];
  stockAdjustment: CreateUpdateStockAdjustmentDto = new CreateUpdateStockAdjustmentDto();
  stockAdjustmentLines: CreateUpdateStockAdjustmentLine[] = [];
  fakeId: any;
  discountAmount: number;
  isEditMode: boolean = false;
  calculatedToolPrice: number = 0;
  calculatedLabourPrice: number = 0;
  calculatedTotalPrice: number = 0;
  selectedCustomer: CustomerSearch;
  selectedCustomerField: any;
  showSearchItem: boolean = true;
  selectedSalesOrderid: any;
  activeIndex: number = 1;
  result: SalesOrder.SalesOrder;
  value: any[] = [];
  uploadHeaders: any;
  uploadFakeId = 0;
  salesOrderVisible: boolean = false;
  customerFieldReadOnly: boolean = false;
  customerColSpan: number = 2;
  sharedButton: SharedButtonModel = new SharedButtonModel();

  constructor(
    private adjustStockService: AdjustStockService,
    private productService: ProductService,
    private routeParams: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService,
    private locationService: LocationService,
    private configState: ConfigStateService) {
  }

  ngOnInit(): void {
    this.stockDataBind();
    this.productBind();
    this.locationBind();

    this.setProductPriceCellValue = this.setProductPriceCellValue.bind(this);

    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedSalesOrderid = params.get("id");
      if (this.selectedSalesOrderid != null && this.selectedSalesOrderid.length > 0) {
        this.showSearchItem = false;
        this.isEditMode = true;
        this.customerColSpan = 1;
        this.customerFieldReadOnly = true;
        this.editmode(this.selectedSalesOrderid);
        this.customerFieldReadOnly = this.isEditMode;
        this.salesOrderVisible = this.isEditMode;
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      } else {
        this.isEditMode = false;
        this.customerColSpan = 2;
        this.clearForm();
        this.customerFieldReadOnly = this.isEditMode;
        this.salesOrderVisible = this.isEditMode;
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      }

      this.sharedService.bClickSendSubject$ = new BehaviorSubject(new SharedButtonModel());

      this.sharedService.bClickSendSubject$.subscribe(s => {
        if (s && !s.name) return;
        validationEngine.validateGroup();

        if (!validationEngine.validateGroup().isValid) {
          this.sharedService.bComplatedSubject$.next(this.sharedButton);
        } else {
          this.Save();
        }
      });
    });



    if (this.showSearchItem) {
      this.activeIndex = 0;
    }
  }

  calculateProductAmount(rowData) {
    return rowData.quantity * rowData.unitPrice;
  }
  setProductPriceCellValue(rowData, value, currentData) {
    rowData[(<any>this).dataField] = value;
    let product = this.getPrice(value);
    rowData.unitPrice = product.defaultPrice;
  }

  getPrice(id: string): ProductDto {
    let aa = this.productArrayData.find((f) => f.id == id);
    return aa;
  }

  productBind() {
    this.productService.getStockUnstockTypeItems().toPromise().then((t) => {
      this.productArrayData = this.productArrayData.concat(t);
    });
  }

  locationBind() {
    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  stockDataBind() {
    this.stockAdjustmentLineDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.stockAdjustmentLines
      },
      insert: (values) => {
        let length = this.stockAdjustmentLines.length;
        let insertItem: CreateUpdateStockAdjustmentLine = {
          id: (length++).toString(),
          productId: values.productId,
          locationId: values.locationId,
          quantityAfter: values.quantityAfter,
          quantityBefore: values.quantityBefore
          //description: values.description,
          //unitPrice: values.unitPrice,
          //quantity: values.quantity,
          //subTotal: values.unitPrice * values.quantity
        }
        this.stockAdjustmentLines.push(insertItem);
        //this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.stockAdjustmentLines }).then();
      },
      update: (key, values) => {
        let itemIndex = this.stockAdjustmentLines.findIndex(t => t.id == key);
        if (values.productId != undefined) { this.stockAdjustmentLines[itemIndex].productId = values.productId; }
        if (values.locationId != undefined) { this.stockAdjustmentLines[itemIndex].locationId = values.locationId; }
        if (values.quantityAfter != undefined) { this.stockAdjustmentLines[itemIndex].quantityAfter = values.quantityAfter; }
        // if (values.unitPrice != undefined) { this.stockAdjustmentLines[itemIndex].unitPrice = values.unitPrice; }
        // if (values.quantity != undefined) { this.stockAdjustmentLines[itemIndex].quantity = values.quantity; }
        // if (values.unitPrice != undefined || values.quantity != undefined) {
        //   // this.productServiceSourceArray[itemIndex].subTotal = this.productServiceSourceArray[itemIndex].unitPrice * this.productServiceSourceArray[itemIndex].quantity;
        // }
        //this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.stockAdjustmentLines }).then();
      },
      remove: (key) => {
        let itemIndex = this.stockAdjustmentLines.find(t => t.id == key);
        itemIndex.isDeleted = true;

        //this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.stockAdjustmentLines }).then();
      }
    });
  }



  clearForm() {
    this.stockAdjustment = new CreateUpdateStockAdjustmentDto();
    this.discountAmount = 0;
    this.stockAdjustmentLines = [];
    this.discountAmount = 0;
    this.calculatedToolPrice = 0;
    this.calculatedLabourPrice = 0;
    this.calculatedTotalPrice = 0;
    this.uploadFakeId = 0;

    setTimeout(() => {
      this.productGrid.instance.refresh();
    }, 100);

    validationEngine.resetGroup();
  }

  editmode(id: string) {
    //this.adjustStockService.getById(this.selectedSalesOrderid).toPromise().then(result => {
    // this.result = result;
    // this.salesorder.description = result.description;
    // this.salesorder.orderNumber = result.orderNumber;
    // this.salesorder.kilometrage = result.kilometrage;
    // this.salesorder.vehicleReceiveFrom = result.vehicleReceiveFrom;
    // this.salesorder.vehicleReceiveDate = result.vehicleReceiveDate;
    // this.salesorder.notes = result.notes;
    // this.salesorder.headTechnicianId = result.headTechnicianId;
    // this.productServiceSourceArray = result.lines;
    // this.labourServiceSourceArray = result.lines;
    // this.salesorder.discount = result.discount;
    // this.salesorder.workorderTypeId = result.workorderTypeId;
    // this.salesorder.customerId = result.customerId;
    // this.salesorder.salesOrderStatusId = result.salesOrderStatusId;

    // this.productServiceSourceArray = result.lines.filter(q => q.productType != 3);
    // this.labourServiceSourceArray = result.lines.filter(q => q.productType == 3);
    // this.fileSourceArray = result.files;
    // this.setCustomerSearchSelection(result.userVehicleId);
    // this.totalCalculation();
    // this.uploadGrid.instance.refresh();
    // this.serviceGrid.instance.refresh();
    // this.productGrid.instance.refresh();

    // if (result.deletedTechnicians.length > 0) {
    //   this.technicianServiceData = this.technicianServiceData.concat(result.deletedTechnicians);
    // }
    // if (result.deletedHeadTechnician != null) {
    //   this.headTechnitionArrayData.push(result.deletedHeadTechnician);
    // }
    // if (result.deletedProducts.length > 0) {
    //   this.productArrayData = this.productArrayData.concat(result.deletedProducts);
    // }
    // if (result.deletedServices.length > 0) {
    //   this.serviceArrayData = this.serviceArrayData.concat(result.deletedServices);
    // }
    //});
  }


  Save() {

    this.productGrid.instance.saveEditData();

    this.stockAdjustment.lines = [];
    this.stockAdjustment.lines = this.stockAdjustmentLines;
    //this.salesorder.fakeId = this.fakeId;

    this.stockAdjustment.lines.forEach(element => {
      if (element.id.length < 5) {
        element.id = '00000000-0000-0000-0000-000000000000';
      }
    });

    if (this.isEditMode == true) {
      this.Update(this.selectedSalesOrderid);
    } else {
      this.Create();
    }
  }

  Create() {
    this.adjustStockService.create(this.stockAdjustment).subscribe(() => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        "success");
      this.clearForm();
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  Update(id: string) {
    this.adjustStockService.update(id, this.stockAdjustment).subscribe(() => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        "success",
        '/inventory/adjust-stock/');
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  onToolbarPreparing(e) {
    let toolbarItems = e.toolbarOptions.items;
    // Modifies an existing item
    toolbarItems.forEach(function (item) {
      if (item.name === "saveButton") {
        item.visible = false;
      }
    });
  }

}
