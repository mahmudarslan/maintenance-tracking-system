import { Component, OnInit, ViewChild } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import CustomStore from 'devextreme/data/custom_store';
import { ApiInterceptor } from '@abp/ng.core';
import { ActivatedRoute } from '@angular/router';
import { DxDataGridComponent, DxFileUploaderComponent, DxFormComponent, DxSelectBoxComponent } from 'devextreme-angular';
import { Guid } from "guid-typescript";
import { environment } from 'src/environments/environment';
import validationEngine from 'devextreme/ui/validation_engine';
import { CustomersService, CustomerVehicleDto } from '@arslan/vms.base'
import { EmployeeService } from '@arslan/vms.base'
import { RoleUserDto } from '@arslan/vms.base';
import { CustomerSearch } from '@arslan/vms.base';
import { SalesOrder, SalesOrderService } from '../../../proxy/order';
import { ProductDto, ProductService } from '@arslan/vms.base/proxy/product';
import { FileAttachmentDto, HelperService, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject } from 'rxjs';
import { AdjustStockService } from '@arslan/vms.inventory/proxy/adjust-stock';
import { ProductStockItem } from '@arslan/vms.inventory';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-sales-order-new',
  templateUrl: './sales-order-new.component.html',
  styleUrls: ['./sales-order-new.component.scss'],
  preserveWhitespaces: true
})
export class SalesOrderNewComponent implements OnInit {

  @ViewChild('vehicleSelectBox') vehicleSelectBox: DxSelectBoxComponent;
  @ViewChild('serviceGrid') serviceGrid: DxDataGridComponent;
  @ViewChild('productGrid') productGrid: DxDataGridComponent;
  @ViewChild('uploadGrid') uploadGrid: DxDataGridComponent;
  @ViewChild('fileUploader') fileUploader: DxFileUploaderComponent;
  @ViewChild('form') form: DxFormComponent;
  url: string = environment.apis.default.url + "rest/api/latest/vms/";
  uploadUrl = this.url + "core/file/upload";
  downloadUrl = "rest/api/latest/vms/core/file/download?id=";
  productDataSource: CustomStore;
  labourDataSource: CustomStore;
  uploadDataSource: any;
  now: Date = new Date();
  minDate: Date = new Date(1900, 0, 1);
  workorderTypeData: any;
  salesorderStatusData: any;
  customerVehicleArrayData: CustomerVehicleDto[] = [];;
  headTechnitionArrayData: RoleUserDto[] = [];
  technicianServiceArrayData: RoleUserDto[] = [];
  serviceArrayData: ProductDto[] = [];
  productArrayData: ProductDto[] = [];
  customerDataSource: any = {};
  salesorder: SalesOrder.CreateUpdateSalesOrderDto = new SalesOrder.CreateUpdateSalesOrderDto();
  productServiceSourceArray: SalesOrder.CreateUpdateSalesOrderProductLineDto[] = [];
  labourServiceSourceArray: SalesOrder.CreateUpdateSalesOrderServiceLineDto[] = [];
  fileSourceArray: FileAttachmentDto[] = [];
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
  imageSource = "";
  fileCount: number = 0;
  customerId: string;
  locationArrayData: LocationDto[] = [];
  productStockItems: ProductStockItem[] = [];
  loadingVisible: Boolean = false;
  salesOrderInventoryStatusData: any;
  salesOrderPaymentStatusData: any;
  isProductGridValidate?: boolean = null
  isServiceGridValidate?: boolean = null

  constructor(private soService: SalesOrderService,
    private adjustStockService: AdjustStockService,
    private productService: ProductService,
    private customerService: CustomersService,
    private locationService: LocationService,
    private employeeService: EmployeeService,
    private sharedService: SharedService,
    private helperService: HelperService,
    private routeParams: ActivatedRoute,
    private apiInterceptor: ApiInterceptor,) {
  }

