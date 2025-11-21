// Extended Syncfusion modules for CRM-specific components

// Chart Components
import { ChartModule } from '@syncfusion/ej2-angular-charts';
import { AccumulationChartModule } from '@syncfusion/ej2-angular-charts';
import { RangeNavigatorModule } from '@syncfusion/ej2-angular-charts';
import { SmithChartModule } from '@syncfusion/ej2-angular-charts';
import { SparklineModule } from '@syncfusion/ej2-angular-charts';
import { TreeMapModule } from '@syncfusion/ej2-angular-charts';
import { BulletChartModule } from '@syncfusion/ej2-angular-charts';
import { CirculargaugeModule, LineargaugeModule } from '@syncfusion/ej2-angular-circulargauge';

// Schedule Components
import { ScheduleModule } from '@syncfusion/ej2-angular-schedule';

// Kanban Components
import { KanbanModule } from '@syncfusion/ej2-angular-kanban';

// File Management
import { FileManagerModule } from '@syncfusion/ej2-angular-filemanager';
import { PdfViewerModule } from '@syncfusion/ej2-angular-pdfviewer';

// Advanced Components
import { DiagramModule } from '@syncfusion/ej2-angular-diagrams';
import { GanttModule } from '@syncfusion/ej2-angular-gantt';

// Rich Text Editor
import { RichTextEditorModule } from '@syncfusion/ej2-angular-richtexteditor';

// ListView for custom lists
import { ListViewModule } from '@syncfusion/ej2-angular-lists';

// Export module groups
export const SyncfusionChartModules = [
  ChartModule,
  AccumulationChartModule,
  RangeNavigatorModule,
  SmithChartModule,
  SparklineModule,
  TreeMapModule,
  BulletChartModule,
  CirculargaugeModule,
  LineargaugeModule
];

export const SyncfusionDashboardModules = [
  ...SyncfusionChartModules,
  RichTextEditorModule,
  ListViewModule
];

export const SyncfusionAdvancedModules = [
  KanbanModule,
  ScheduleModule,
  FileManagerModule,
  PdfViewerModule,
  DiagramModule,
  GanttModule,
  RichTextEditorModule
];

export const SyncfusionCalendarModules = [
  ScheduleModule,
  DatePickerModule,
  DateTimePickerModule,
  TimePickerModule
];

// Complete CRM module bundle
export const SyncfusionCrmComplete = [
  // Base form modules (from syncfusion-modules.ts)
  ...SyncfusionCommonModules,
  ...SyncfusionFormModules,
  ...SyncfusionGridModules,

  // Advanced modules
  ...SyncfusionChartModules,
  ...SyncfusionDashboardModules,
  ...SyncfusionAdvancedModules
];

// Export individual modules for specific use cases
export {
  ChartModule,
  AccumulationChartModule,
  RangeNavigatorModule,
  SmithChartModule,
  SparklineModule,
  TreeMapModule,
  BulletChartModule,
  CirculargaugeModule,
  LineargaugeModule,
  ScheduleModule,
  KanbanModule,
  FileManagerModule,
  PdfViewerModule,
  DiagramModule,
  GanttModule,
  RichTextEditorModule,
  ListViewModule
};