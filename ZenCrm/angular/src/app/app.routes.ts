import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'crm/dashboard',
    loadComponent: () => import('./dashboard/sales-dashboard.component').then(c => c.SalesDashboardComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () => import('@abp/ng.tenant-management').then(c => c.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(c => c.createRoutes()),
  },
  {
    path: 'books',
    loadComponent: () => import('./book/book.component').then(c => c.BookComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'crm/clients',
    loadComponent: () => import('./clients/client.component').then(c => c.ClientComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'crm/clients/:id',
    loadComponent: () => import('./clients/client-detail.component').then(c => c.ClientDetailComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'crm/customers',
    loadComponent: () => import('./customers/customer.component').then(c => c.CustomerComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'crm/opportunities',
    loadComponent: () => import('./opportunities/opportunities.component').then(c => c.OpportunitiesComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'crm/opportunities/pipeline',
    loadComponent: () => import('./opportunities/opportunity-pipeline.component').then(c => c.OpportunityPipelineComponent),
    canActivate: [authGuard, permissionGuard],
  },
];