  ngOnInit(): void {

    this.fileCount = 0;
    this.salesorder.vehicleReceiveDate = this.now;
    if (this.showSearchItem) {
      this.activeIndex = 0;
    }

    this.queryParameterBind();
    this.actionButtonBind();
    this.uploadDataBind();
    this.productDataBind();
    this.serviceDataBind();
    this.workorderTypeBind();
    this.headTechnicianBind();
    this.technicianBind();
    this.serviceBind();
    this.productBind();
    this.locationBind();
    this.autoComplateDataBind();
    this.inventoryStatusDataBind();
    this.paymentStatusDataBind();
    this.statusDataBind();

    this.setUploadHeader();

    this.validateCustomerSelection = this.validateCustomerSelection.bind(this);
    this.setProductPriceCellValue = this.setProductPriceCellValue.bind(this);
    this.setServicePriceCellValue = this.setServicePriceCellValue.bind(this);
    this.calculateProductSummary = this.calculateProductSummary.bind(this);
    this.calculateServiceSummary = this.calculateServiceSummary.bind(this);
    this.addIdParameter = this.addIdParameter.bind(this);
    this.downloadIconClick = this.downloadIconClick.bind(this);
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
    let so = this.getFormData();

    if (this.isEditMode == true) {
      this.update(this.selectedSalesOrderid, so);
    } else {
      this.create(so);
    }
  }

  getFormData(): SalesOrder.CreateUpdateSalesOrderDto {
    let currentDate = new Date();

    this.saveProduct();
    this.saveService();

    this.salesorder.orderDate = currentDate;
    this.salesorder.files = this.fileSourceArray;
    this.salesorder.total = this.calculatedTotalPrice;
    this.salesorder.fakeId = this.fakeId;
    this.salesorder.customerId = this.selectedCustomer.id;

    this.salesorder.files.forEach(element => {
      if (element.id.length < 5) {
        element.id = '00000000-0000-0000-0000-000000000000';
      }
    });

    return this.salesorder;
  }

