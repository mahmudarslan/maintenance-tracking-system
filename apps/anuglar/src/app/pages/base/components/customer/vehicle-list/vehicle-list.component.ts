import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { HelperService, SharedService } from '@arslan/vms.base';
import { DxDataGridComponent } from 'devextreme-angular';
import { CustomerDto, CustomersService, VehicleBrand, VehicleBrandModel } from '../../../proxy/customer';
import { BaseStateService } from '../../../services';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import saveAs from 'file-saver';
import CustomStore from 'devextreme/data/custom_store';
import { VehicleService } from '../../../proxy/vehicle/vehicle.service';
import { CreateUpdateVehicleDto } from '@arslan/vms.base';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html'
})
export class VehicleListComponent implements OnInit {
  @ViewChild(DxDataGridComponent, { static: false }) dataGrid: DxDataGridComponent;
  dataSource: any;
  navigateUrl: string = "/base/customer/customer-new";
  roleData: any;
  allBrandModel: VehicleBrandModel[];
  brands: VehicleBrand[];
  brandDataSource: VehicleBrand[] = [];
  customerDataSource: CustomerDto[] = [];
  brandModelDataSource: VehicleBrandModel[] = [];
  @ViewChild('vehicleGrid') vehicleGrid: DxDataGridComponent;
  localization: LocalizationPipe;
  exportName = "";
  filterValueIsDeleted?: Boolean = false;
  vehicleDataStore: CustomStore;
  vehicleSourceArray: CreateUpdateVehicleDto[] = [];
  loadingVisible: Boolean = false;

  constructor(
    private router: Router,
    private helperService: HelperService,
    private vehicleService: VehicleService,
    private BaseStateService: BaseStateService,
    private localizationService: LocalizationService,
    private customerService: CustomersService,
    private http: HttpClient,
    private sharedService: SharedService) {

    this.dataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/base/vehicle", "id");
    this.filterValueIsDeleted = this.helperService.getStorageFilterValue("Storage_VehicleList", "isDeleted");

    this.sharedService.hideButton();

    this.editClick = this.editClick.bind(this);
    this.undoClick = this.undoClick.bind(this);

    this.localization = new LocalizationPipe(this.localizationService);

    this.exportName = this.localization.transform("Base::VehicleList");
  }

  ngOnInit(): void {
    this.bindBrand();
    this.bindCustomer();
    this.vehicleDataBind();

    this.getFilteredVehicleModels = this.getFilteredVehicleModels.bind(this);
    this.setModelValue = this.setModelValue.bind(this);

    this.allBrandModel = this.BaseStateService.getAllBrandModel();

    if (!this.allBrandModel?.length) {
      this.BaseStateService.dispatchGetAllBrandModel().subscribe(s => {
        this.allBrandModel = this.BaseStateService.getAllBrandModel();
        this.brandModelDataSource = this.allBrandModel;
        this.vehicleGrid.instance.refresh();
      });
    } else {
      this.brandModelDataSource = this.allBrandModel;
      //this.vehicleGrid.instance.refresh();
    }
  }

  bindCustomer() {
    this.customerService.getAll().subscribe(s => {
      this.customerDataSource = s;
    });
  }

  bindBrand() {
    this.brands = this.BaseStateService.getBrands();

    if (!this.brands?.length) {
      this.BaseStateService.dispatchGetBrands().subscribe(s => {
        this.brandDataSource = this.BaseStateService.getBrands();
        this.brands = this.BaseStateService.getBrands();
      });
    } else {
      this.brandDataSource = this.brands;
    }
  }

  vehicleDataBind() {
    this.vehicleDataStore = new CustomStore({
      key: 'userVehicleId',
      load: (loadOptions: any) => {
        return this.vehicleService.getList(loadOptions).toPromise();
      },
      insert: (values) => {
        let insertItem: CreateUpdateVehicleDto = {
          id: '00000000-0000-0000-0000-000000000000',
          plate: values.plate,
          color: values.color,
          motor: values.motor,
          chassis: values.chassis,
          modelId: values.modelId,
          brandId: values.brandId,
          customerId: values.customerId
        }
        return this.vehicleService.create(insertItem).toPromise();
      },
      update: (key, values) => {
        let updateItem: CreateUpdateVehicleDto = {
          id: values.id,
          modelId: values.modelId,
          customerId: values.customerId,
          userVehicleId: values.userVehicleId,
          plate: values.plate,
          color: values.color,
          motor: values.motor,
          chassis: values.chassis,
        }
        return this.vehicleService.update(key, updateItem).toPromise();
      },
      remove: (key) => {
        return this.vehicleService.delete(key).toPromise();
      }
    });
  }

  onRowUpdating(options) {
    options.newData = Object.assign(options.oldData, options.newData);
  }

  createClick = e => {
    this.router.navigate([this.navigateUrl]);
  }

  editClick(e) {
    this.router.navigateByUrl(this.navigateUrl + `?id=` + e.row.data.customerId + '&isDeleted=' + e.row.data.customerIsDeleted);
  }

  undoClick(e) {
    this.loadingVisible = true;

    this.vehicleService.undo(e.row.data.userVehicleId).subscribe(
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
            text: this.localization.transform("Base::ActiveRecords")
          }, {
            value: null,
            text: this.localization.transform("Base::Active&PassiveRecords")
          }, {
            value: true,
            text: this.localization.transform("Base::PassiveRecords")
          }],
          displayExpr: 'text',
          valueExpr: 'value',
          value: this.filterValueIsDeleted,
          onValueChanged: this.filterClick.bind(this)
        }
      },
      // {
      //   location: 'after',
      //   widget: 'dxButton',
      //   options: {
      //     icon: 'add',
      //     onClick: this.createClick.bind(this)
      //   }
      // },
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

  onInitializedGrid(e) {
    //this.dataGrid.instance.filter(["IsDeleted", "=", false]);
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
    if (e.row.data.isDeleted == null || e.row.isEditing) {
      return false;
    }

    if (e.row.data.isDeleted == false) {
      return true;
    }
    else {
      return false;
    }
  }

  isUndoIconVisible(e) {
    if (e.row.data.isDeleted == null || e.row.isEditing) {
      return false;
    }

    if (e.row.data.isDeleted == true) {
      return true;
    }
    else {
      return false;
    }
  }

  isUpdateIconVisible(e) {
    return !e.row.data.isDeleted;
  }

  setModelValue(rowData, value) {
    rowData.modelId = null;
    rowData.brandId = value;
    //(<any>this).defaultSetCellValue(rowData, value);
  }

  getFilteredVehicleModels(options) {
    return {
      store: this.brandModelDataSource,
      filter: options.data ? ["brandId", "=", options.data.brandId] : null
    };
  }

}