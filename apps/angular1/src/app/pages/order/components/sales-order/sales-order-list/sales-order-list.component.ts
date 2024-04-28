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
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';

@Component({
  selector: 'app-sales-order-list',
  templateUrl: './sales-order-list.component.html',
  styleUrls: ['./sales-order-list.component.scss'],
})
export class SalesOrderListComponent implements OnInit {

  dataSource: any;
  items: any;
  url: string = environment.apis.default.url + "rest/api/latest/vms/orders/sales/";
  downloadUrl: string = this.url + "/download";
  stateData: any;
  sharedButton: SharedButtonModel = new SharedButtonModel();
  @ViewChild('salesorderGrid') dataGrid: DxDataGridComponent;
  loadingVisible: Boolean = false;
  localization: LocalizationPipe;
  exportName = "";
  filterValueIsDeleted?: Boolean = false;
  salesOrderInventoryStatusData: any;
  salesOrderPaymentStatusData: any;
  isModalVisible: boolean;

  constructor(
    private router: Router,
    private helperService: HelperService,
    private sharedService: SharedService,
    private soService: SalesOrderService,
    private localizationService: LocalizationService,
    private BaseStateService: BaseStateService,
    private vehicleService: VehicleTypeService) {

    this.dataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/orders/sales", "id", "");
    this.filterValueIsDeleted = this.helperService.getStorageFilterValue("Storage_SalesOrderList", "isDeleted");

    this.items = [
      { text: 'Download' },
      { text: 'Güncelle' }
    ];

    this.stateData = {
      store: new CustomStore({
        key: "id",
        loadMode: "raw",
        load: () => {
          return this.soService.getStatus().toPromise();
        }
      })
    };

    this.sharedService.hideButton();

    this.editClick = this.editClick.bind(this);
    this.downloadClick = this.downloadClick.bind(this);
    this.undoClick = this.undoClick.bind(this)

    this.localization = new LocalizationPipe(this.localizationService);

    this.exportName = this.localization.transform("Orders::SalesOrderList");
  }

  ngOnInit(): void {
    this.soService.getInventoryStatus().toPromise().then((result) => {
      this.salesOrderInventoryStatusData = result;
    });

    this.soService.getPaymentStatus().toPromise().then((result) => {
      this.salesOrderPaymentStatusData = result;
    });
  }

  createClick = e => {
    // this.router.navigate(['/order/sales-order/sales-order-new']);
    this.isModalVisible = true;
  }

  editClick(e) {
    this.router.navigateByUrl('/order/sales-order/sales-order-new?id=' + e.row.data.id + '&isDeleted=' + e.row.data.isDeleted);
  }

  undoClick(e) {
    this.loadingVisible = true;

    this.soService.undo(e.row.data.id).subscribe(
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

    this.helperService.download(e.row.data.orderNumber).subscribe({
      next: (n) => {
        this.loadingVisible = false;
      },
      error: (e) => {
        this.loadingVisible = false;
        console.log(e);
      },
      complete: () => {
        this.loadingVisible = false;
      }
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