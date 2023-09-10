import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Store } from '@ngxs/store';
import { SessionStateService } from '@abp/ng.core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { RestService, ConfigStateService, CurrentTenantDto } from '@abp/ng.core';
import { PurchaseOrder } from '.';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrderService {

  constructor(
    private restService: RestService,
    private store: Store,
    private http: HttpClient,
    private sessionState: SessionStateService,
    private configState: ConfigStateService) { }

  create(salesOrderInput: PurchaseOrder.CreateUpdatePurchaseOrderDto): Observable<PurchaseOrder.PurchaseOrder> {
    return this.restService.request<PurchaseOrder.CreateUpdatePurchaseOrderDto, PurchaseOrder.PurchaseOrder>({
      method: 'POST',
      url: 'rest/api/latest/vms/orders/purchase',
      body: salesOrderInput
    })
  }

  update(salesOrderInput: PurchaseOrder.CreateUpdatePurchaseOrderDto, id: string): Observable<PurchaseOrder.PurchaseOrder> {
    return this.restService.request<PurchaseOrder.CreateUpdatePurchaseOrderDto, PurchaseOrder.PurchaseOrder>({
      method: 'PUT',
      url: `rest/api/latest/vms/orders/purchase/${id}`,
      body: salesOrderInput
    })
  }

  delete(id: string): Observable<any> {
    return this.restService.request<void, null>({
      method: 'DELETE',
      url: `rest/api/latest/vms/orders/purchase/${id}`,
    })
  }

  undo(id: string): Observable<any> {
    return this.restService.request<PurchaseOrder.CreateUpdatePurchaseOrderDto, null>({
      method: 'POST',
      url: `rest/api/latest/vms/orders/purchase/undo?Id=${id}`,
    })
  }

  get(id: string, isDeleted: boolean): Observable<PurchaseOrder.PurchaseOrder> {
    return this.restService.request<void, PurchaseOrder.PurchaseOrder>({
      method: 'GET',
      url: `rest/api/latest/vms/orders/purchase/${id}/${isDeleted}`
    });
  }

  getStatus(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/purchase/StatusList'
    })
  }

  getInventoryStatus(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/purchase/InventoryStatusList'
    })
  }

  getPaymentStatus(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/purchase/PaymentStatusList'
    })
  }

  getWorkorderTypes(): Observable<any> {
    return this.restService.request<void, any>({
      method: 'GET',
      url: 'rest/api/latest/vms/orders/purchase/workorderTypes'
    })
  }
}