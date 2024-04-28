import { Injectable } from "@angular/core";
import { Store } from "@ngxs/store";

@Injectable({
    providedIn: 'root',
})
export class OrderStateService {
    constructor(private store: Store) { }
}