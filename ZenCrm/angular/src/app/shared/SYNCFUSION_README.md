# Syncfusion Integration Guide

This document explains how to use Syncfusion components in the ZenCrm Angular application.

## Installation

The following Syncfusion packages have been installed:

- `@syncfusion/ej2-angular-base` - Core functionality
- `@syncfusion/ej2-angular-buttons` - Button components
- `@syncfusion/ej2-angular-popups` - Popup components (Dialog, Tooltip)
- `@syncfusion/ej2-angular-grids` - Data Grid components
- `@syncfusion/ej2-angular-inputs` - Input components
- `@syncfusion/ej2-angular-calendars` - Calendar and date components
- `@syncfusion/ej2-angular-dropdowns` - Dropdown components
- `@syncfusion/ej2-angular-navigations` - Navigation components

## License Configuration

A valid Syncfusion license has been configured and registered in the application:

- **License Key**: Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5ccXRcQ2ddV0NwVkFWYEw=
- **Registration**: Automatic on application startup in `main.ts`
- **Benefits**: No watermarks, full functionality, commercial use permitted

### License Status Page

Access `/admin/syncfusion-license` to view the current license status and test component functionality without watermarks.

## Styling

Syncfusion Material Design styles have been configured in `angular.json`:

- `ej2-base-material.css`
- `ej2-buttons-material.css`
- `ej2-popups-material.css`
- `ej2-splitbuttons-material.css`

## Usage in Components

### Option 1: Using Module Groups (Recommended)

Import the pre-configured module groups in your standalone components:

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionCommonModules, SyncfusionFormModules, SyncfusionGridModules } from '../shared/syncfusion-modules';

@Component({
  selector: 'app-your-component',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ...SyncfusionCommonModules  // Basic components like buttons, inputs
    // or ...SyncfusionFormModules  // All form-related components
    // or ...SyncfusionGridModules  // Grid-related components
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA], // Required for Syncfusion web components
  template: `
    <!-- Your template with Syncfusion components -->
  `
})
export class YourComponent { }
```

### Option 2: Import Individual Modules

```typescript
import { ButtonModule, TextBoxModule, DropDownListModule } from '../shared/syncfusion-modules';

@Component({
  // ...
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    TextBoxModule,
    DropDownListModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  // ...
})
```

## Available Module Groups

### SyncfusionCommonModules
Essential components for most use cases:
- Buttons (Button, RadioButton, CheckBox)
- Inputs (TextBox)
- Dropdowns (DropDownList)
- Calendars (DatePicker)
- Popups (Dialog, Tooltip)

### SyncfusionFormModules
All form-related components:
- All CommonModules components
- Switch, NumericTextBox, MaskedTextBox
- ComboBox, AutoComplete, MultiSelect
- DateTimePicker, TimePicker, Calendar

### SyncfusionGridModules
Grid-related components:
- GridModule
- PagerModule

## Component Examples

### Buttons
```html
<ejs-button content="Primary Button" cssClass="e-primary"></ejs-button>
<ejs-button content="Success Button" cssClass="e-success"></ejs-button>
<ejs-button content="Toggle Button" isToggle="true"></ejs-button>
```

### Input Fields
```html
<ejs-textbox
  [(ngModel)]="textValue"
  placeholder="Enter text..."
  floatLabelType="Auto">
</ejs-textbox>

<ejs-numerictextbox
  [(ngModel)]="numericValue"
  [min]="0"
  [max]="100"
  format="n2"
  floatLabelType="Auto">
</ejs-numerictextbox>
```

### Date Picker
```html
<ejs-datepicker
  [(ngModel)]="selectedDate"
  format="dd/MM/yyyy"
  placeholder="Select date..."
  floatLabelType="Auto">
</ejs-datepicker>
```

### Dropdown
```html
<ejs-dropdownlist
  [(ngModel)]="selectedValue"
  [dataSource]="data"
  [fields]="{ text: 'name', value: 'id' }"
  placeholder="Select item..."
  floatLabelType="Auto">
</ejs-dropdownlist>
```

### Dialog
```html
<ejs-dialog
  #dialog
  [visible]="dialogVisible"
  [header]="dialogTitle"
  [isModal]="true"
  (close)="onDialogClose()">
  <div>Dialog content here</div>
</ejs-dialog>
```

## Demo Component

A complete demonstration of Syncfusion components is available at:
- Route: `/admin/syncfusion-demo`
- Component: `src/app/shared/syncfusion-demo.component.ts`

Access this component to see all available components in action and copy working examples.

## Important Notes

1. **Always include `CUSTOM_ELEMENTS_SCHEMA`** in component schemas when using Syncfusion components
2. **Use Material Design CSS classes** for consistency with the application theme
3. **Syncfusion components use `ejs-` prefix** in HTML (e.g., `<ejs-button>`)
4. **Two-way binding works with `[(ngModel)]`** for most input components
5. **Use `floatLabelType="Auto"`** for modern floating label behavior

## Adding New Syncfusion Components

To add new Syncfusion components:

1. Install the required package: `npm install @syncfusion/ej2-angular-<component-name>`
2. Add the CSS to `angular.json` if needed
3. Import the module in `syncfusion-modules.ts`
4. Add to appropriate module group or create a new group
5. Update component imports as needed

## Troubleshooting

- **Build Errors**: Ensure `CUSTOM_ELEMENTS_SCHEMA` is included in component schemas
- **Missing Styles**: Check that CSS files are properly configured in `angular.json`
- **Import Errors**: Verify module names in `syncfusion-modules.ts` match the installed packages
- **Component Not Found**: Ensure the component module is imported in your component's `imports` array