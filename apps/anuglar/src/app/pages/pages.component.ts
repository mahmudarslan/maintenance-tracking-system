import { LocalizationPipe, LocalizationService, RoutesService } from '@abp/ng.core';
import { Component } from '@angular/core';

import { MENU_ITEMS } from './pages-menu';

@Component({
  selector: 'ngx-pages',
  styleUrls: ['pages.component.scss'],
  template: `
    <ngx-one-column-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-one-column-layout>
  `,
})
export class PagesComponent {
  menu = [];
  private localization: LocalizationPipe;

  constructor(public readonly routes: RoutesService,
    private localizationService: LocalizationService) {

    this.localization = new LocalizationPipe(this.localizationService);

    this.routes.tree.forEach(f => {
      this.menu.push({
        title: this.localization.transform(f.name), link: '/pages/' + f.path, icon: f.iconClass,
        children: f.children.map(m => {
          return {
            title: this.localization.transform(m.name),
            link: '/pages/' + m.path,
            icon: m.iconClass
          };
        })
      });
    });
  }

}
