import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/books',
        name: '::Menu:Books',
        iconClass: 'fas fa-book',
        layout: eLayoutType.application,
        requiredPolicy: 'ZenCrm.Books',
      },
      {
        path: '/crm',
        name: '::Menu:CRM',
        iconClass: 'fas fa-users',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/crm/clients',
        name: '::Menu:Clients',
        iconClass: 'fas fa-building',
        parentName: '::Menu:CRM',
        layout: eLayoutType.application,
      },
      {
        path: '/crm/customers',
        name: '::Menu:Customers',
        iconClass: 'fas fa-user',
        parentName: '::Menu:CRM',
        layout: eLayoutType.application,
      },
  ]);
}
