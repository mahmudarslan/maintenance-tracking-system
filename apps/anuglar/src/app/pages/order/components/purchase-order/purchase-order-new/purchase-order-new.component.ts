import { Component, OnInit, ViewChild } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import CustomStore from 'devextreme/data/custom_store';
import { ActivatedRoute } from '@angular/router';
import { DxDataGridComponent, DxFormComponent } from 'devextreme-angular';
import { environment } from 'src/environments/environment';
import validationEngine from 'devextreme/ui/validation_engine';
import { ProductDto, ProductService } from '@arslan/vms.base/proxy/product';
import { SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject } from 'rxjs';
import { AdjustStockService } from '@arslan/vms.inventory/proxy/adjust-stock';
import { ProductStockItem } from '@arslan/vms.inventory';
import { PurchaseOrderService } from '../../../proxy/purchase/purchase-order.service';
import { PurchaseOrder } from '../../../proxy/purchase';
import { VendorSearch, VendorService } from '@arslan/vms.base/proxy/vendor';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-purchase-order-new',
  templateUrl: './purchase-order-new.component.html'
})
export class PurchaseOrderNewComponent implements OnInit {

  @ViewChild('serviceGrid') serviceGrid: DxDataGridComponent;
  @ViewChild('productGrid') productGrid: DxDataGridComponent;
  @ViewChild('form') form: DxFormComponent;
  url: string = environment.apis.default.url + "rest/api/latest/vms/";
  productDataSource: CustomStore;
  labourDataSource: CustomStore;
  now: Date = new Date();
  minDate: Date = new Date(1900, 0, 1);
  workorderTypeData: any;
  purchaseOrderInventoryStatusData: any;
  purchaseOrderPaymentStatusData: any;
  serviceArrayData: ProductDto[] = [];
  productArrayData: ProductDto[] = [];
  vendorDataSource: any = {};
  purchaseOrder: PurchaseOrder.CreateUpdatePurchaseOrderDto = new PurchaseOrder.CreateUpdatePurchaseOrderDto();
  productServiceSourceArray: PurchaseOrder.CreateUpdatePurchaseOrderProductLineDto[] = [];
  labourServiceSourceArray: PurchaseOrder.CreateUpdatePurchaseOrderServiceLineDto[] = [];
  fakeId: any;
  discountAmount: number;
  isEditMode: boolean = false;
  calculatedToolPrice: number = 0;
  calculatedLabourPrice: number = 0;
  calculatedTotalPrice: number = 0;
  selectedVendor: VendorSearch;
  selectedVendorField: any;
  showSearchItem: boolean = true;
  selectedPurchaseOrderid: any;
  activeIndex: number = 1;
  result: PurchaseOrder.PurchaseOrder;
  value: any[] = [];
  purchaseOrderVisible: boolean = false;
  vendorFieldReadOnly: boolean = false;
  vendorColSpan: number = 2;
  sharedButton: SharedButtonModel = new SharedButtonModel();
  imageSource = "";
  vendorId: string;
  locationArrayData: LocationDto[] = [];
  productStockItems: ProductStockItem[] = [];
  loadingVisible: Boolean = false;
  isProductGridValidate?: boolean = null
  isServiceGridValidate?: boolean = null

  constructor(private poService: PurchaseOrderService,
    private adjustStockService: AdjustStockService,
    private productService: ProductService,
    private locationService: LocationService,
    private vendorService: VendorService,
    private sharedService: SharedService,
    private routeParams: ActivatedRoute) {
  }

  ngOnInit(): void {
    if (this.showSearchItem) {
      this.activeIndex = 0;
    }
    this.purchaseOrder.orderDate = this.now;

    this.productServiceStoreBind();
    this.labourServiceStoreBind();
    this.autoComplateDataBind();
    this.serviceDataBind();
    this.productDataBind();
    this.locationDataBind();
    this.inventoryStatusDataBind();
    this.paymentStatusDataBind();
    this.queryParameterBind();
    this.actionButtonBind();

    this.onReorder = this.onReorder.bind(this);
    this.validateVendorSelection = this.validateVendorSelection.bind(this);
    this.setProductPriceCellValue = this.setProductPriceCellValue.bind(this);
    this.setServicePriceCellValue = this.setServicePriceCellValue.bind(this);
    this.calculateProductSummary = this.calculateProductSummary.bind(this);
    this.calculateServiceSummary = this.calculateServiceSummary.bind(this);
  }

