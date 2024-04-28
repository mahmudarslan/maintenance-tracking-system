import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Store } from '@ngxs/store';
import { ApiInterceptor, SessionStateService } from '@abp/ng.core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { RestService, ConfigStateService, CurrentTenantDto } from '@abp/ng.core';
import { SalesOrder } from '.';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SalesOrderService {

  constructor(
    private restService: RestService,
    private store: Store,
    private http: HttpClient,
    private sessionState: SessionStateService,
    private apiInterceptor: ApiInterceptor,
    private configState: ConfigStateService) { }

  create(salesOrderInput: SalesOrder.CreateUpdateSalesOrderDto): Observable<SalesOrder.SalesOrder> {
    return this.restService.request<SalesOrder.CreateUpdateSalesOrderDto, SalesOrder.SalesOrder>({
      method: 'POST',
      url: 'rest/api/latest/vms/orders/sales',
      body: salesOrderInput
    })
  }

  update(salesOrderInput: SalesOrder.CreateUpdateSalesOrderDto, id: string): Observable<SalesOrder.SalesOrder> {
    return this.restService.request<SalesOrder.CreateUpdateSalesOrderDto, SalesOrder.SalesOrder>({
      method: 'PUT',
      url: `rest/api/latest/vms/orders/sales/${id}`,
      body: salesOrderInput
    })
  }

  delete(id: string): Observable<any> {
    return this.restService.request<void, null>({
      method: 'DELETE',
      url: `rest/api/latest/vms/orders/sales/${id}`,
    })
  }

  undo(id: string): Observable<any> {
    return this.restService.request<SalesOrder.CreateUpdateSalesOrderDto, null>({
      method: 'POST',
      url: `rest/api/latest/vms/orders/sales/undo?Id=${id}`,
    })
  }

  get(id: string, isDeleted: boolean): Observable<SalesOrder.SalesOrder> {
    return this.restService.request<void, SalesOrder.SalesOrder>({
      method: 'GET',
      url: `rest/api/latest/vms/orders/sales/${id}/${isDeleted}`
    });
  }

  getStatus(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/sales/StatusList'
    })
  }

  getInventoryStatus(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/sales/InventoryStatusList'
    })
  }

  getPaymentStatus(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/sales/PaymentStatusList'
    })
  }

  getWorkorderTypes(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/sales/workorderTypes'
    })
  }
}