import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { CreateUpdateStockAdjustmentDto } from '.';
import { Observable } from 'rxjs';
import { CurrentStockDto } from '../current-stock';

@Injectable({
  providedIn: 'root'
})
export class AdjustStockService {
  constructor(private restService: RestService) { }

  create(input: CreateUpdateStockAdjustmentDto): Observable<any> {
    return this.restService.request<CreateUpdateStockAdjustmentDto, null>({
      method: 'POST',
      url: 'rest/api/latest/vms/inventory/stockadjustment',
      body: input
    });
  }

  update(id: string, input: CreateUpdateStockAdjustmentDto): Observable<any> {
    return this.restService.request<CreateUpdateStockAdjustmentDto, null>({
      method: 'PUT',
      url: `rest/api/latest/vms/inventory/stockadjustment/${id}`,
      body: input
    });
  }

  delete(id: string): Observable<any> {
    return this.restService.request<void, null>({
      method: 'DELETE',
      url: `rest/api/latest/vms/inventory/stockadjustment/${id}`,
    });
  }

  undo(id: string): Observable<any> {
    return this.restService.request<CreateUpdateStockAdjustmentDto, null>({
      method: 'POST',
      url: `rest/api/latest/vms/inventory/stockadjustment/undo?Id=${id}`,
    });
  }

  get(id: string, isDeleted: boolean): Observable<CreateUpdateStockAdjustmentDto> {
    return this.restService.request<void, CreateUpdateStockAdjustmentDto>({
      method: 'GET',
      url: `rest/api/latest/vms/inventory/stockadjustment/${id}/${isDeleted}`
    });
  }

  getCurrentStock(id: string): Observable<CurrentStockDto> {
    return this.restService.request<void, CurrentStockDto>({
      method: 'GET',
      url: `rest/api/latest/vms/inventory/stockadjustment/currentstock?Id=${id}`
    });
  }

  getCurrentStockWithLocation(id: string, locationId: string): Observable<CurrentStockDto> {
    return this.restService.request<void, CurrentStockDto>({
      method: 'GET',
      url: `rest/api/latest/vms/inventory/stockadjustment/CurrentStockWithLocation?Id=${id}&locationId=${locationId}`
    });
  }
}
