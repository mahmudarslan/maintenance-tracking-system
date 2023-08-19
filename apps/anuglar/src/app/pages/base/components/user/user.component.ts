import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DxFormComponent } from 'devextreme-angular';
import { EmployeeRole } from '../../proxy/employee';
import { UserDto } from '../../proxy/user';
import { BaseStateService } from '../../services';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  @Input() user: UserDto = new UserDto();
  @Input() roleVisible: boolean = false;
  @Input() isEditMode: boolean = false;
  @ViewChild('userForm') formComp: DxFormComponent;

  selectedEmployeeId: any;
  roleSource: any;
  roleArraySource: any;
  initialized: boolean = false;
  now: Date = new Date();
  minDate: Date = new Date(1950, 0, 0);
  roles: EmployeeRole[];

  constructor(
    private BaseStateService: BaseStateService) {
  }

  ngOnInit(): void {

    this.now.setFullYear(this.now.getFullYear() - 10);
    this.minDate.setFullYear(this.now.getFullYear() - 70);

    if (this.roleVisible) {
      this.bindRole();
    }
  }

  onRoleChanged(e) {
    this.user.selectedRole = e.value;
  }

  bindRole() {
    this.roles = this.BaseStateService.getRoles();

    setTimeout(() => {
      if (!this.roles.length) {
        this.BaseStateService.dispatchGetRoles().subscribe(s => {
          this.roles = this.BaseStateService.getRoles();
          this.setRole();
        });
      } else {
        this.setRole();
      }
    }, 100);
  }

  setRole() {
    this.roleSource = this.roles;
    if (this.isEditMode == false) {
      this.user.selectedRole = this.roleSource[0].value;
    }
  }

  onKeyDown(e) {
    let keyCode = e.event.keyCode;
    // Disallow anything not matching the regex pattern (A to Z uppercase, a to z lowercase and white space)
    var englishAlphabetAndNumber = /[A-Za-z0-9]/g;
    if (keyCode == 8 || keyCode == 37 || keyCode == 39 || englishAlphabetAndNumber.test(e.event.key)) {
    } else {
      e.event.preventDefault();
    }
  }

}