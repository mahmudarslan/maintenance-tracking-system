import { Component, Input, OnInit, ViewChild } from '@angular/core';
import CustomStore from "devextreme/data/custom_store";
import { DxDataGridComponent } from 'devextreme-angular';
import 'devextreme/data/odata/store';
import { CreateUpdateVehicleDto, BaseStateService, VehicleBrand, VehicleBrandModel } from '@arslan/vms.base';
import { DocNumberDto, DocNumberService, DocTypeDto } from '../../proxy/doc-numbers';
import { HelperService, SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject } from 'rxjs';
import validationEngine from 'devextreme/ui/validation_engine';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-doc-numbers',
  templateUrl: './doc-numbers.component.html',
  styleUrls: ['./doc-numbers.component.scss']
})
export class DocNumbersComponent implements OnInit {


  //dataSource: any;
  DocTypeNoSourceArray: DocNumberDto[] = [];
  @ViewChild('docNoGrid') docNoGrid: DxDataGridComponent;
  docTypeDataSource: DocTypeDto[] = [];
  DocTypeNoDataSource: CustomStore;
  isValidate: boolean = false;
  validateText: string = "";
  sharedButton: SharedButtonModel = new SharedButtonModel();

  constructor(private docNumberService: DocNumberService,
    private sharedService: SharedService,
    private helperService: HelperService,
    private routeParams: ActivatedRoute) {
    //this.dataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/core/documentNoFormats", "id");

    this.bindDocNoData();
  }

  ngOnInit(): void {
    this.bindDocTypes();

    this.sharedButton = this.sharedService.showButton(true);

    this.sharedService.bClickSendSubject$ = new BehaviorSubject(new SharedButtonModel());

    this.sharedService.bClickSendSubject$.subscribe(s => {
      if (s && !s.name) return;
      validationEngine.validateGroup();

      if (!validationEngine.validateGroup().isValid) {
        this.sharedService.bComplatedSubject$.next(this.sharedButton);
      } else {
        this.Save();
      }
    });
  }

  bindDocNoData() {
    this.docNumberService.getAll().toPromise().then(t => {
      this.DocTypeNoSourceArray = t;
      this.DocTypeNoDataBind();
    });
  }

  onEditorPreparing(e) {
    if (e.parentType === "dataRow" && e.dataField === "modelId") {
      e.editorOptions.disabled = (typeof e.row.data.brandId !== "string");
    }
  }

  onEditingStart(e) {
  }

  DocTypeNoDataBind() {
    this.DocTypeNoDataSource = new CustomStore({
      key: 'id',
      loadMode: 'raw',
      load: () => {
        return this.DocTypeNoSourceArray;
      },
      // insert: (values) => {
      //   return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.DocTypeNoSourceArray });
      // },
      update: (key, values) => {
        let itemIndex = this.DocTypeNoSourceArray.findIndex(t => t.id == key);
        //if (values.docNoType != undefined) { this.DocTypeNoSourceArray[itemIndex].docNoType = values.docNoType; }
        if (values.nextNumber != undefined) { this.DocTypeNoSourceArray[itemIndex].nextNumber = values.nextNumber; }
        if (values.minDigits != undefined) { this.DocTypeNoSourceArray[itemIndex].minDigits = values.minDigits; }
        if (values.prefix != undefined) { this.DocTypeNoSourceArray[itemIndex].prefix = values.prefix; }
        if (values.suffix != undefined) { this.DocTypeNoSourceArray[itemIndex].suffix = values.suffix; }

        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then(t => { return this.DocTypeNoSourceArray });
      },
      remove: (key) => {
        return new Promise((resolve, reject) => { resolve([{}]), reject([{}]) }).then();
      }
    });
  }

  refreshClick = e => {
    this.docNumberService.getAll().toPromise().then(t => {
      this.DocTypeNoSourceArray = t;
      //this.DocTypeNoDataBind();
      this.docNoGrid.instance.refresh();
    });

    //this.docNoGrid.instance.refresh();
  }

  onToolbarPreparing(e) {
    let toolbarItems = e.toolbarOptions.items;
    // Modifies an existing item
    toolbarItems.forEach(function (item) {
      if (item.name === "saveButton") {
        item.visible = false;
      }
    });

    e.toolbarOptions.items.unshift(
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

  clear() {
    this.DocTypeNoSourceArray = [];
    this.docNoGrid.instance.refresh();
  }

  Save() {
    this.docNoGrid.instance.saveEditData().then(t => {
      return this.validateText;
    });

    this.docNumberService.update(this.DocTypeNoSourceArray).subscribe(
      () => {
        this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
          'Core::TransactionSuccesfullySaved',
          "success");
      },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  refresh() {
    this.docNoGrid.instance.refresh();
  }

  onRowValidating(e) {
    var brokenRules = e.brokenRules, errorText = brokenRules.map(function (rule) { return rule.message; }).join(", ");
    e.errorText = errorText;

    if (brokenRules.length == 0) {
      this.isValidate = true;
      this.validateText = "";
    } else {
      this.isValidate = false;
      this.validateText = errorText;
    }
  }

  bindDocTypes() {
    this.docNumberService.getDocNoTypes().toPromise().then(t => {
      this.docTypeDataSource = t;
    });
  }
}