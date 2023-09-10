import { Component, OnInit } from '@angular/core';
import { HelperService, SharedService } from '@arslan/vms.base';
import { BaseStateService } from '@arslan/vms.base';

@Component({
  selector: 'app-addresstree',
  templateUrl: './addresstree.component.html',
  styleUrls: ['./addresstree.component.scss']
})
export class AddressTreeComponent implements OnInit {

  addressDataSource: any;

  constructor(private helperService: HelperService,
    private sharedService: SharedService,
    private BaseStateService: BaseStateService) {
  }

  ngOnInit(): void {
    this.addressDataSource = this.helperService.gridDatasourceBind("rest/api/latest/vms/base/address", "id", "WithDeleteds");
    this.sharedService.hideButton();
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

  onRowUpdated(e) {
    this.BaseStateService.dispatchGetBrands().subscribe(s => {
    });
  }

  onRowRemoved(e) {
    this.BaseStateService.dispatchGetBrands().subscribe(s => {
    });
  }
}