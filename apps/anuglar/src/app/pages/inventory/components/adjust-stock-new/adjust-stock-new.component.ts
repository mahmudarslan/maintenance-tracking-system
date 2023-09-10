import { Component, OnInit, ViewChild } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { ActivatedRoute } from '@angular/router';
import { DxDataGridComponent, DxFormComponent } from 'devextreme-angular';
import { environment } from 'src/environments/environment';
import validationEngine from 'devextreme/ui/validation_engine';
import { ProductDto, ProductService } from '../../../base/proxy/product';
import {
  AdjustStockService,
  CreateUpdateStockAdjustmentDto,
  CreateUpdateStockAdjustmentLine
} from '../../proxy/adjust-stock';
import { ProductStockItem } from '../../models/inventory';
import { SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject } from 'rxjs';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-adjust-stock-new',
  templateUrl: './adjust-stock-new.component.html'
})
export class AdjustStockNewComponent implements OnInit {

  @ViewChild('form') form: DxFormComponent;
  @ViewChild('newStockGrid') newStockGrid: DxDataGridComponent;
  url: string = environment.apis.default.url + "rest/api/latest/vms/";
  stockAdjustmentLineDataStore: CustomStore;
  productArrayData: ProductDto[] = [];
  locationArrayData: LocationDto[] = [];
  stockAdjustment: CreateUpdateStockAdjustmentDto = new CreateUpdateStockAdjustmentDto();
  stockAdjustmentLines: CreateUpdateStockAdjustmentLine[] = [];
  productStockItems: ProductStockItem[] = [];
  fakeId: number;
  isEditMode: boolean = false;
  parameterId: any;
  result: CreateUpdateStockAdjustmentDto;
  value: any[] = [];
  sharedButton: SharedButtonModel = new SharedButtonModel();
  loadingVisible: Boolean = false;
  isStockGridValidate?: boolean = null

  constructor(
    private adjustStockService: AdjustStockService,
    private productService: ProductService,
    private sharedService: SharedService,
    private locationService: LocationService,
    private routeParams: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.fakeId = 0;
    this.stockStoreBind();
    this.productDataBind();
    this.locationDataBind();
    this.queryParameterBind();
    this.actionButtonBind();

    this.setNewQuantityCellValue = this.setNewQuantityCellValue.bind(this);
    this.setDifferenceCellValue = this.setDifferenceCellValue.bind(this);
    this.overrideOnValueChanged = this.overrideOnValueChanged.bind(this);
  }

  Save() {
    this.isStockGridValidate = null;

    this.newStockGrid.instance.saveEditData().then(t => {
      if (this.isStockGridValidate != false) {
        this.crudAction();
      } else {
        this.sharedService.buttonComplate(this.sharedButton);
      }
    });
  }

  crudAction() {
    let productItem = this.getFormData();

    if (this.isEditMode) {
      this.update(this.parameterId, productItem);
    }
    else {
      this.create(productItem);
    }
  }

