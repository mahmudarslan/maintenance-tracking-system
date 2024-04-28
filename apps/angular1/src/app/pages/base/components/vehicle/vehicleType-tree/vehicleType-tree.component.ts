import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { HelperService, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { VehicleTypeDto, VehicleTypeService } from '@arslan/vms.base';
import { DxTreeListComponent } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import { BaseStateService } from '../../../services';

@Component({
  selector: 'app-vehicleType-tree',
  templateUrl: './vehicleType-tree.component.html'
})
export class VehicleTypeTreeComponent implements OnInit {


  @ViewChild('vehicleTypeTreeList') vehicleTypeTreeList: DxTreeListComponent;
  vehicleTypeDataStore: CustomStore;
  sharedButton: SharedButtonModel = new SharedButtonModel();
  loadingVisible: Boolean = false;
  localization: LocalizationPipe;
  filterValue: boolean = false;
  filterValueIsDeleted?: Boolean = false;

  constructor(private helperService: HelperService,
    private sharedService: SharedService,
    private vehicleTypeService: VehicleTypeService,
    private localizationService: LocalizationService,
    private BaseStateService: BaseStateService) {
  }

  ngOnInit(): void {
    //this.vehicleTypeDataStore = this.helperService.gridDatasourceBind("rest/api/latest/vms/base/vehicletype", "id", "");

    this.bindVehicleTypeStore();
    this.filterValueIsDeleted = this.helperService.getStorageFilterValue("Storage_VehicleType", "isDeleted");

    this.sharedService.hideButton();

    this.onReorder = this.onReorder.bind(this);

    this.localization = new LocalizationPipe(this.localizationService);

    this.undoClick = this.undoClick.bind(this);
  }

  onCellPrepared(e) {
    if (e.column.command === "edit") {
      if (e.row && e.row.level === 2) {
        if (e.cellElement.children[0].className.includes("dx-link-add")) {
          e.cellElement.children[0].remove();
        }
      }

      if (e.row && e.row.data.isDeleted == true) {
        let lastIndex = e.cellElement.children.length - 1;
        if (e.cellElement.children[lastIndex].className.includes("dx-link-delete")) {
          e.cellElement.children[lastIndex].remove();
        }
        if (e.cellElement.children[0].className.includes("dx-link-add")) {
          e.cellElement.children[0].remove();
        }
      }

    }
  }

  onDragChange(e) {
    let visibleRows = e.component.getVisibleRows(),
      sourceNode = e.component.getNodeByKey(e.itemData.id),
      targetNode = visibleRows[e.toIndex].node;

    while (targetNode && targetNode.data) {
      if (targetNode.data.id === sourceNode.data.id) {
        e.cancel = true;
        break;
      }
      targetNode = targetNode.parent;
    }
  }

  onReorder(e) {
    let visibleRows = e.component.getVisibleRows(),
      sourceData = e.itemData,
      targetData = visibleRows[e.toIndex].data;

    if (e.dropInsideItem) {
      e.itemData.parentId = targetData.id;
      //e.component.refresh();
    } else {
      // let sourceIndex = this.vehicleTypeArray.indexOf(sourceData),
      //   targetIndex = this.vehicleTypeArray.indexOf(targetData);

      // if (sourceData.parentId !== targetData.parentId) {
      //   sourceData.parentId = targetData.parentId;
      //   if (e.toIndex > e.fromIndex) {
      //     targetIndex++;
      //   }
      // }

      // this.vehicleTypeArray.splice(sourceIndex, 1);
      // this.vehicleTypeArray.splice(targetIndex, 0, sourceData);
    }

    //update
    this.vehicleTypeService.update(sourceData, sourceData.id).subscribe(
      {
        next: (n) => {//load data
          e.component.refresh();
        },
        error: (e) => { console.log(e); },
        complete: () => { }
      }
    );
  }

  bindVehicleTypeStore() {
    this.vehicleTypeDataStore = new CustomStore({
      key: 'id',
      load: (loadOptions: any) => {
        return this.vehicleTypeService.getList(loadOptions).toPromise();
      },
      insert: (values) => {
        let insertItem: VehicleTypeDto = {
          id: '00000000-0000-0000-0000-000000000000',
          parentId: values.parentId,
          name: values.name,
          isDeleted: false
        }

        //create
        return this.vehicleTypeService.create(insertItem).toPromise().catch(
          (e) => { console.log(e); }
        );
      },
      update: (key, values) => {
        // let itemIndex = this.vehicleTypeArray.findIndex(t => t.id == key);
        // let updatedItem = this.vehicleTypeArray[itemIndex];
        let updateItem: VehicleTypeDto = {
          id: values.id,
          parentId: values.parentId,
          name: values.name,
          isDeleted: values.isDeleted,
        }

        //update
        return this.vehicleTypeService.update(key, updateItem).toPromise().catch(
          (e) => { console.log(e); }
        );
      },
      remove: (key) => {
        //delete
        return this.vehicleTypeService.delete(key).toPromise().catch(
          (e) => { console.log(e); }
        );
      }
    });
  }

  undoClick(e) {
    e.row.data.isDeleted = !e.row.data.isDeleted;
    this.vehicleTypeService.undo(e.row.data.id).subscribe(
      {
        next: (n) => { this.bindVehicleTypeStore(); },
        error: (e) => { console.log(e); },
        complete: () => { }
      });
  }

  filterClick(e) {
    if (e.value != null) {
      this.vehicleTypeTreeList.instance.filter(["isDeleted", "=", e.value]);
    } else {
      this.vehicleTypeTreeList.instance.clearFilter();
    }
    //this.filterValueIsDeleted = e.value;
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
    );
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

  onRowUpdated(e) {
    this.BaseStateService.dispatchGetBrands().subscribe(s => {
    });
  }

  onRowRemoved(e) {
    this.BaseStateService.dispatchGetBrands().subscribe(s => {
    });
  }
}