import { Component, Input, OnInit, ViewChild } from '@angular/core';
import CustomStore from "devextreme/data/custom_store";
import { DxDataGridComponent } from 'devextreme-angular';
import 'devextreme/data/odata/store';
import {
  VehicleBrandModel,
  CreateUpdateVehicleDto,
  VehicleBrand,
  BaseStateService
} from '../../../public-api';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html'
})
export class VehicleComponent implements OnInit {
  @Input() vehicleSourceArray: CreateUpdateVehicleDto[] = [];
  @ViewChild('vehicleGrid') vehicleGrid: DxDataGridComponent;
  allBrandModel: VehicleBrandModel[];
  brands: VehicleBrand[];
  brandDataSource: VehicleBrand[] = [];
  brandModelDataSource: VehicleBrandModel[] = [];
  //vehicleSourceArray: CreateUpdateVehicleDto[] = []; 
  vehicle: CreateUpdateVehicleDto = new CreateUpdateVehicleDto();
  vehicleDataSource: any;
  isValidate: boolean = false;
  validateText: string = "";
  isVehicleGridValidate?: boolean = null

  constructor(
    private BaseStateService: BaseStateService) {
  }

  ngOnInit(): void {
    this.bindBrand();
    this.vehicleDataBind();

    this.getFilteredVehicleModels = this.getFilteredVehicleModels.bind(this);
    this.setModelValue = this.setModelValue;

    this.allBrandModel = this.BaseStateService.getAllBrandModel();

    if (!this.allBrandModel?.length) {
      this.BaseStateService.dispatchGetAllBrandModel().subscribe(s => {
        this.allBrandModel = this.BaseStateService.getAllBrandModel();
        this.brandModelDataSource = this.allBrandModel;
        this.vehicleGrid.instance.refresh();
      });
    } else {
      this.brandModelDataSource = this.allBrandModel;
    }
  }

  getFilteredVehicleModels(options) {
    return {
      store: this.brandModelDataSource,
      filter: options.data ? ["brandId", "=", options.data.brandId] : null
    };
  }

  onEditorPreparing(e) {
    if (e.parentType === "dataRow" && e.dataField === "modelId") {
      e.editorOptions.disabled = (typeof e.row.data.brandId !== "string");
    }
  }

  onEditingStart(e) {
  }

  vehicleDataBind() {
    this.vehicleDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.vehicleSourceArray.filter(f => f.isDeleted == false || f.isDeleted == null)
      },
      // insert: (values) => {

      //   let length = this.vehicleSourceArray.length;
      //   let insertItem: CreateUpdateVehicleDto = {
      //     id: (length++).toString(),
      //     plate: values.plate,
      //     color: values.color,
      //     motor: values.motor,
      //     chassis: values.chassis,
      //     modelId: values.modelId,
      //     brandId: values.brandId,
      //   }
      //   this.vehicleSourceArray.push(insertItem);

      //   return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.vehicleSourceArray });
      // },
      update: (key, values) => {
        let itemIndex = this.vehicleSourceArray.findIndex(t => t.id == key);
        if (values.plate != undefined) { this.vehicleSourceArray[itemIndex].plate = values.plate; }
        if (values.color != undefined) { this.vehicleSourceArray[itemIndex].color = values.color; }
        if (values.motor != undefined) { this.vehicleSourceArray[itemIndex].motor = values.motor; }
        if (values.chassis != undefined) { this.vehicleSourceArray[itemIndex].chassis = values.chassis; }
        if (values.modelId != undefined) { this.vehicleSourceArray[itemIndex].modelId = values.modelId; }
        if (values.brandId != undefined) { this.vehicleSourceArray[itemIndex].brandId = values.brandId; }

        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.vehicleSourceArray });
      },
      remove: (key) => {
        let itemIndex = this.vehicleSourceArray.find(t => t.id == key);
        itemIndex.isDeleted = true;

        if (itemIndex.id.length < 5) {
          let itemIndex1 = this.vehicleSourceArray.findIndex(t => t.id == key);
          this.vehicleSourceArray.splice(itemIndex1, 1);
        }
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then();
      }
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

  setModelValue(rowData, value) {
    rowData.modelId = null;
    rowData.brandId = value;
    //(<any>this).defaultSetCellValue(rowData, value);
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

  clear() {
    this.vehicle = new CreateUpdateVehicleDto();
    this.vehicleSourceArray = [];
    this.vehicleGrid.instance.refresh();
  }

  save(): Promise<boolean> {
    this.isVehicleGridValidate = null;
    return this.vehicleGrid.instance.saveEditData().then(t => {
      return this.isVehicleGridValidate;
    });
  }

  refresh() {
    this.vehicleGrid.instance.refresh();
  }

  onRowValidating(e) {
    var brokenRules = e.brokenRules;
    var errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;
    this.isVehicleGridValidate = e.isValid;
    // if (brokenRules.length == 0) {
    //   this.isValidate = true;
    //   this.validateText = "";
    // } else {
    //   this.isValidate = false;
    //   this.validateText = errorText;
    // }
  }
}