  Save() {

    this.saveProduct().then(t => {

      this.saveService().then(t => {
        if (this.isProductGridValidate != false && this.isServiceGridValidate != false) {
          this.crudAction();
        } else {
          this.sharedService.buttonComplate(this.sharedButton);
        }
      });

    });

  }

  crudAction() {
    let po = this.getFormData();

    if (this.isEditMode == true) {
      this.update(this.selectedPurchaseOrderid, po);
    } else {
      this.create(po);
    }
  }

  getFormData(): PurchaseOrder.CreateUpdatePurchaseOrderDto {
    let currentDate = new Date();

    this.saveProduct();
    this.saveService();

    this.purchaseOrder.orderDate = currentDate;
    this.purchaseOrder.total = this.calculatedTotalPrice;
    this.purchaseOrder.fakeId = this.fakeId;
    this.purchaseOrder.vendorId = this.selectedVendor.id;

    return this.purchaseOrder;
  }

  saveProduct(): Promise<boolean> {
    this.isProductGridValidate = null;
    return this.productGrid.instance.saveEditData().then(t => {
      if (this.isProductGridValidate) {
        this.purchaseOrder.productLines = this.productServiceSourceArray;
        this.purchaseOrder.productLines.forEach(element => {
          if (element.id.length < 5) {
            element.id = '00000000-0000-0000-0000-000000000000';
          }
        });
      }
      return this.isProductGridValidate;
    });
  }

  saveService(): Promise<boolean> {
    this.isServiceGridValidate = null;
    return this.serviceGrid.instance.saveEditData().then(t => {
      if (this.isServiceGridValidate) {
        this.purchaseOrder.serviceLines = this.labourServiceSourceArray;
        this.purchaseOrder.serviceLines.forEach(element => {
          if (element.id.length < 5) {
            element.id = '00000000-0000-0000-0000-000000000000';
          }
        });
      }
      return this.isServiceGridValidate;
    });
  }

