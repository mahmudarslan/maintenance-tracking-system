import { Injectable } from '@angular/core';
import { State, Action, StateContext } from '@ngxs/store';

export class InventoryStateModel {
  public items: string[];
}

const defaults = {
  items: []
};

@State<InventoryStateModel>({
  name: 'inventory',
  defaults
})
@Injectable()
export class InventoryState {
  
}
