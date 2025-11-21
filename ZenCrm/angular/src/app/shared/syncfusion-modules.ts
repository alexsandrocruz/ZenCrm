// Import all Syncfusion modules here for centralized management
// These will be imported in components as needed

// Button Components
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { RadioButtonModule } from '@syncfusion/ej2-angular-buttons';
import { CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { SwitchModule } from '@syncfusion/ej2-angular-buttons';

// Input Components
import { TextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { NumericTextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { MaskedTextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { SliderModule } from '@syncfusion/ej2-angular-inputs';

// Dropdown Components
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { ComboBoxModule } from '@syncfusion/ej2-angular-dropdowns';
import { AutoCompleteModule } from '@syncfusion/ej2-angular-dropdowns';
import { MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';

// Calendar Components
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { TimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { CalendarModule } from '@syncfusion/ej2-angular-calendars';

// Grid Components
import { GridModule } from '@syncfusion/ej2-angular-grids';
import { PagerModule } from '@syncfusion/ej2-angular-grids';

// Popup Components
import { DialogModule } from '@syncfusion/ej2-angular-popups';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';

// Navigation Components
import { TabModule } from '@syncfusion/ej2-angular-navigations';
import { ToolbarModule as NavToolbarModule } from '@syncfusion/ej2-angular-navigations';
import { MenuModule } from '@syncfusion/ej2-angular-navigations';
import { ContextMenuModule } from '@syncfusion/ej2-angular-navigations';
import { TreeViewModule } from '@syncfusion/ej2-angular-navigations';

// Commonly used module groups for easy import
export const SyncfusionCommonModules = [
  ButtonModule,
  RadioButtonModule,
  CheckBoxModule,
  TextBoxModule,
  DropDownListModule,
  DatePickerModule,
  DialogModule,
  TooltipModule,
];

export const SyncfusionGridModules = [
  GridModule,
  PagerModule,
];

export const SyncfusionFormModules = [
  ButtonModule,
  RadioButtonModule,
  CheckBoxModule,
  SwitchModule,
  TextBoxModule,
  NumericTextBoxModule,
  MaskedTextBoxModule,
  DropDownListModule,
  ComboBoxModule,
  AutoCompleteModule,
  MultiSelectModule,
  DatePickerModule,
  DateTimePickerModule,
  TimePickerModule,
];