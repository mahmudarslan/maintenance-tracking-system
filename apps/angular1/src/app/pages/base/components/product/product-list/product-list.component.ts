import { Component, ChangeDetectionStrategy, OnInit, ViewChild, Pipe, PipeTransform } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { Router } from '@angular/router';
import { HelperService, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { DxDataGridComponent } from 'devextreme-angular';
import { ProductService } from '../../../proxy/product';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';
import { CategoryService } from '../../../proxy/category/category.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {

  customersData: any;
  categoryData: any;
  dataStore: CustomStore;
  url: string;
  productData: any;
  @ViewChild('productGrid') dataGrid: DxDataGridComponent;
  localization: LocalizationPipe;
  exportName = "";
  loadingVisible: Boolean = false;
  filterValueIsDeleted?: Boolean = false;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private localizationService: LocalizationService,
    private sharedService: SharedService,
    private router: Router,
    private helperService: HelperService) {

    this.dataStore = this.helperService.gridDatasourceBind("rest/api/latest/vms/base/product", "id");
    this.filterValueIsDeleted = this.helperService.getStorageFilterValue("Storage_Productist", "isDeleted");

    this.productData = {
      store: new CustomStore({
        key: "id",
        loadMode: "raw",
        load: () => {
          return this.productService.getProductTypes().toPromise();
        }
      })
    };

    this.categoryData = {
      store: new CustomStore({
        key: "ID",
        loadMode: "raw",
        load: () => { return this.categoryService.getAll().toPromise(); }
      })
    };

    this.sharedService.hideButton();

    this.editClick = this.editClick.bind(this);
    this.undoClick = this.undoClick.bind(this);


    this.localization = new LocalizationPipe(this.localizationService);

    this.exportName = this.localization.transform("Inventory::ProductList");
  }

  ngOnInit(): void {
  }

  createClick = e => {
    this.router.navigate(['/inventory/product/product-new']);
  }

  editClick(e) {
    this.router.navigateByUrl(`/inventory/product/product-new?id=` + e.row.data.id + '&isDeleted=' + e.row.data.isDeleted);
  }

  undoClick(e) {
    this.loadingVisible = true;

    this.productService.undo(e.row.data.id).subscribe(
      {
        next: (n) => {
          this.dataGrid.instance.refresh();
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

  refreshClick = e => {
    this.dataGrid.instance.refresh();
  }

  filterClick(e) {
    this.filterValueIsDeleted = e.value;
  }

  onToolbarPreparing(e) {
    e.toolbarOptions.items.unshift(
      {
        location: 'before',
        widget: 'dxSelectBox',
        options: {
          width: 200,
          items: [{
            value: false,
            text: this.localization.transform("Inventory::ActiveRecords")
          }, {
            value: null,
            text: this.localization.transform("Inventory::Active&PassiveRecords")
          }, {
            value: true,
            text: this.localization.transform("Inventory::PassiveRecords")
          }],
          displayExpr: 'text',
          valueExpr: 'value',
          value: this.filterValueIsDeleted,
          onValueChanged: this.filterClick.bind(this)
        }
      },
      {
        location: 'after',
        widget: 'dxButton',
        options: {
          icon: 'add',
          onClick: this.createClick.bind(this)
        }
      }, {
      location: 'after',
      widget: 'dxButton',
      options: {
        icon: 'refresh',
        onClick: this.refreshClick.bind(this)
      }
    }
    );
  }

  onExporting(e, key) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Main sheet');
    exportDataGrid({
      component: e.component,
      worksheet: worksheet,
      customizeCell: function (options) {
        const excelCell = options;
        //excelCell.font = { name: 'Arial', size: 12 };
        //excelCell.alignment = { horizontal: 'left' };
      }
    }).then(function () {
      workbook.xlsx.writeBuffer()
        .then(function (buffer: BlobPart) {
          saveAs(new Blob([buffer], { type: 'application/octet-stream' }), (key + '.xlsx'));
        });
    });
    e.cancel = true;
  }

  onRowRemoved(e) {
    this.dataGrid.instance.refresh();
  }

  isDeleteIconVisible(e) {
    return !e.row.data.isDeleted;
  }

  isUndoIconVisible(e) {
    return e.row.data.isDeleted;
  }

}