  create(stockAdjustment: CreateUpdateStockAdjustmentDto) {
    this.adjustStockService.create(stockAdjustment).subscribe(
      {
        next: (n) => {
          this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
            'Base::TransactionSuccesfullySaved',
            "success");
          this.clearForm();
        },
        error: (e) => { this.sharedService.buttonComplateWithNotify(this.sharedButton, e, "error"); },
        complete: () => { }
      }
    );
  }

  update(id: string, stockAdjustment: CreateUpdateStockAdjustmentDto) {
    this.adjustStockService.update(id, stockAdjustment).subscribe(
      {
        next: (n) => {
          this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
            'Base::TransactionSuccesfullySaved',
            "success",
            '/inventory/adjust-stock-list/');
        },
        error: (e) => { this.sharedService.buttonComplateWithNotify(this.sharedButton, e, "error"); },
        complete: () => { }
      }
    );
  }

  getFormData(): CreateUpdateStockAdjustmentDto {
    this.stockAdjustment.lines = [];
    this.stockAdjustment.lines = this.stockAdjustmentLines;

    this.stockAdjustment.lines.forEach(element => {
      if (element.id.length < 5) {
        element.id = '00000000-0000-0000-0000-000000000000';
      }
    });
    return this.stockAdjustment;
  }

  clearForm() {
    this.stockAdjustment = new CreateUpdateStockAdjustmentDto();
    this.stockAdjustmentLines = [];
    setTimeout(() => {
      this.newStockGrid.instance.refresh();
      this.newStockGrid.instance.cancelEditData();
    }, 100);

    validationEngine.resetGroup();
  }


  //#region Stock Grid

  stockStoreBind() {
    this.stockAdjustmentLineDataStore = new CustomStore({
      key: 'id',
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
          quantityBefore: values.quantityBefore,
          difference: values.difference
        }
        this.stockAdjustmentLines.push(insertItem);
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.stockAdjustmentLines }).then();
      },
      update: (key, values) => {
        let itemIndex = this.stockAdjustmentLines.findIndex(t => t.id == key);
        if (values.productId != undefined) { this.stockAdjustmentLines[itemIndex].productId = values.productId; }
        if (values.locationId != undefined) { this.stockAdjustmentLines[itemIndex].locationId = values.locationId; }
        if (values.quantityAfter != undefined) { this.stockAdjustmentLines[itemIndex].quantityAfter = values.quantityAfter; }
        if (values.difference != undefined) { this.stockAdjustmentLines[itemIndex].difference = values.difference; }
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.stockAdjustmentLines }).then();
      },
      remove: (key) => {
        let itemIndex = this.stockAdjustmentLines.find(t => t.id == key);
        itemIndex.isDeleted = true;
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.stockAdjustmentLines }).then();
      }
    });
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

  overrideOnValueChanged(e) {
    if (e.dataField === 'productId' && e.parentType === 'dataRow') {
      const defaultValueChangeHandler = e.editorOptions.onValueChanged;
      e.editorOptions.onValueChanged = (args) => {
        this.adjustStockService.getCurrentStockWithLocation(args.value, this.locationArrayData[0].id).toPromise().then((t) => {

          if (t) {
            let items = this.productStockItems;
            let itemIndex = items.findIndex(item => item.id == t.id && item.fakeId == t.fakeId);

            if (itemIndex < 0) {
              let fakeId = ++this.fakeId;

              let aa = new ProductStockItem();
              aa.fakeId = fakeId;
              aa.id = t.id;
              aa.quantityBefore = t.quantity;
              items.push(aa);

              let products = items.filter(item => item.id == t.id);

              let sumDifference = products.reduce((a, b) => a + (b.difference || 0), 0);

              e.row.data.quantityBefore = t.quantity + sumDifference;
              e.row.data.fakeId = fakeId;
            } else {
              let item = items[itemIndex];

              e.row.data.quantityBefore = t.quantity + item.difference;
            }

          } else {
            e.row.data.quantityBefore = 0;
          }

          if (!e.row.data.locationId) {
            e.row.data.locationId = this.locationArrayData[0].id;
          }
          this.newStockGrid.instance.refresh();

          defaultValueChangeHandler(args);
        });
      }
    }

    if (e.dataField === 'locationId' && e.parentType === 'dataRow') {
      const defaultValueChangeHandler = e.editorOptions.onValueChanged;
      e.editorOptions.onValueChanged = (args) => {
        this.adjustStockService.getCurrentStockWithLocation(e.row.data.productId, args.value).toPromise().then((t) => {

          if (t) {
            e.row.data.quantityBefore = t.quantity;
          } else {
            e.row.data.quantityBefore = 0;
            e.row.data.quantityAfter = 0;
            e.row.data.difference = 0;
          }
          this.newStockGrid.instance.refresh();
          defaultValueChangeHandler(args);
        });
      }
    }
  }

  setNewQuantityCellValue(rowData, value, currentData) {
    rowData[(<any>this).dataField] = value;

    rowData.difference = value - currentData.quantityBefore;

    let items = this.productStockItems;
    let itemIndex = items.findIndex(item => item.id == currentData.productId && item.fakeId == currentData.fakeId);

    if (itemIndex > -1) {
      let item = items[itemIndex];
      item.quantityAfter = value;
      item.difference = value - currentData.quantityBefore;
      items[itemIndex] = item;
    }
  }

  setDifferenceCellValue(rowData, value, currentData) {
    rowData[(<any>this).dataField] = value;

    rowData.quantityAfter = currentData.quantityBefore + value;

    let items = this.productStockItems;
    let itemIndex = items.findIndex(item => item.id == currentData.productId && item.fakeId == currentData.fakeId);

    if (itemIndex > -1) {
      let item = items[itemIndex];
      item.quantityAfter = currentData.quantityBefore + value;
      item.difference = value;
      items[itemIndex] = item;
    }
  }

  onRowValidating(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;
    this.isStockGridValidate = e.isValid;
  }

  //#endregion

  //#region  Bind

  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.parameterId = params.get("id");
      let parameterIsDeleted = params.get("isDeleted") == "true" ? true : false;

      if (this.parameterId != null && this.parameterId.length > 0) {
        this.isEditMode = true;
        this.formDataBind(this.parameterId, parameterIsDeleted);
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      } else {
        this.isEditMode = false;
        this.clearForm();
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
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

    this.adjustStockService.get(id, isDeleted).subscribe(
      {
        next: result => {
          this.result = result;
          this.stockAdjustment.creationTime = result.creationTime;
          this.stockAdjustment.adjustmentNumber = result.adjustmentNumber;
          this.stockAdjustment.remarks = result.remarks;
          this.stockAdjustmentLines = result.lines;
          this.newStockGrid.instance.refresh();
          this.stockAdjustment.status = result.status;

          if (result.isDeleted) {
            this.form.disabled = true;
            this.sharedService.hideButton();
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

  productDataBind() {
    this.productService.getStockTypeItems().toPromise().then((t) => {
      this.productArrayData = this.productArrayData.concat(t);
    });
  }

  locationDataBind() {
    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  //#endregion

}