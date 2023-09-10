import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { Order } from '../models/order.model';


@State<Order.State>({
    name: 'OrderState',
    defaults: {
        country: {},
        city: {},
        district: {},
        technitionUser: {},
        headTechnitionUser: {},
        role: {},
        brands: {},
        brandModels: {},
    } as Order.State,
})
@Injectable()
export class OrderState {
    constructor(
    ) { }
}