  saveProduct(): Promise<boolean> {
    this.isProductGridValidate = null;
    return this.productGrid.instance.saveEditData().then(t => {
      if (this.isProductGridValidate) {
        this.salesorder.productLines = this.productServiceSourceArray;
        this.salesorder.productLines.forEach(element => {
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
        this.salesorder.serviceLines = this.labourServiceSourceArray;
        this.salesorder.serviceLines.forEach(element => {
          if (element.id.length < 5) {
            element.id = '00000000-0000-0000-0000-000000000000';
          }
        });
      }
      return this.isServiceGridValidate;
    });
  }

  create(salesorder) {
    this.soService.create(salesorder).subscribe(() => {
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

  update(id: string, salesorder) {
    this.soService.update(salesorder, id).subscribe(() => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        "success",
        '/order/sales-order/sales-order-list');
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }



  //#region  Product Grid

  productDataBind() {
    this.productDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.productServiceSourceArray.filter(f => f.isDeleted == false || f.isDeleted == null)
      },
      // insert: (values) => {
      //   let length = this.productServiceSourceArray.length + 1;
      //   let insertItem: SalesOrder.CreateUpdateSalesOrderProductLineDto = {
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
      //   return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.productServiceSourceArray }).catch(e => { console.log(e) });
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
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.productServiceSourceArray }).catch(e => { console.log(e) });
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
    //rowData[(<any>this).dataField] = value;
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
    //rowData[(<any>this).dataField] = value;
    rowData.productId = value;
    let product = this.getPrice1(value);
    rowData.unitPrice = product.defaultPrice;
  }

  getPrice1(id: string): ProductDto {
    let aa = this.serviceArrayData.find((f) => f.id == id);
    return aa;
  }

  onRowValidatingProduct(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;
    this.isProductGridValidate = e.isValid;
  }

  //#endregion


  //#region  Service Grid

  serviceDataBind() {
    this.labourDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.labourServiceSourceArray.filter(f => f.isDeleted == false || f.isDeleted == null)
      },
      // insert: (values) => {
      //   let length = this.labourServiceSourceArray.length + 1;
      //   let insertItem: SalesOrder.CreateUpdateSalesOrderServiceLineDto = {
      //     id: length.toString(),
      //     productId: values.productId,
      //     description: values.description,
      //     unitPrice: values.unitPrice,
      //     subTotal: values.unitPrice,
      //     technicianId: values.technicianId,
      //     lineNum: length,
      //   }
      //   this.labourServiceSourceArray.push(insertItem);
      //   this.totalCalculation();
      //   return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.labourServiceSourceArray }).catch(e => { console.log(e) });
      // },
      update: (key, values) => {
        let itemIndex = this.labourServiceSourceArray.findIndex(t => t.id == key);
        if (values.productId != undefined) { this.labourServiceSourceArray[itemIndex].productId = values.productId; }
        if (values.description != undefined) { this.labourServiceSourceArray[itemIndex].description = values.description; }
        if (values.unitPrice != undefined) { this.labourServiceSourceArray[itemIndex].unitPrice = values.unitPrice; }
        if (values.technicianId != undefined) { this.labourServiceSourceArray[itemIndex].technicianId = values.technicianId; }
        if (values.unitPrice != undefined) { this.labourServiceSourceArray[itemIndex].subTotal = values.unitPrice; }
        this.totalCalculation();
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.labourServiceSourceArray }).catch(e => { console.log(e) });
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

  onRowValidatingService(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;
    this.isServiceGridValidate = e.isValid;
  }
  //#endregion




  clearForm() {
    this.salesorder = new SalesOrder.CreateUpdateSalesOrderDto();
    this.discountAmount = 0;
    this.productServiceSourceArray = [];
    this.labourServiceSourceArray = [];
    this.customerVehicleArrayData = [];
    this.fileSourceArray = [];
    this.discountAmount = 0;
    this.calculatedToolPrice = 0;
    this.calculatedLabourPrice = 0;
    this.calculatedTotalPrice = 0;
    this.uploadFakeId = 0;


    setTimeout(() => {
      //this.customerVehicleData = null;
      this.fileUploader.instance.reset();
      this.uploadGrid.resetOptions();
      this.uploadGrid.instance.refresh();
      this.serviceGrid.instance.refresh();
      this.productGrid.instance.refresh();
      this.uploadGrid.instance.cancelEditData();
      this.serviceGrid.instance.cancelEditData();
      this.productGrid.instance.cancelEditData();
    }, 100);

    this.setUploadHeader();
    validationEngine.resetGroup();
  }

  setCustomer(event) {
    if (event === 'undefined' || event == null) return;
    this.selectedCustomer = event.selectedItem;
    //this.salesorder.userVehicleId = this.selectedCustomer.userVehicleId;

    this.customerVehiclesBind(this.selectedCustomer.id);
  }

  setCustomerSearchSelection(userVehicleId) {
    if (userVehicleId !== undefined && userVehicleId != null) {
      this.customerService.searchCustomerById(userVehicleId).subscribe(item => {
        this.selectedCustomer = item
        this.selectedCustomerField = item.selectionField;
      });
    }
  }

  validateCustomerSelection(e) {
    return this.selectedCustomer != undefined && this.selectedCustomer.id != undefined
  }


  onToolbarPreparing(e) {
    let toolbarItems = e.toolbarOptions.items;
    // Modifies an existing item
    toolbarItems.forEach((item) => {
      if (item.name === "saveButton") {
        item.visible = false;
      }
    });
  }


  totalProductPrice_valueChanged(e) {
    const newValue = e.value;
    this.calculatedToolPrice = newValue;
    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    this.discountAmount = this.calculatedTotalPrice * this.salesorder.discount;
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  totalServicePrice_valueChanged(e) {
    const newValue = e.value;

    this.calculatedLabourPrice = newValue;
    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    this.discountAmount = this.calculatedTotalPrice * this.salesorder.discount;
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  discount_valueChanged(e) {
    const newValue = e.value;
    this.salesorder.discount = newValue;

    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    this.discountAmount = (this.calculatedTotalPrice * this.salesorder.discount);
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  totalCalculation() {
    this.calculatedTotalPrice = this.calculatedToolPrice + this.calculatedLabourPrice;
    if (this.calculatedTotalPrice && this.salesorder.discount) {
      this.discountAmount = this.calculatedTotalPrice * this.salesorder.discount;
    } else {
      this.discountAmount = 0;
    }
    this.calculatedTotalPrice = this.calculatedTotalPrice - this.discountAmount;
  }

  //#region Upload

  updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
      return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
      return uri + separator + key + "=" + value;
    }
  }

  setUploadHeader() {
    //const currentTenant = this.configState.getOne('currentTenant') as CurrentTenantDto;
    this.fakeId = (Guid.create()).toString();
    this.uploadHeaders = this.apiInterceptor.getAdditionalHeaders();
  }

  addIdParameter(e) {
    this.uploadUrl = this.updateQueryStringParameter(this.uploadUrl, "id", this.fakeId);
    e.component.option("uploadUrl", this.uploadUrl);

    e.value.forEach((value) => {
      let isPreValue = false;

      if (e.previousValue.length > 0) {
        e.previousValue.forEach((value1) => {
          if (value1.name == value.name) { isPreValue = true; }
        });
      }

      if (!isPreValue) {
        this.uploadFakeId += 1;

        const file = e.value[this.fileCount];

        if (e.value.length == this.fileCount) {
          this.fileCount = 0;
        } else {
          this.fileCount += 1;
        }

        const fileReader = new FileReader();
        fileReader.onload = () => {
          this.fileSourceArray.push(
            {
              id: this.uploadFakeId.toString(),
              downloadUrl: '/',
              fileName: value.name,
              isDeleted: false,
              source: fileReader.result as string
            });
          this.uploadGrid.instance.refresh();
        }
        fileReader.readAsDataURL(file);
      }
    });
  }

  uploadDataBind() {
    this.uploadDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.fileSourceArray.filter(f => f.isDeleted == false);
      },
      update: (key, values) => {
        let itemIndex = this.fileSourceArray.findIndex(t => t.id == key);
        // if (values.name != undefined) {
        //   this.fileSourceArray[itemIndex].fileName = values.name;
        // }
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.fileSourceArray }).catch(e => { console.log(e) });
      },
      remove: (key) => {
        let itemIndex = this.fileSourceArray.findIndex(t => t.id == key);
        {
          this.fileSourceArray[itemIndex].isDeleted = true;
        }
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then();
      }
    });
  }

  downloadIconClick(e) {
    this.helperService.download1(e.row.data.id, e.row.data.fileName, e.row.data.extension);
  }

  onUploaded(e) {
    // const file = e.file;
    // const fileReader = new FileReader();
    // fileReader.onload = () => { 
    //   //this.imageSource = fileReader.result as string;
    // }
    // fileReader.readAsDataURL(file); 
  }

  //#endregion


  //#region Bind

  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedSalesOrderid = params.get("id");
      let parameterIsDeleted = params.get("isDeleted") == "true" ? true : false;

      if (this.selectedSalesOrderid != null && this.selectedSalesOrderid.length > 0) {
        this.showSearchItem = false;
        this.isEditMode = true;
        this.customerColSpan = 1;
        this.customerFieldReadOnly = true;
        this.formDataBind(this.selectedSalesOrderid, parameterIsDeleted);
        this.customerFieldReadOnly = this.isEditMode;
        this.salesOrderVisible = this.isEditMode;
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      } else {
        this.isEditMode = false;
        this.customerColSpan = 2;

        this.customerFieldReadOnly = this.isEditMode;
        this.salesOrderVisible = this.isEditMode;
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

    this.soService.get(id, isDeleted).subscribe(

      {
        next: result => {
          this.result = result;
          this.salesorder.description = result.description;
          this.salesorder.orderNumber = result.orderNumber;
          this.salesorder.kilometrage = result.kilometrage;
          this.salesorder.vehicleReceiveFrom = result.vehicleReceiveFrom;
          this.salesorder.vehicleReceiveDate = result.vehicleReceiveDate;
          this.salesorder.notes = result.notes;
          this.salesorder.headTechnicianId = result.headTechnicianId;
          this.salesorder.discount = result.discount;
          this.salesorder.workorderTypeId = result.workorderTypeId;
          this.salesorder.customerId = result.customerId;
          this.salesorder.userVehicleId = result.userVehicleId;
          this.salesorder.inventoryStatus = result.inventoryStatus;
          this.salesorder.paymentStatus = result.paymentStatus;

          this.productServiceSourceArray = result.productLines;
          this.labourServiceSourceArray = result.serviceLines;
          this.fileSourceArray = result.files;
          this.setCustomerSearchSelection(result.userVehicleId);
          this.customerVehiclesBind(result.customerId);
          this.totalCalculation();

          this.uploadGrid.instance.refresh();
          this.serviceGrid.instance.refresh();
          this.productGrid.instance.refresh();

          //deleted vehicle add
          let deletedVehicle = this.customerVehicleArrayData.find(f => f.userVehicleId == result.userVehicleId);
          if (!deletedVehicle) {
            this.customerService.getCustomerVehicle(result.userVehicleId).toPromise().then((t) => {
              this.customerVehicleArrayData = this.customerVehicleArrayData.concat(t);
            });
          }
          //deleted headTechnicaian add
          let deletedHeadTechnician = this.headTechnitionArrayData.find(f => f.id == result.headTechnicianId);
          if (result.headTechnicianId && !deletedHeadTechnician) {
            this.employeeService.getUser(result.headTechnicianId).toPromise().then(t => {
              this.headTechnitionArrayData = this.headTechnitionArrayData.concat(t);
            });
          }
          //deleted technician add
          for (let serviceLine of result.serviceLines) {
            if (!this.technicianServiceArrayData.find(f => f.id == serviceLine.technicianId)) {
              this.employeeService.getUser(serviceLine.technicianId).toPromise().then(t => {
                this.technicianServiceArrayData = this.technicianServiceArrayData.concat(t);
              });
            }
          }
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

  statusDataBind() {
    this.soService.getStatus().toPromise().then((result) => {
      this.salesorderStatusData = result;
    });
  }

  inventoryStatusDataBind() {
    this.soService.getInventoryStatus().toPromise().then((result) => {
      this.salesOrderInventoryStatusData = result;
    });
  }

  paymentStatusDataBind() {
    this.soService.getPaymentStatus().toPromise().then((result) => {
      this.salesOrderPaymentStatusData = result;
    });
  }

  serviceBind() {
    this.productService.getServiceTypeItems().toPromise().then((t) => {
      this.serviceArrayData = this.serviceArrayData.concat(t);
    });
  }

  productBind() {
    this.productService.getStockUnstockTypeItems().toPromise().then((t) => {
      this.productArrayData = this.productArrayData.concat(t);
    });
  }

  headTechnicianBind() {
    this.employeeService.getHeadTechnicianUsers().toPromise().then(t => {
      this.headTechnitionArrayData = this.headTechnitionArrayData.concat(t);
    });
  }

  technicianBind() {
    this.employeeService.getTechnicianUsers().toPromise().then(t => {
      this.technicianServiceArrayData = this.technicianServiceArrayData.concat(t);
    }
    );
  }

  locationBind() {
    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  autoComplateDataBind() {
    this.customerDataSource =  new CustomStore({
        key: "ID",
        load: (loadOptions: any) => {
          return this.customerService.bySearchContent(loadOptions["searchValue"]).toPromise();
        },
        remove: function (key) {
          return this.customerService.bySearchContent(key).toPromise();
        }
      });
    
  }

  customerVehiclesBind(customerId: string) {
    this.customerService.getCustomerVehicles(customerId).toPromise().then((t) => {
      this.customerVehicleArrayData = this.customerVehicleArrayData.concat(t);
    });
  }

  workorderTypeBind() {
    this.workorderTypeData = new DataSource({
      store: new CustomStore({
        key: "ID",
        loadMode: "raw",
        load: () => {
          return this.soService.getWorkorderTypes().toPromise();
        }
      })
    });
  }

  //#endregion
}