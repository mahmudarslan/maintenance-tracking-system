import { Component, OnInit, Input, Output, EventEmitter, ViewChild, OnChanges, DoCheck, AfterContentInit, AfterContentChecked, AfterViewInit, AfterViewChecked, OnDestroy, SimpleChanges } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DxSelectBoxComponent } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';
import { Address, CityDto, CountryDto, DistrictDto } from '../../../proxy/address';
import { BaseStateService } from '../../../services';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html'
})
export class AddressComponent implements OnInit {

  @Input() address: Address;
  countryDataSource: DataSource;
  cityDataSource: DataSource;
  districtDataSource: DataSource;
  countries: CountryDto[];
  cities: CityDto[];
  districts: DistrictDto[];
  @ViewChild('countrySelectBox', { static: false }) countrySelectBox: DxSelectBoxComponent;

  constructor(
    private BaseStateService: BaseStateService) {
  }

  ngOnInit(): void {
    this.countries = this.BaseStateService.getCountries();

    setTimeout(() => {
      if (!this.countries?.length) {
        this.BaseStateService.dispatchGetCountries().subscribe(s => {
          this.countries = this.BaseStateService.getCountries();
          this.setCountry();
        });
      } else {
        this.setCountry();
      }
    }, 500);
  }

  setCountry() {
    if (this.countries.length == 0) { return }

    this.bindCountry();
    if (this.address && this.address.countryId == null) {
      this.countrySelectBox.instance.option("value", this.countries[0].id);
    }
  }

  onCountryChanged(e) {
    if (!e.value) {
      this.cities = null;
      this.bindCity();
      return;
    }

    this.cities = this.BaseStateService.getCities();

    if (!this.cities.length || this.cities[0].countryId != e.value) {
      this.BaseStateService.dispatchGetCityByCountryId(e.value).subscribe(s => {
        this.cities = this.BaseStateService.getCities();
        this.bindCity();
      });
    } else {
      this.bindCity();
    }
  }

  onCityChanged(e) {
    if (!e.value) {
      this.districts = null;
      this.bindDistrict();
      return;
    }

    this.districts = this.BaseStateService.getDistricts();

    if (!this.districts.length || this.districts[0].cityId != e.value) {
      this.BaseStateService.dispatchGetDistrictByCityId(e.value).subscribe(s => {
        this.districts = this.BaseStateService.getDistricts();
        this.bindDistrict();
      });
    } else {
      this.bindDistrict();
    }
  }

  bindCountry() {
    this.countryDataSource = new DataSource(
      {
        store: new CustomStore(
          {
            key: "ID",
            loadMode: "raw",
            load: () => {
              return this.countries;
            }
          })
      }
    );
  }

  bindCity() {
    this.cityDataSource = new DataSource({
      store: new CustomStore({
        key: "ID",
        loadMode: "raw",
        load: () => {
          return this.cities;
        }
      })
    });
  }

  bindDistrict() {
    this.districtDataSource = new DataSource({
      store: new CustomStore({
        key: "ID",
        loadMode: "raw",
        load: () => this.districts
      })
    });
  }

}