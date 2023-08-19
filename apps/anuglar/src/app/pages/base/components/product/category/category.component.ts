import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { HelperService, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { DxTreeListComponent } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import validationEngine from 'devextreme/ui/validation_engine';
import { BehaviorSubject } from 'rxjs';
import { CategoryDto, CreateUpdateCategoryDto, ProductService } from '../../../proxy/product';
import { CategoryService } from '../../../proxy/category/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {

  @ViewChild('categoryTreeList') categoryTreeList: DxTreeListComponent;
  categoryDataSource: CustomStore;
  categoryArray: CategoryDto[] = [];
  sharedButton: SharedButtonModel = new SharedButtonModel();
  loadingVisible: Boolean = false;
  localization: LocalizationPipe;
  filterValue: boolean = false;

  constructor(
    private categoryService: CategoryService,
    private helperService: HelperService,
    private localizationService: LocalizationService,
    private sharedService: SharedService) {
  }

  ngOnInit(): void {
    this.sharedService.hideButton();

    this.bindCategoryArrayData(true);

    this.onReorder = this.onReorder.bind(this);

    this.localization = new LocalizationPipe(this.localizationService);

    this.undoClick = this.undoClick.bind(this);
  }

  onCellPrepared(e) {
    if (e.column.command === "edit") {
      if (e.row && e.row.level >= 2) {
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
      let sourceIndex = this.categoryArray.indexOf(sourceData),
        targetIndex = this.categoryArray.indexOf(targetData);

      if (sourceData.parentId !== targetData.parentId) {
        sourceData.parentId = targetData.parentId;
        if (e.toIndex > e.fromIndex) {
          targetIndex++;
        }
      }

      this.categoryArray.splice(sourceIndex, 1);
      this.categoryArray.splice(targetIndex, 0, sourceData);
    }

    //update
    this.categoryService.update(sourceData, sourceData.id).subscribe(
      {
        next: (n) => { this.bindCategoryArrayData(); e.component.refresh(); },
        error: (e) => { console.log(e); },
        complete: () => { }
      }
    );
  }

  bindCategoryArrayData(isInitialized: boolean = false) {
    this.categoryService.getAllWithDeleteds().toPromise().then(t => {
      this.categoryArray = t;
      this.bindCategoryStore();
    });
  }

  bindCategoryStore() {
    this.categoryDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        this.categoryTreeList.instance.filter(["isDeleted", "=", this.filterValue]);
        return this.categoryArray;
      },
      // insert: (values) => {
      //   let insertItem: CategoryDto = {
      //     id: '00000000-0000-0000-0000-000000000000',
      //     parentId: values.parentId,
      //     name: values.name,
      //     isDeleted: false
      //   }

      //   //create
      //   return this.categoryService.create(insertItem).toPromise().then(t => { this.bindCategoryArrayData(); }).catch((e) => { console.log(e); });
      // },
      update: (key, values) => {
        let itemIndex = this.categoryArray.findIndex(t => t.id == key);
        let updatedItem = this.categoryArray[itemIndex];

        if (values.parentId != undefined) { updatedItem.parentId = values.parentId; }
        if (values.name != undefined) { updatedItem.name = values.name; }
        if (values.isDeleted != undefined) { updatedItem.isDeleted = values.isDeleted; }

        //update
        return this.categoryService.update(updatedItem, key).toPromise().then(t => { this.bindCategoryArrayData(); }).catch((e) => { console.log(e); });
      },
      remove: (key) => {
        //delete
        return this.categoryService.delete(key).toPromise().then(t => { this.bindCategoryArrayData(); }).catch((e) => { console.log(e); });
      }
    });
  }

  undoClick(e) {
    e.row.data.isDeleted = !e.row.data.isDeleted;
    this.categoryService.undo(e.row.data.id).subscribe(
      {
        next: (n) => { this.bindCategoryArrayData(); },
        error: (e) => { console.log(e); },
        complete: () => { }
      });
  }

  filterClick(e) {
    this.filterValue = e.value;

    if (e.value != null) {
      this.categoryTreeList.instance.filter(["isDeleted", "=", e.value]);
    } else {
      this.categoryTreeList.instance.clearFilter();
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