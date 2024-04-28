import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core'; 
import { Observable } from 'rxjs';
import { CurrentStockDto } from '.';

@Injectable({
  providedIn: 'root'
})
export class CurrentStockService {
  constructor(private restService: RestService) { }

   
}