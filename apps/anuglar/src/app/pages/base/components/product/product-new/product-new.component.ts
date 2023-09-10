import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DxDataGridComponent, DxFileUploaderComponent, DxFormComponent, DxSelectBoxComponent } from 'devextreme-angular';
import DataSource from 'devextreme/data/data_source';
import CustomStore from 'devextreme/data/custom_store';
import validationEngine from 'devextreme/ui/validation_engine';
import { FileAttachmentDto, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { CreateUpdateProductDto, ProductService, stockAdjustmentLine } from '../../../proxy/product';
import { AdjustStockService } from '@arslan/vms.inventory/proxy/adjust-stock';
import { BehaviorSubject } from 'rxjs';
import { ApiInterceptor } from '@abp/ng.core';
import { environment } from 'src/environments/environment';
import { Guid } from 'guid-typescript';
import { CategoryService } from '../../../proxy/category/category.service';
import { SecurePipe } from '@arslan/vms.base';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-product-new',
  templateUrl: './product-new.component.html',
  styleUrls: ['./product-new.component.scss']
})
export class ProductNewComponent implements OnInit {

  @ViewChild('productTypeBox') productTypeBox: DxSelectBoxComponent;
  @ViewChild('productCategoryBox') productCategoryBox: DxSelectBoxComponent;
  @ViewChild('fileUploader') fileUploader: DxFileUploaderComponent;
  @ViewChild('newStockGrid') newStockGrid: DxDataGridComponent;
  @ViewChild('form') form: DxFormComponent;
  product: CreateUpdateProductDto = new CreateUpdateProductDto();
  selectedId: any;
  isEditMode = false;
  isProductTypeVisible: boolean = true;
  loadingVisible = false;
  sortOrderCreatedDate: string;
  sortOrderUpdatedDate: string;
  sharedButton: SharedButtonModel = new SharedButtonModel();
  imageSource = '';
  value: any[] = [];
  uploadHeaders: any;
  uploadFakeId = 0;
  url: string = environment.apis.default.url + 'rest/api/latest/vms/';
  uploadUrl = this.url + 'core/file/upload';
  downloadUrl = '';
  fakeId: any;
  productData: any;
  productTypeSource: any;
  categoriesSource: any;
  fileSourceArray: FileAttachmentDto[] = [];
  locationArrayData: LocationDto[] = [];
  stockAdjustmentLineDataSource: CustomStore;
  isStockGridValidate?: boolean = null

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private adjustStockService: AdjustStockService,
    private sharedService: SharedService,
    private apiInterceptor: ApiInterceptor,
    private locationService: LocationService,
    private routeParams: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.isProductTypeVisible = false;

