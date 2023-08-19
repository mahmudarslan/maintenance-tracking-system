import { Component, OnInit, ViewChild, Pipe, PipeTransform } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { HelperService, SharedService } from '@arslan/vms.base';
import { DxDataGridComponent } from 'devextreme-angular';
import { CategoryDto, ProductService } from '../../../base/proxy/product';
import { AdjustStockService } from '../../proxy/adjust-stock';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';
import { CategoryService } from '../../../base/proxy/category/category.service';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-current-stock',
  templateUrl: './current-stock.component.html',
})
export class CurrentStockComponent implements OnInit {
  customersData: any;
  dataSource: any;
  url: string;
  productData: any;
  locationArrayData: LocationDto[] = [];
  categoryArrayData: CategoryDto[] = [];
  @ViewChild('stockGrid') dataGrid: DxDataGridComponent;
  localization: LocalizationPipe;
  exportName = "";

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private adjustStockService: AdjustStockService,
    private helperService: HelperService,
    private localizationService: LocalizationService,
    private locationService: LocationService,
    private sharedService: SharedService) {

    this.dataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/inventory/stockadjustment/CurrentStocks", "id");

    this.productData = {
      store: new CustomStore({
        key: "id",
        loadMode: "raw",
        load: () => {
          return this.productService.getProductTypes().toPromise();
        }
      })
    };

    this.sharedService.hideButton();

    this.localization = new LocalizationPipe(this.localizationService);

    this.exportName = this.localization.transform("Inventory::CurrentStockList");
  }

  ngOnInit(): void {
    this.locationBind();
    this.categorynBind();
  }

  locationBind() {
    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  categorynBind() {
    this.categoryService.getAll().toPromise().then((t) => {
      this.categoryArrayData = this.categoryArrayData.concat(t);
    });
  }

  refreshClick = e => {
    this.dataGrid.instance.refresh();
  }

  onToolbarPreparing(e) {
    e.toolbarOptions.items.unshift({
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

}

@Pipe({ name: 'gridCellData' })
export class GridCellDataPipe implements PipeTransform {
  transform(gridData: any) {
    return gridData.data[gridData.column.caption.toLowerCase()];
  }
}