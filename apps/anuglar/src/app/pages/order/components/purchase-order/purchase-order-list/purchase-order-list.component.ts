import { Component, ChangeDetectionStrategy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import notify from 'devextreme/ui/notify';
import { HelperService, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { SalesOrderService } from '../../../proxy/order';
import { DxDataGridComponent } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import { EmployeeService, BaseStateService, VehicleBrandModel, VehicleTypeService } from '@arslan/vms.base';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { PurchaseOrderService } from '../../../proxy/purchase';
import { AdjustStockService } from '@arslan/vms.inventory/proxy/adjust-stock';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';
import { LocationDto, LocationService } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-purchase-order-list',
  templateUrl: './purchase-order-list.component.html'
})
export class PurchaseOrderListComponent implements OnInit {

  @ViewChild('purchaseorderGrid') dataGrid: DxDataGridComponent;
  dataSource: any;
  addPhoneButtonOptions: any;
  items: any;
  url: string = environment.apis.default.url + "rest/api/latest/vms/orders/purchase/";
  downloadUrl: string = this.url + "/download";
  stateData: any;
  vehicleBrandData: any;
  sharedButton: SharedButtonModel = new SharedButtonModel();
  allBrandModel: VehicleBrandModel[];
  brandModelDataSource: VehicleBrandModel[] = [];
  loadingVisible: Boolean = false;
  localization: LocalizationPipe;
  purchaseOrderInventoryStatusData: any;
  purchaseOrderPaymentStatusData: any;
  locationArrayData: LocationDto[] = [];
  exportName = "";
  filterValueIsDeleted?: Boolean = false;
  isModalVisible: boolean;

  constructor(
    private router: Router,
    private helperService: HelperService,
    private sharedService: SharedService,
    private poService: PurchaseOrderService,
    private localizationService: LocalizationService,
    private BaseStateService: BaseStateService,
    private locationService: LocationService,
    private adjustStockService: AdjustStockService,
    private vehicleService: VehicleTypeService) {

    this.dataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/orders/purchase", "id", "");
    this.filterValueIsDeleted = this.helperService.getStorageFilterValue("Storage_PurchaseOrderList", "isDeleted");

    this.sharedService.hideButton();

    this.editClick = this.editClick.bind(this);
    this.downloadClick = this.downloadClick.bind(this);

    this.undoClick = this.undoClick;

    this.localization = new LocalizationPipe(this.localizationService);

    this.exportName = this.localization.transform("Orders::PurchaseOrderList");
  }

  ngOnInit(): void {
    this.poService.getInventoryStatus().toPromise().then((result) => {
      this.purchaseOrderInventoryStatusData = result;
    });

    this.poService.getPaymentStatus().toPromise().then((result) => {
      this.purchaseOrderPaymentStatusData = result;
    });

    this.locationService.getLocations().toPromise().then((t) => {
      this.locationArrayData = this.locationArrayData.concat(t);
    });
  }

  createClick = e => {
    // this.router.navigate(['/order/purchase-order/purchase-order-new']);
    this.isModalVisible = true;
  }

  editClick(e) {
    this.router.navigateByUrl(`/order/purchase-order/purchase-order-new?id=` + e.row.data.id + '&isDeleted=' + e.row.data.isDeleted);
  }

  undoClick(e) {
    this.loadingVisible = true;

    this.poService.undo(e.row.data.id).subscribe(
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

  downloadClick(e) {
    this.loadingVisible = true;

    this.helperService.download(e.row.data.orderNumber).subscribe(s => {
      this.loadingVisible = false;
    });
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
            text: this.localization.transform("Orders::ActiveRecords")
          }, {
            value: null,
            text: this.localization.transform("Orders::Active&PassiveRecords")
          }, {
            value: true,
            text: this.localization.transform("Orders::PassiveRecords")
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
    });
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
    notify(e.data.orderNumber + " Workorder deleted", "success", 3000);
  }

  isDeleteIconVisible(e) {
    return !e.row.data.isDeleted;
  }

  isUndoIconVisible(e) {
    return e.row.data.isDeleted;
  }

  saveDetail(e) {
    // this.taskDetailComponent.save().subscribe(s => {
    //   if (s) {
    //     this.isModalVisible = false;
    //     this.crudGrid.instance.refresh();
    //   }
    // });
  }

  onClick_Cancel(e) {
    this.isModalVisible = false;
  }

}