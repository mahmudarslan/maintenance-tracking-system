import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { HelperService, SharedService } from '@arslan/vms.base';
import { DxTreeListComponent } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import { LocationService } from '@arslan/vms.base/proxy/location/location.service';
import { LocationDto } from '@arslan/vms.base/proxy/location';

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.scss']
})
export class LocationComponent implements OnInit {

  @ViewChild('categoryTreeList') locationTreeList: DxTreeListComponent;
  categoryDataSource: any;
  locationArray: LocationDto[] = [];
  localization: LocalizationPipe;
  filterValue: boolean = false;

  constructor(
    private helperService: HelperService,
    private locationService: LocationService,
    private localizationService: LocalizationService,
    private sharedService: SharedService,) {
  }

  ngOnInit(): void {
    //this.categoryDataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/inventory/location", "id", "withdeleteds");
    this.sharedService.hideButton();

    this.bindLocationArrayData(true);

    this.localization = new LocalizationPipe(this.localizationService);

    this.undoClick = this.undoClick.bind(this);
  }

  onCellPrepared(e) {
    if (e.column.command === "edit") {
      if (e.row && e.row.level === 0) {
        if (e.cellElement.children[0].className.includes("dx-link-add")) {
          e.cellElement.children[0].remove();
        }
      }
      if (e.row && e.row.level === 1) {
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

  bindLocationArrayData(isInitialized: boolean = false) {
    this.locationService.getAllWithDeleteds().toPromise().then(t => {
      this.locationArray = t;
      this.bindLocationStore();
      this.locationTreeList.instance.refresh();
      if (isInitialized) {
        setTimeout(() => {
          this.locationTreeList.instance.filter(["isDeleted", "=", false]);
          //this.categoryTreeList.instance.refresh();
        }, 100);
      }
    });
  }

  bindLocationStore() {
    this.categoryDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        this.locationTreeList.instance.filter(["isDeleted", "=", this.filterValue]);
        return this.locationArray
      },
      // insert: (values) => {
      //   let insertItem: LocationDto = {
      //     id: '00000000-0000-0000-0000-000000000000',
      //     parentId: values.parentId,
      //     name: values.name,
      //     isDeleted: false
      //   }

      //   //create
      //   return this.locationService.create(insertItem).toPromise().then(t => { this.bindLocationArrayData(); }).catch((e) => { console.log(e); });
      // },
      update: (key, values) => {
        let itemIndex = this.locationArray.findIndex(t => t.id == key);
        let updatedItem = this.locationArray[itemIndex];

        if (values.parentId != undefined) { updatedItem.parentId = values.parentId; }
        if (values.name != undefined) { updatedItem.name = values.name; }
        if (values.isDeleted != undefined) { updatedItem.isDeleted = values.isDeleted; }

        //update
        return this.locationService.update(updatedItem, key).toPromise().then(t => { this.bindLocationArrayData(); }).catch((e) => { console.log(e); });
      },
      remove: (key) => {
        //delete
        return this.locationService.delete(key).toPromise().then(t => { this.bindLocationArrayData(); }).catch((e) => { console.log(e); });
      }
    });
  }

  undoClick(e) {
    e.row.data.isDeleted = !e.row.data.isDeleted;
    this.locationService.undo(e.row.data.id).subscribe(
      {
        next: (n) => { this.bindLocationArrayData(); },
        error: (e) => { console.log(e); },
        complete: () => { }
      });
  }

  filterClick(e) {
    this.filterValue = e.value;

    if (e.value != null) {
      this.locationTreeList.instance.filter(["isDeleted", "=", e.value]);
    } else {
      this.locationTreeList.instance.clearFilter();
    }
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
          value: false,
          onValueChanged: this.filterClick.bind(this)
        }
      },
    );
  }

  onInitialized(e) {
    e.filter(["isDeleted", "=", false]);
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

}