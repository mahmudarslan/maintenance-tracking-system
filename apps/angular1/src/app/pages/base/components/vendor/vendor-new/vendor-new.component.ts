import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import validationEngine from 'devextreme/ui/validation_engine';
import 'devextreme/data/odata/store';
import { DxFormComponent, DxValidatorComponent } from 'devextreme-angular';
import { SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject } from 'rxjs';
import { CreateUpdateVendor, VendorService } from '../../../proxy/vendor';
import { Address } from '../../../proxy/address';
import { UserComponent } from '../..';
import { UserDto } from '../../../proxy/user';

@Component({
  selector: 'app-vendor-new',
  templateUrl: './vendor-new.component.html'
})
export class VendorNewComponent implements OnInit {

  @ViewChild('form') form: DxFormComponent;
  @ViewChild(UserComponent) userComp: UserComponent;
  @ViewChildren(DxValidatorComponent) validatorViewChildren: QueryList<DxValidatorComponent>;
  selectedVendorId: any;
  isEditMode = false;
  address: Address = new Address();
  vendor: CreateUpdateVendor = new CreateUpdateVendor();
  user: UserDto = new UserDto();
  loadingVisible: Boolean = false;
  sharedButton: SharedButtonModel = new SharedButtonModel();

  constructor(
    private vendorService: VendorService,
    private routeParams: ActivatedRoute,
    private sharedService: SharedService) { }

  ngOnInit(): void {
    this.vendor.addresses = [];

    this.queryParameterBind();
    this.actionButtonBind();
  }

  Save() {
    this.crudAction();
  }

  crudAction() {
    let c = this.getFormData();

    if (this.isEditMode == true) {
      this.update(this.selectedVendorId, c);
    } else {
      this.create(c);
    }
  }

  getFormData(): CreateUpdateVendor {
    let customerItem = new CreateUpdateVendor();
    customerItem.name = this.user.name;
    customerItem.surname = this.user.surname;
    customerItem.userName = this.user.userName;
    customerItem.email = this.user.email;
    customerItem.phoneNumber = this.user.phoneNumber;
    customerItem.homePhoneNumber = this.user.homePhoneNumber;
    customerItem.birthDate = this.user.birthDate;
    customerItem.faxNumber = this.user.faxNumber;
    customerItem.password = "Demo_123";

    let addressItem = new Address();
    addressItem.id = this.address.id;
    addressItem.countryId = this.address.countryId;
    addressItem.cityId = this.address.cityId;
    addressItem.districtId = this.address.districtId;
    addressItem.address1 = this.address.address1;

    customerItem.addresses = [];
    customerItem.addresses.push(addressItem);
    return customerItem;
  }

  create(customerItem) {
    this.vendorService.create(customerItem).subscribe(
      () => {
        this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
          'Base::TransactionSuccesfullySaved',
          "success");
        this.clearForm();
      },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  update(id, customerItem) {
    this.vendorService.update(this.selectedVendorId, customerItem).subscribe(
      () => {
        this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
          'Base::TransactionSuccesfullySaved',
          "success",
          '/base/vendor/vendor-list');
      },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  clearForm() {
    this.vendor = new CreateUpdateVendor();
    this.address = new Address();
    this.user = new UserDto();

    setTimeout(() => {
      validationEngine.resetGroup();
    }, 100);
  }

  //#region Bind

  formDataBind(id: string, isDeleted: boolean) {
    this.loadingVisible = true;

    this.vendorService.get(id, isDeleted).subscribe(
      {
        next: result => {
          this.vendor = result;
          this.address = result.addresses[0];

          this.user.name = this.vendor.name;
          this.user.surname = this.vendor.surname;
          this.user.userName = this.vendor.userName;
          this.user.email = this.vendor.email;
          this.user.phoneNumber = this.vendor.phoneNumber;
          this.user.homePhoneNumber = this.vendor.homePhoneNumber;
          this.user.birthDate = this.vendor.birthDate;
          this.user.faxNumber = this.vendor.faxNumber;

          if (result.isDeleted) {
            this.form.disabled = true;
            this.sharedService.hideButton();
          }

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

  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedVendorId = params.get("id");
      let parameterIsDeleted = params.get("isDeleted") == "true" ? true : false;

      if (this.selectedVendorId !== undefined && this.selectedVendorId != null) {
        this.isEditMode = true;
        this.formDataBind(this.selectedVendorId, parameterIsDeleted);
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      } else {
        this.isEditMode = false;
        this.clearForm();
        this.sharedButton = this.sharedService.showButton(this.isEditMode);
      }
    })
  }

  actionButtonBind() {
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

  //#endregion
}