    this.stockStoreBind();
    this.locationDataBind();
    this.productTypeDataBind();
    this.categoriesDataBind();
    this.productDataBind();
    this.queryParameterBind();
    this.actionButtonBind();
    this.setUploadHeader();
    this.addIdParameter = this.addIdParameter.bind(this);
  }


  Save() {
    if (this.isProductTypeVisible) {
      this.saveStock().then(t => {
        if (t != false) {
          this.crudAction();
        } else {
          this.sharedService.buttonComplate(this.sharedButton);
        }
      });

    } else {
      this.crudAction();
    }
  }

  crudAction() {
    let productItem = this.getFormData();

    if (this.isEditMode) {
      this.update(productItem);
    }
    else {
      this.create(productItem);
    }
  }

  create(productItem) {
    this.productService.create(productItem).subscribe(s => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        'success');
      this.clearForm();
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, 'error');
      },
    );
  }

  update(productItem) {
    this.productService.update(productItem, this.selectedId).subscribe(s => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        'success',
        '/base/product/product-list');
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, 'error');
      },
    );

  }


  saveStock(): Promise<boolean> {
    this.isStockGridValidate = null;
    return this.newStockGrid.instance.saveEditData().then(t => {

      if (this.isStockGridValidate) {
        this.product.stockAdjustmentLines.forEach(element => {
          if (element.id.length < 5) {
            element.id = '00000000-0000-0000-0000-000000000000';
          }
        });
      }
      return this.isStockGridValidate;
    });
  }

  getFormData(): CreateUpdateProductDto {
    if (this.product.prices.length > 0) {
      this.product.prices[0].unitPrice = this.product.defaultPrice;
    }

    let productItem = new CreateUpdateProductDto();
    productItem.name = this.product.name;
    productItem.productType = this.product.productType;
    productItem.barcode = this.product.barcode;
    productItem.cost = this.product.cost;
    productItem.defaultStockCount = this.product.defaultStockCount;
    productItem.defaultPrice = this.product.defaultPrice;
    productItem.reorderPoint = this.product.reorderPoint;
    productItem.prices = this.product.prices;
    productItem.stockAdjustmentLines = this.product.stockAdjustmentLines;
    productItem.defaultLocationId = this.product.defaultLocationId;
    productItem.productCategoryId = this.product.productCategoryId;
    productItem.fakeId = this.fakeId;

    return productItem;
  }

  setProductTypeVisible(value) {
    if (value == 1) {
      this.isProductTypeVisible = true;
    } else {
      this.isProductTypeVisible = false;
    }
  }

  onProductTypeChanged(e) {
    if (this.isEditMode) {
      return;
    }
    this.product.productType = e.value;
    this.setProductTypeVisible(e.value);
  }

  clearForm() {
    setTimeout(() => {
      this.isStockGridValidate = false;
      this.product = new CreateUpdateProductDto();
      this.productTypeBox.readOnly = false;
      this.sortOrderCreatedDate = 'desc';
      this.uploadFakeId = 0;
      this.fileSourceArray = [];
      this.setUploadHeader();
      this.fileUploader.instance.reset();
      this.imageSource = '';
      this.downloadUrl = '';
      validationEngine.resetGroup();
      this.stockStoreBind();
    }, 100);
  }




  //#region Stock Grid

  stockStoreBind() {
    this.stockAdjustmentLineDataSource = new CustomStore({
      key: 'id',
      load: () => {
        return this.product.stockAdjustmentLines
      },
      insert: (values) => {
        let length = this.product.stockAdjustmentLines.length;
        let insertItem: stockAdjustmentLine = {
          id: (length++).toString(),
          locationId: values.locationId,
          quantityAfter: values.quantityAfter,
        }
        this.product.stockAdjustmentLines.push(insertItem);
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.product.stockAdjustmentLines }).then();
      },
      update: (key, values) => {
        let itemIndex = this.product.stockAdjustmentLines.findIndex(t => t.id == key);
        if (values.locationId != undefined) { this.product.stockAdjustmentLines[itemIndex].locationId = values.locationId; }
        if (values.quantityAfter != undefined) { this.product.stockAdjustmentLines[itemIndex].quantityAfter = values.quantityAfter; }
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.product.stockAdjustmentLines }).then();
      },
      remove: (key) => {
        let itemIndex = this.product.stockAdjustmentLines.find(t => t.id == key);
        itemIndex.isDeleted = true;

        if (itemIndex.id.length < 5) {
          let itemIndex1 = this.product.stockAdjustmentLines.findIndex(t => t.id == key);
          this.product.stockAdjustmentLines.splice(itemIndex1, 1);
        }
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.product.stockAdjustmentLines }).then();
      }
    });
  }

  onRowValidating(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(', ');
    e.errorText = errorText;
    this.isStockGridValidate = e.isValid;
  }

  refreshClick = e => {
    this.newStockGrid.instance.refresh();
  }

  onToolbarPreparing(e) {
    let toolbarItems = e.toolbarOptions.items;
    // Modifies an existing item
    toolbarItems.forEach(function (item) {
      if (item.name === 'saveButton') {
        item.visible = false;
      }
    });

    e.toolbarOptions.items.unshift(
      {
        location: 'after',
        widget: 'dxButton',
        options: {
          icon: 'refresh',
          onClick: this.refreshClick.bind(this)
        }
      }
    );
  }

  onEditorPreparing(e) {
    if (e.dataField === 'productId' && e.parentType === 'dataRow') {
      const defaultValueChangeHandler = e.editorOptions.onValueChanged;
    }
  }

  //#endregion

  //#region Upload

  updateQueryStringParameter(uri, key, value) {
    var re = new RegExp('([?&])' + key + '=.*?(&|$)', 'i');
    var separator = uri.indexOf('?') !== -1 ? '&' : '?';
    if (uri.match(re)) {
      return uri.replace(re, '$1' + key + '=' + value + '$2');
    }
    else {
      return uri + separator + key + '=' + value;
    }
  }

  setUploadHeader() {
    this.fakeId = (Guid.create()).toString();
    this.uploadHeaders = this.apiInterceptor.getAdditionalHeaders();
  }

  addIdParameter(e) {
    this.uploadUrl = this.updateQueryStringParameter(this.uploadUrl, 'id', this.fakeId);
    e.component.option('uploadUrl', this.uploadUrl);

    e.value.forEach((value) => {
      let isPreValue = false;

      if (e.previousValue.length > 0) {
        e.previousValue.forEach(function (value1) {
          if (value1.name == value.name) { isPreValue = true; }
        });
      }

      if (!isPreValue) {
        this.uploadFakeId += 1;

        const file = e.value[0];
        const fileReader = new FileReader();
        fileReader.onload = () => {

          this.imageSource = fileReader.result as string;
          this.fileSourceArray.push(
            {
              id: this.uploadFakeId.toString(),
              downloadUrl: '/',
              fileName: value.name,
              isDeleted: false,
              source: fileReader.result as string
            });
        }
        fileReader.readAsDataURL(file);
      }
    });
  }

  //#endregion

  //#region  Bind

  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedId = params.get('id');
      let parameterIsDeleted = params.get('isDeleted') == 'true' ? true : false;

      if (this.selectedId !== undefined && this.selectedId != null) {
        this.loadingVisible = true;
        this.isEditMode = true;
        this.sortOrderUpdatedDate = 'desc';
        this.formDataBind(this.selectedId, parameterIsDeleted);
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      } else {
        this.isEditMode = false;
        this.clearForm();
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      }
    })
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

    this.productService.get(id, isDeleted).subscribe(
      {
        next: (result) => {
          this.product.name = result.name;
          this.product.productType = result.productType;
          this.product.barcode = result.barcode;
          this.product.cost = result.cost;
          this.product.defaultPrice = result.defaultPrice;
          this.product.defaultStockCount = result.defaultStockCount;
          this.product.reorderPoint = result.reorderPoint;
          this.product.prices = result.prices;
          this.product.stockAdjustmentLines = result.stockAdjustmentLines;
          this.product.defaultLocationId = result.defaultLocationId;
          this.product.productCategoryId = result.productCategoryId;
          this.fileSourceArray = result.files ?? new Array<FileAttachmentDto>();
          if (result.files.length > 0) {
            this.downloadUrl = 'rest/api/latest/vms/base/file/download?id=' + result.files[0].id;
          }

          this.productTypeBox.readOnly = true;

          this.setProductTypeVisible(result.productType);

          this.stockStoreBind();

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
  };

  productTypeDataBind() {
    this.productTypeSource = new DataSource({
      store: new CustomStore({
        key: 'ID',
        loadMode: 'raw',
        load: () => { return this.productService.getProductTypes().toPromise(); }
      })
    });
  }

  categoriesDataBind() {
    this.categoriesSource = new DataSource({
      store: new CustomStore({
        key: 'ID',
        loadMode: 'raw',
        load: () => { return this.categoryService.getAll().toPromise(); }
      })
    });
  }

  productDataBind() {
    this.productData = {
      store: new CustomStore({
        key: 'id',
        loadMode: 'raw',
        load: () => {
          return this.productService.getProductTypes().toPromise();
        }
      })
    };
  }

  locationDataBind() {
    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  //#endregion
}
