import { ABP } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';

@Injectable({
    providedIn: 'root',
})
export class InventoryStateService {
    constructor(private store: Store) { }
}