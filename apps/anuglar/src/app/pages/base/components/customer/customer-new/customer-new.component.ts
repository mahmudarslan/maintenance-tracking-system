import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import validationEngine from 'devextreme/ui/validation_engine';
import 'devextreme/data/odata/store';
import {
  CustomersService,
  CreateUpdateVehicleDto,
  CreateUpdateCustomerWithVehicleDto,
  Address,
  EmployeeRole,
  VehicleComponent,
  UserDto,
  UserComponent
} from '../../../public-api';
import { SharedButtonModel, SharedService } from '@arslan/vms.base';
import { BehaviorSubject } from 'rxjs';
import { DxFormComponent } from 'devextreme-angular';

@Component({
  selector: 'app-customer-new',
  templateUrl: './customer-new.component.html',
})

export class CustomerNewComponent implements OnInit {
  @ViewChild('form') form: DxFormComponent;
  @ViewChild(VehicleComponent) vehicleComp: VehicleComponent;
  @ViewChild(UserComponent) userComp: UserComponent;
  selectedCustomerId: any;
  isEditMode = false;
  addPhoneButtonOptions: any;
  now: Date = new Date();
  minDate: Date = new Date(1900, 0, 1);
  address: Address = new Address();
  customer: CreateUpdateCustomerWithVehicleDto = new CreateUpdateCustomerWithVehicleDto();
  roles: EmployeeRole[];
  vehicleSourceArray: CreateUpdateVehicleDto[] = [];
  user: UserDto = new UserDto();
  sharedButton: SharedButtonModel = new SharedButtonModel();
  popupVisible = false;
  loadingVisible: Boolean = false;

  constructor(
    private customerService: CustomersService,
    private sharedService: SharedService,
    private routeParams: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.sharedButton.hitCount = 0;
    this.customer.addresses = [];
    this.customer.vehicles = [];

    this.queryParameterBind();
    this.actionButtonBind();
  }

  Save() {
    this.vehicleComp.save().then(t => {
      if (t != false) {
        this.crudAction();
      } else {
        this.sharedService.buttonComplate(this.sharedButton);
      }
    });
  }

  crudAction() {
    let c = this.getFormData();

    if (this.isEditMode == true) {
      this.update(this.selectedCustomerId, c);
    } else {
      this.create(c);
    }
  }

  getFormData(): CreateUpdateCustomerWithVehicleDto {
    let customerItem = new CreateUpdateCustomerWithVehicleDto();
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

    this.vehicleSourceArray.forEach(element => {
      if (element.id.length < 5) {
        element.id = '00000000-0000-0000-0000-000000000000';
      }
    });
    customerItem.vehicles = this.vehicleSourceArray;

    return customerItem;
  }

  create(customerItem) {
    this.customerService.create(customerItem).subscribe(
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
    this.customerService.update(id, customerItem).subscribe(
      () => {
        this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
          'Base::TransactionSuccesfullySaved',
          "success",
          '/base/customer/customer-list');
      },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }


  clearForm() {
    this.customer = new CreateUpdateCustomerWithVehicleDto();
    this.address = new Address();
    this.user = new UserDto();
    this.vehicleSourceArray = [];

    setTimeout(() => {
      validationEngine.resetGroup();
      this.vehicleComp.refresh();
    }, 100);
  }

  //#region Bind

  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedCustomerId = params.get("id");
      let parameterIsDeleted = params.get("isDeleted") == "true" ? true : false;
      if (this.selectedCustomerId !== undefined && this.selectedCustomerId != null) {
        this.isEditMode = true;
        this.formDataBind(this.selectedCustomerId, parameterIsDeleted);
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
        // this.popupVisible = true;

      } else {
        this.Save();
      }
    });
  }

  formDataBind(id: string, isDeleted: boolean) {
    this.loadingVisible = true;

    this.customerService.get(id, isDeleted).subscribe(
      {
        next: (result) => {
          this.customer = result;
          this.address = result.addresses[0];
          this.vehicleSourceArray = result.vehicles;
          this.vehicleComp.refresh();

          this.user.name = this.customer.name;
          this.user.surname = this.customer.surname;
          this.user.userName = this.customer.userName;
          this.user.email = this.customer.email;
          this.user.phoneNumber = this.customer.phoneNumber;
          this.user.homePhoneNumber = this.customer.homePhoneNumber;
          this.user.birthDate = this.customer.birthDate;
          this.user.faxNumber = this.customer.faxNumber;

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

  //#endregion
}