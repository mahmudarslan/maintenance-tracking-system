import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root'
})
export class ReorderStockService {

  constructor(private restService: RestService) { }
}