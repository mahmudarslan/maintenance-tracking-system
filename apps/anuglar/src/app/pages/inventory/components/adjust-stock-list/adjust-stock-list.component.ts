import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, ViewChild, } from '@angular/core';
import { Router } from '@angular/router';
import { HelperService, SharedService } from '@arslan/vms.base';
import { DxDataGridComponent } from 'devextreme-angular';
import Query from "devextreme/data/query";
import { AdjustStockService } from '../../proxy/adjust-stock';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';
import CustomStore from 'devextreme/data/custom_store';

@Component({
  selector: 'app-adjust-stock-list',
  templateUrl: './adjust-stock-list.component.html'
})
export class AdjustStockListComponent implements OnInit {

  customersData: any;
  categoryData: any;
  dataStore: CustomStore;
  url: string;
  productData: any;
  @ViewChild('adjustStockGrid') dataGrid: DxDataGridComponent;
  localization: LocalizationPipe;
  exportName = "";
  loadingVisible: Boolean = false;
  filterValueIsDeleted?: Boolean = false;
  isModalVisible: boolean;

  constructor(
    private router: Router,
    private helperService: HelperService,
    private sharedService: SharedService,
    private adjustStockService: AdjustStockService,
    private localizationService: LocalizationService,) {


    this.dataStore = this.helperService.gridDatasourceBind("rest/api/latest/vms/inventory/stockadjustment", "id");
    this.filterValueIsDeleted = this.helperService.getStorageFilterValue("Storage_AdjustStockList", "isDeleted");

    this.sharedService.hideButton();

    this.editClick = this.editClick.bind(this);

    this.undoClick = this.undoClick.bind(this);

    this.localization = new LocalizationPipe(this.localizationService);

    this.exportName = this.localization.transform("Inventory::AdjustStockList");
  }

  ngOnInit(): void {

  }

  createClick(e) {
    // this.router.navigate(['/inventory/adjust-stock-new']);
    this.isModalVisible = true;
  }

  editClick(e) {
    this.router.navigateByUrl(`/inventory/adjust-stock-new?id=` + e.row.data.id + '&isDeleted=' + e.row.data.isDeleted);
  }

  undoClick(e) {
    this.loadingVisible = true;

    this.adjustStockService.undo(e.row.data.id).subscribe(
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

  refreshClick(e) {
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