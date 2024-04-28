import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedButtonModel, SharedService } from '@arslan/vms.base';
import { DxButtonComponent, DxFormComponent } from 'devextreme-angular';
import validationEngine from 'devextreme/ui/validation_engine';
import { BehaviorSubject, Subject } from 'rxjs';
import {
  EmployeeService,
  CreateUpdateEmployee,
  Address,
  EmployeeRole,
  UserDto
} from '../../../public-api';

@Component({
  selector: 'app-employee-new',
  templateUrl: './employee-new.component.html',
  styleUrls: ['./employee-new.component.css']
})
export class EmployeeNewComponent implements OnInit {

  @ViewChild('form') form: DxFormComponent;
  @ViewChild('saveButton') saveButton: DxButtonComponent;
  @ViewChild(DxFormComponent) tabPanel: DxFormComponent;
  saveObservable = new Subject<boolean>();
  selectedEmployeeId: any;
  address: Address = new Address();
  employee: CreateUpdateEmployee = new CreateUpdateEmployee();
  isEditMode = false;
  roleSource: any;
  roleArraySource: any;
  initialized: boolean = false;
  now: Date = new Date();
  minDate: Date = new Date(1900, 0, 1);
  roles: EmployeeRole[];
  user: UserDto = new UserDto();
  sharedButton: SharedButtonModel = new SharedButtonModel();
  buttonHitCount = 0;
  loadingVisible: Boolean = false;
  buttonOptions: any = {
    text: "Register",
    type: "success",
    useSubmitBehavior: true
  }

  constructor(
    private employeeService: EmployeeService,
    private sharedService: SharedService,
    private routeParams: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.employee.addresses = [];

    this.queryParameterBind();
    this.actionButtonBind();
  }


  queryParameterBind() {
    this.routeParams.queryParamMap.subscribe(params => {
      this.selectedEmployeeId = params.get("id");
      let parameterIsDeleted = params.get("isDeleted") == "true" ? true : false;

      if (this.selectedEmployeeId !== undefined && this.selectedEmployeeId != null) {
        this.isEditMode = true;
        this.formDataBind(this.selectedEmployeeId, parameterIsDeleted);
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
      let aa = validationEngine.validateGroup();

      if (!aa.isValid) {
        this.sharedService.bComplatedSubject$.next(this.sharedButton);
      } else {
        this.save();
      }
    });
  }

  save() {
    this.crudAction();


    return this.saveObservable;
  }

  crudAction() {
    let c = this.getFormData();

    if (this.isEditMode == true) {
      this.update(this.selectedEmployeeId, c);
    } else {
      this.create(c);
    }
  }

  getFormData(): CreateUpdateEmployee {
    this.user.roles = [];
    this.user.roles.push(this.user.selectedRole);
    if (this.user.isCustomerRole) {
      this.user.roles.push('CUSTOMER');
    }

    let employeeItem = new CreateUpdateEmployee();
    employeeItem.name = this.user.name;
    employeeItem.surname = this.user.surname;
    employeeItem.userName = this.user.userName;
    employeeItem.email = this.user.email;
    employeeItem.phoneNumber = this.user.phoneNumber;
    employeeItem.homePhoneNumber = this.user.homePhoneNumber;
    employeeItem.birthDate = this.user.birthDate;
    employeeItem.faxNumber = this.user.faxNumber;
    employeeItem.password = "Demo_123";
    employeeItem.roles = this.user.roles;

    let addressItem = new Address();
    addressItem.id = this.address.id;
    addressItem.countryId = this.address.countryId;
    addressItem.cityId = this.address.cityId;
    addressItem.districtId = this.address.districtId;
    addressItem.address1 = this.address.address1;

    employeeItem.addresses = [];
    employeeItem.addresses.push(addressItem);

    return employeeItem;
  }

  create(employeeItem) {
    this.employeeService.create(employeeItem).subscribe(() => {
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

  update(id, employeeItem) {
    this.employeeService.update(id, employeeItem).subscribe(() => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        "success",
        '/base/employee/employee-list');
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  clearForm() {
    this.employee = new CreateUpdateEmployee();
    this.address = new Address();
    this.user = new UserDto();

    setTimeout(() => {
      validationEngine.resetGroup();
    }, 10);
  }

  formDataBind(id: string, isDeleted: boolean) {
    this.loadingVisible = true;

    this.employeeService.get(id, isDeleted).subscribe(
      {
        next: result => {
          this.employee = result;
          this.address = result.addresses[0] ?? new Address();

          this.user.isCustomerRole = this.employee.isCustomerRole;
          this.user.name = this.employee.name;
          this.user.surname = this.employee.surname;
          this.user.userName = this.employee.userName;
          this.user.email = this.employee.email;
          this.user.phoneNumber = this.employee.phoneNumber;
          this.user.homePhoneNumber = this.employee.homePhoneNumber;
          this.user.birthDate = this.employee.birthDate;
          this.user.faxNumber = this.employee.faxNumber;
          this.user.selectedRole = this.employee.role;

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
}