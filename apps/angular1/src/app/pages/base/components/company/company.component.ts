import { ApiInterceptor, ConfigStateService, CurrentTenantDto } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FileAttachmentDto, HelperService, SharedButtonModel, SharedService, SecurePipe } from '@arslan/vms.base';
import validationEngine from 'devextreme/ui/validation_engine';
import { Guid } from 'guid-typescript';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Address } from '../../proxy/address';
import { CreateUpdateCompany } from '../../proxy/company';
import { CompanyService } from '../../proxy/company/company.service';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit {
  selectedCompanyId: any;
  address: Address = new Address();
  company: CreateUpdateCompany = new CreateUpdateCompany();
  initialized: boolean = false;
  sharedButton: SharedButtonModel = new SharedButtonModel();
  value: any[] = [];
  uploadHeaders: any;
  uploadFakeId = 0;
  url: string = environment.apis.default.url + "rest/api/latest/vms/";
  uploadUrl = this.url + "core/file/upload";
  downloadUrl = "";
  fakeId: any;
  fileSourceArray: FileAttachmentDto[] = [];
  imageSource = "";
  isDropZoneActive = false;
  textVisible = true;
  progressVisible = false;
  progressValue = 0;
  loadingVisible: Boolean = false;

  constructor(
    private companyService: CompanyService,
    private sharedService: SharedService,
    private configState: ConfigStateService,
    private apiInterceptor: ApiInterceptor,
    private routeParams: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.company.addresses = [];
    this.bindCompany();
    this.sharedButton = this.sharedService.showButton(true);

    this.addIdParameter = this.addIdParameter.bind(this);

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

    this.setUploadHeader();
  }

  Save() {

    this.company.fakeId = this.fakeId;

    let addressItem = new Address();
    addressItem.id = this.address.id;
    addressItem.countryId = this.address.countryId;
    addressItem.cityId = this.address.cityId;
    addressItem.districtId = this.address.districtId;
    addressItem.address1 = this.address.address1;

    this.company.addresses = [];
    this.company.addresses.push(addressItem);

    this.update(this.selectedCompanyId, this.company);

  }

  update(id, company) {
    this.companyService.update(id, company).subscribe(() => {
      this.sharedService.buttonComplateWithLocalizationNotify(this.sharedButton,
        'Base::TransactionSuccesfullySaved',
        "success");
    },
      error => {
        this.sharedService.buttonComplateWithNotify(this.sharedButton, error, "error");
      },
    );
  }

  clearForm() {
    this.company = new CreateUpdateCompany();
    this.address = new Address();

    setTimeout(() => {
      validationEngine.resetGroup();
    }, 100);
  }

  bindCompany() {
    this.loadingVisible = true;

    this.companyService.get().subscribe(
      {
        next: (result) => {
          this.company = result;
          this.address = result.addresses[0] ?? new Address();
          this.fileSourceArray = result.files ?? new Array<FileAttachmentDto>();
          if (result.files.length > 0) {
            this.downloadUrl = "rest/api/latest/vms/core/file/download?id=" + result.files[0].id;
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

  onKeyDown(e) {
    let keyCode = e.event.keyCode;
    // Disallow anything not matching the regex pattern (A to Z uppercase, a to z lowercase and white space)
    var englishAlphabetAndNumber = /[A-Za-z0-9]/g;
    if (keyCode == 8 || keyCode == 37 || keyCode == 39 || englishAlphabetAndNumber.test(e.event.key)) {
    } else {
      e.event.preventDefault();
    }
  }

  updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
      return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
      return uri + separator + key + "=" + value;
    }
  }

  setUploadHeader() {
    //const currentTenant = this.configState.getOne('currentTenant') as CurrentTenantDto;
    this.fakeId = (Guid.create()).toString();
    this.uploadHeaders = this.apiInterceptor.getAdditionalHeaders();

    // {
    //   "__tenant": currentTenant && currentTenant.id,
    //   "fake_id": this.fakeId,
    // }
  }

  addIdParameter(e,) {
    this.uploadUrl = this.updateQueryStringParameter(this.uploadUrl, "id", this.fakeId);
    e.component.option("uploadUrl", this.uploadUrl);

    e.value.forEach((value) => {
      let isPreValue = false;

      if (e.previousValue.length > 0) {
        e.previousValue.forEach((value1) => {
          if (value1.name == value.name) { isPreValue = true; }
        });
      }

      if (!isPreValue) {
        this.uploadFakeId += 1;

        const file = e.value[0];
        const fileReader = new FileReader();
        fileReader.onload = () => {

          this.imageSource = fileReader.result as string;
          this.fileSourceArray.push(
            {
              id: this.uploadFakeId.toString(),
              downloadUrl: '/',
              fileName: value.name,
              isDeleted: false,
              source: fileReader.result as string
            });
        }
        fileReader.readAsDataURL(file);
      }
    });
  }
  //#endregion
}