  create(purchaseOrder) {
    this.poService.create(purchaseOrder).subscribe(() => {
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

  update(id: string, purchaseOrder) {
    this.poService.update(purchaseOrder, id).subscribe(() => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        "success",
        '/order/purchase-order/purchase-order-list');
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  //#region  Product Grid

  productServiceStoreBind() {
    this.productDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.productServiceSourceArray.filter(f => f.isDeleted == false || f.isDeleted == null)
      },
      // insert: (values) => {
      //   let length = this.productServiceSourceArray.length + 1;
      //   let insertItem: PurchaseOrder.CreateUpdatePurchaseOrderProductLineDto = {
      //     id: length.toString(),
      //     productId: values.productId,
      //     description: values.description,
      //     unitPrice: values.unitPrice,
      //     quantity: values.quantity,
      //     subTotal: values.unitPrice * values.quantity,
      //     locationId: values.locationId,
      //     lineNum: length,
      //   }
      //   this.productServiceSourceArray.push(insertItem);
      //   this.totalCalculation();
      //   return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.productServiceSourceArray });
      // },
      update: (key, values) => {
        let itemIndex = this.productServiceSourceArray.findIndex(t => t.id == key);
        if (values.productId != undefined) { this.productServiceSourceArray[itemIndex].productId = values.productId; }
        if (values.description != undefined) { this.productServiceSourceArray[itemIndex].description = values.description; }
        if (values.unitPrice != undefined) { this.productServiceSourceArray[itemIndex].unitPrice = values.unitPrice; }
        if (values.quantity != undefined) { this.productServiceSourceArray[itemIndex].quantity = values.quantity; }
        if (values.locationId != undefined) { this.productServiceSourceArray[itemIndex].locationId = values.locationId; }
        if (values.unitPrice != undefined || values.quantity != undefined) {
          this.productServiceSourceArray[itemIndex].subTotal = this.productServiceSourceArray[itemIndex].unitPrice * this.productServiceSourceArray[itemIndex].quantity;
        }
        this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.productServiceSourceArray });
      },
      remove: (key) => {
        let itemIndex = this.productServiceSourceArray.find(t => t.id == key);
        itemIndex.isDeleted = true;

        if (itemIndex.id.length < 5) {
          let itemIndex1 = this.productServiceSourceArray.findIndex(t => t.id == key);
          this.productServiceSourceArray.splice(itemIndex1, 1);
        }

        this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then();
      }
    });
  }

  calculateProductAmount(rowData) {
    return rowData.quantity * rowData.unitPrice;
  }

  calculateProductSummary(options) {
    if (options.name === "SelectedRowsSummary") {
      if (options.summaryProcess === "start") {
        options.totalValue = 0;
        this.calculatedToolPrice = options.totalValue;
      }
      else if (options.summaryProcess === "calculate") {
        if (options.value.unitPrice && options.value.quantity) {
          options.totalValue = options.totalValue + (options.value.unitPrice * options.value.quantity);
          this.calculatedToolPrice = options.totalValue;
          this.totalCalculation();
        }
      }
    }
  }

  setProductPriceCellValue(rowData, value) {
    rowData.productId = value;
    let product = this.getPrice(value);
    rowData.unitPrice = product.defaultPrice;

    if (!rowData.locationId) {
      rowData.locationId = this.locationArrayData[0].id;
    }
  }

  getPrice(id: string): ProductDto {
    let aa = this.productArrayData.find((f) => f.id == id);
    return aa;
  }

  setServicePriceCellValue(rowData, value) {
    rowData.productId = value;
    let product = this.getPrice1(value);
    rowData.unitPrice = product.defaultPrice;
  }

  getPrice1(id: string): ProductDto {
    let aa = this.serviceArrayData.find((f) => f.id == id);
    return aa;
  }


  totalProductPrice_valueChanged(e) {
    const newValue = e.value;
    this.calculatedToolPrice = newValue;
    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    this.discountAmount = this.calculatedTotalPrice * (this.purchaseOrder.discount ?? 0);
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  onReorder(e) {
    var visibleRows = e.component.getVisibleRows(),
      toIndex = this.productServiceSourceArray.indexOf(visibleRows[e.toIndex].data),
      fromIndex = this.productServiceSourceArray.indexOf(e.itemData);

    this.productServiceSourceArray[toIndex].lineNum = fromIndex + 1;
    this.productServiceSourceArray[fromIndex].lineNum = toIndex + 1;


    this.productServiceSourceArray.splice(fromIndex, 1);
    this.productServiceSourceArray.splice(toIndex, 0, e.itemData);
    this.productGrid.instance.refresh();
  }

  onRowValidatingProduct(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;
    this.isProductGridValidate = e.isValid;
  }

  //#endregion

  //#region  Service Grid

  labourServiceStoreBind() {
    this.labourDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.labourServiceSourceArray.filter(f => f.isDeleted == false || f.isDeleted == null)
      },
      // insert: (values) => {
      //   let length = this.labourServiceSourceArray.length + 1;
      //   let insertItem: PurchaseOrder.CreateUpdatePurchaseOrderServiceLineDto = {
      //     id: length.toString(),
      //     productId: values.productId,
      //     description: values.description,
      //     unitPrice: values.unitPrice,
      //     subTotal: values.unitPrice,
      //     lineNum: length,
      //   }
      //   this.labourServiceSourceArray.push(insertItem);
      //   this.totalCalculation();
      //   return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.labourServiceSourceArray });
      // },
      update: (key, values) => {
        let itemIndex = this.labourServiceSourceArray.findIndex(t => t.id == key);
        if (values.productId != undefined) { this.labourServiceSourceArray[itemIndex].productId = values.productId; }
        if (values.description != undefined) { this.labourServiceSourceArray[itemIndex].description = values.description; }
        if (values.unitPrice != undefined) { this.labourServiceSourceArray[itemIndex].unitPrice = values.unitPrice; }
        if (values.unitPrice != undefined) { this.labourServiceSourceArray[itemIndex].subTotal = values.unitPrice; }
        this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.labourServiceSourceArray });
      },
      remove: (key) => {
        let itemIndex = this.labourServiceSourceArray.find(t => t.id == key);
        itemIndex.isDeleted = true;

        if (itemIndex.id.length < 5) {
          let itemIndex1 = this.labourServiceSourceArray.findIndex(t => t.id == key);
          this.labourServiceSourceArray.splice(itemIndex1, 1);
        }

        this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then();
      }
    });
  }

  calculateServiceSummary(options) {
    if (options.name === "SelectedRowsSummary") {
      if (options.summaryProcess === "start") {
        options.totalValue = 0;
        this.calculatedLabourPrice = options.totalValue;
      }
      else if (options.summaryProcess === "calculate") {
        if (options.value.unitPrice) {
          options.totalValue = options.totalValue + options.value.unitPrice;
          this.calculatedLabourPrice = options.totalValue;
          this.totalCalculation();
        }
      }
    }
  }

  totalServicePrice_valueChanged(e) {
    const newValue = e.value;

    this.calculatedLabourPrice = newValue;
    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    this.discountAmount = this.calculatedTotalPrice * (this.purchaseOrder.discount ?? 0);
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  onRowValidatingService(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;
    this.isServiceGridValidate = e.isValid;
  }

  //#endregion


  onToolbarPreparing(e) {
    let toolbarItems = e.toolbarOptions.items;
    // Modifies an existing item
    toolbarItems.forEach((item) => {
      if (item.name === "saveButton") {
        item.visible = false;
      }
    });
  }

  setVendor(event) {
    if (event === 'undefined' || event == null) return;
    this.selectedVendor = event.selectedItem;
  }

  setVendorSearchSelection(userVehicleId) {
    if (userVehicleId !== undefined && userVehicleId != null) {
      this.vendorService.searchVendorById(userVehicleId).subscribe(item => {
        this.selectedVendor = item
        this.selectedVendorField = item.selectionField;
      });
    }
  }

  discount_valueChanged(e) {
    const newValue = e.value;
    this.purchaseOrder.discount = newValue;

    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    this.discountAmount = this.calculatedTotalPrice * (this.purchaseOrder.discount ?? 0);
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  totalCalculation() {
    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    if (this.calculatedTotalPrice && this.purchaseOrder.discount) {
      this.discountAmount = this.calculatedTotalPrice * (this.purchaseOrder.discount ?? 0);
    } else {
      this.discountAmount = 0;
    }
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  clearForm() {
    this.purchaseOrder = new PurchaseOrder.CreateUpdatePurchaseOrderDto();
    this.discountAmount = 0;
    this.productServiceSourceArray = [];
    this.labourServiceSourceArray = [];
    this.discountAmount = 0;
    this.calculatedToolPrice = 0;
    this.calculatedLabourPrice = 0;
    this.calculatedTotalPrice = 0;

    setTimeout(() => {
      this.serviceGrid.instance.refresh();
      this.productGrid.instance.refresh();
      this.serviceGrid.instance.cancelEditData();
      this.productGrid.instance.cancelEditData();
    }, 100);
    validationEngine.resetGroup();
  }

  validateVendorSelection(e) {
    return this.selectedVendor != undefined && this.selectedVendor.id != undefined
  }

  //#region Bind

  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedPurchaseOrderid = params.get("id");
      let parameterIsDeleted = params.get("isDeleted") == "true" ? true : false;

      if (this.selectedPurchaseOrderid != null && this.selectedPurchaseOrderid.length > 0) {
        this.showSearchItem = false;
        this.isEditMode = true;
        this.vendorColSpan = 1;
        this.vendorFieldReadOnly = true;
        this.formDataBind(this.selectedPurchaseOrderid, parameterIsDeleted);
        this.vendorFieldReadOnly = this.isEditMode;
        this.purchaseOrderVisible = this.isEditMode;
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      } else {
        this.isEditMode = false;
        this.vendorColSpan = 2;

        this.vendorFieldReadOnly = this.isEditMode;
        this.purchaseOrderVisible = this.isEditMode;
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
        this.clearForm();
      }
    });
  }

  actionButtonBind() {
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
  }

  formDataBind(id: string, isDeleted: boolean) {
    this.loadingVisible = true;

    this.poService.get(id, isDeleted).subscribe(
      {
        next: result => {
          this.result = result;
          this.purchaseOrder.orderNumber = result.orderNumber;
          this.purchaseOrder.orderDate = result.orderDate;
          this.productServiceSourceArray = result.productLines;
          this.labourServiceSourceArray = result.serviceLines;
          this.purchaseOrder.vendorId = result.vendorId;
          this.purchaseOrder.orderRemarks = result.orderRemarks;
          this.purchaseOrder.inventoryStatus = result.inventoryStatus;
          this.purchaseOrder.paymentStatus = result.paymentStatus;
          this.discountAmount = result.discount;
          this.setVendorSearchSelection(result.vendorId);
          this.totalCalculation();
          this.serviceGrid.instance.refresh();
          this.productGrid.instance.refresh();

          //Deleted location add
          for (let productLine of result.productLines) {
            if (!this.locationArrayData.find(f => f.id == productLine.locationId)) {
              this.locationService.getLocation(productLine.locationId).toPromise().then((t) => {
                this.locationArrayData = this.locationArrayData.concat(t);
              });
            }
          }
          //deleted product add
          let deletedProducts = result.productLines.filter(f => f.productIsDeleted == true);
          if (deletedProducts?.length > 0) {
            let s = deletedProducts.map(val => <ProductDto>({
              id: val.productId,
              name: val.productName
            })).filter((thing, i, arr) => arr.findIndex(t => t.id === thing.id) === i);

            this.productArrayData = this.productArrayData.concat(s);
          }
          //deleted service add
          let deletedServices = result.serviceLines.filter(f => f.productIsDeleted == true);
          if (deletedServices?.length > 0) {
            let s = deletedServices.map(val => <ProductDto>({
              id: val.productId,
              name: val.productName
            })).filter((thing, i, arr) => arr.findIndex(t => t.id === thing.id) === i);

            this.serviceArrayData = this.serviceArrayData.concat(s);
          }



          if (result.isDeleted) {
            this.sharedService.hideButton();
            this.form.disabled = true;
          }

          this.loadingVisible = false;
        },
        error: (e) => {
          this.loadingVisible = false;
          console.log(e);
        },
        complete: () => {
          this.loadingVisible = false;
        }
      },
    );
  }

  inventoryStatusDataBind() {
    this.poService.getInventoryStatus().toPromise().then((result) => {
      this.purchaseOrderInventoryStatusData = result;
    });
  }

  paymentStatusDataBind() {
    this.poService.getPaymentStatus().toPromise().then((result) => {
      this.purchaseOrderPaymentStatusData = result;
    });
  }

  serviceDataBind() {
    this.productService.getServiceTypeItems().toPromise().then((t) => {
      this.serviceArrayData = this.serviceArrayData.concat(t);
    });
  }

  productDataBind() {
    this.productService.getStockUnstockTypeItems().toPromise().then((t) => {
      this.productArrayData = this.productArrayData.concat(t);
    });
  }

  locationDataBind() {
    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  autoComplateDataBind() {
    this.vendorDataSource = new DataSource({
      store: new CustomStore({
        key: "ID",
        load: (loadOptions) => {
          return this.vendorService.bySearchContent(loadOptions["searchValue"]).toPromise();
        },
        remove: (key) => {
          return this.vendorService.delete(this.url + 'identity/vendor/BySearchContentt' + encodeURIComponent(key)).toPromise().then();
        }
      })
    });
  }

  //#endregion
}