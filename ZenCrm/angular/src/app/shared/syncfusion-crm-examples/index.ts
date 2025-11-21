// Export all CRM example components
export { SyncfusionClientGridComponent } from './syncfusion-client-grid.component';
export { SyncfusionOpportunityFormComponent } from './syncfusion-opportunity-form.component';
export { SyncfusionSimpleDashboardComponent } from './syncfusion-simple-dashboard.component';
export { SyncfusionMetricsPanelComponent } from './syncfusion-metrics-panel.component';
export { SyncfusionChartDashboardComponent } from './syncfusion-chart-dashboard.component';
export { SyncfusionCompleteDashboardComponent } from './syncfusion-complete-dashboard.component';

// Example usage documentation
export const CRM_EXAMPLES_ROUTES = [
  {
    path: 'admin/syncfusion-examples/client-grid',
    component: 'SyncfusionClientGridComponent',
    title: 'Client Grid Example',
    description: 'Advanced data grid with filtering, sorting, and CRUD operations for client management'
  },
  {
    path: 'admin/syncfusion-examples/opportunity-form',
    component: 'SyncfusionOpportunityFormComponent',
    title: 'Opportunity Form Example',
    description: 'Comprehensive form with various Syncfusion components for opportunity creation'
  },
  {
    path: 'admin/syncfusion-examples/simple-dashboard',
    component: 'SyncfusionSimpleDashboardComponent',
    title: 'Simple Dashboard Example',
    description: 'Basic dashboard with KPI cards and form component demonstrations'
  },
  {
    path: 'admin/syncfusion-examples/metrics-panel',
    component: 'SyncfusionMetricsPanelComponent',
    title: 'Metrics Panel Example',
    description: 'Modern metrics panel with gradient cards and responsive layout'
  },
  {
    path: 'admin/syncfusion-examples/chart-dashboard',
    component: 'SyncfusionChartDashboardComponent',
    title: 'Chart Dashboard Example',
    description: 'Advanced sales dashboard with interactive charts and KPI metrics'
  },
  {
    path: 'admin/syncfusion-examples/complete-dashboard',
    component: 'SyncfusionCompleteDashboardComponent',
    title: 'Complete Dashboard Example',
    description: 'Comprehensive CRM dashboard with metrics, charts, and analysis based on Syncfusion demos'
  }
];

// Integration guide
export const CRM_INTEGRATION_GUIDE = {
  clientGrid: {
    description: 'Perfect for listing clients, leads, or opportunities with advanced filtering',
    features: [
      'Pagination and virtual scrolling',
      'Column filtering and sorting',
      'Inline editing and batch updates',
      'Excel/PDF export functionality',
      'Responsive design for mobile'
    ],
    customizations: [
      'Add custom command buttons',
      'Implement row selection logic',
      'Add custom filter UI',
      'Integrate with ABP permissions',
      'Add audit trail functionality'
    ]
  },
  opportunityForm: {
    description: 'Ideal for complex forms with multiple input types and validation',
    features: [
      'Multi-step form wizard',
      'Dynamic field visibility',
      'Auto-calculation of fields',
      'File upload integration',
      'Rich text description fields'
    ],
    customizations: [
      'Add conditional validation',
      'Implement auto-save drafts',
      'Add form templates',
      'Integrate with file manager',
      'Add signature capture'
    ]
  },
  salesDashboard: {
    description: 'Complete dashboard solution with real-time data and interactive charts',
    features: [
      'Multiple chart types (line, bar, pie, funnel)',
      'Real-time KPI updates',
      'Drill-down capabilities',
      'Export charts as images',
      'Responsive layout system'
    ],
    customizations: [
      'Add custom chart types',
      'Implement data caching',
      'Add alert notifications',
      'Create custom widgets',
      'Add date range selectors'
    ]
  }
};