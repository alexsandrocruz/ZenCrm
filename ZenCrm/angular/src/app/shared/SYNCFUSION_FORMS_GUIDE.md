# Syncfusion Forms Guide - NG01203 Error Fix

## The NG01203 Error

The error `NG01203: No value accessor for form control unspecified name attribute` occurs when using Angular's `ngModel` with Syncfusion components without proper configuration.

## Solution: Use Correct Event Binding for Each Component

Syncfusion components have different ways of handling two-way binding. Some support `ngModel`, others need custom event handlers.

### ✅ Components That Support ngModel (Need Name Attribute)
```html
<ejs-textbox
  name="textBox"
  [(ngModel)]="textValue"
  placeholder="Enter text...">
</ejs-textbox>

<ejs-checkbox
  name="agreeTerms"
  label="I agree"
  [(ngModel)]="agreeTerms">
</ejs-checkbox>

<ejs-radiobutton
  name="gender"
  label="Male"
  [(ngModel)]="gender"
  value="male">
</ejs-radiobutton>
```

### ✅ Components That Need Event Binding
Some components don't support `ngModel` directly and need custom event handlers:

```html
<!-- NumericTextBox - Use value and change events -->
<ejs-numerictextbox
  [value]="numericValue"
  (change)="onNumericChange($event)"
  [min]="0"
  [max]="100">
</ejs-numerictextbox>

<!-- DatePicker - Use value and change events -->
<ejs-datepicker
  [value]="selectedDate"
  (change)="onDateChange($event)"
  format="dd/MM/yyyy">
</ejs-datepicker>

<!-- DropDownList - Use value and change events -->
<ejs-dropdownlist
  [value]="selectedCountry"
  (change)="onCountryChange($event)"
  [dataSource]="countries"
  [fields]="{ text: 'name', value: 'code' }">
</ejs-dropdownlist>

<!-- Switch - Use checked and change events -->
<ejs-switch
  [checked]="switchValue"
  (change)="onSwitchChange($event)">
</ejs-switch>
```

## Component TypeScript Example with Event Handlers

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionFormModules } from '../shared/syncfusion-modules';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ...SyncfusionFormModules],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <!-- Components that use ngModel -->
    <ejs-textbox name="firstName" [(ngModel)]="user.firstName"></ejs-textbox>
    <ejs-checkbox name="agreeTerms" [(ngModel)]="user.agreeTerms"></ejs-checkbox>

    <!-- Components that use event binding -->
    <ejs-numerictextbox [value]="user.age" (change)="onAgeChange($event)"></ejs-numerictextbox>
    <ejs-datepicker [value]="user.birthDate" (change)="onBirthDateChange($event)"></ejs-datepicker>
    <ejs-dropdownlist [value]="user.country" (change)="onCountryChange($event)"></ejs-dropdownlist>
    <ejs-switch [checked]="user.newsletter" (change)="onNewsletterChange($event)"></ejs-switch>
  `
})
export class UserFormComponent {
  user = {
    firstName: '',
    age: 0,
    birthDate: null as Date | null,
    country: '',
    agreeTerms: false,
    newsletter: false
  };

  // Event handlers for components that don't support ngModel
  onAgeChange(event: any): void {
    this.user.age = event.value;
  }

  onBirthDateChange(event: any): void {
    this.user.birthDate = event.value;
  }

  onCountryChange(event: any): void {
    this.user.country = event.value;
  }

  onNewsletterChange(event: any): void {
    this.user.newsletter = event.checked;
  }
}
```

## Component Reference

### Components That Support ngModel (need name attribute):
- ✅ `ejs-textbox`
- ✅ `ejs-checkbox`
- ✅ `ejs-radiobutton`

### Components That Need Event Binding:
- ❌→✅ `ejs-numerictextbox` - Use `[value]` and `(change)`
- ❌→✅ `ejs-datepicker` - Use `[value]` and `(change)`
- ❌→✅ `ejs-dropdownlist` - Use `[value]` and `(change)`
- ❌→✅ `ejs-switch` - Use `[checked]` and `(change)`

## Component TypeScript Example

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionFormModules } from '../shared/syncfusion-modules';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ...SyncfusionFormModules
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `<!-- Your form template here -->`
})
export class UserFormComponent {
  user = {
    firstName: '',
    age: 0,
    birthDate: null as Date | null,
    country: '',
    agreeTerms: false,
    gender: '',
    newsletter: false
  };

  countries = [
    { name: 'United States', code: 'US' },
    { name: 'Brazil', code: 'BR' },
    { name: 'United Kingdom', code: 'UK' }
  ];

  submitForm(): void {
    console.log('Form submitted:', this.user);
  }
}
```

## Special Cases

### 1. Radio Button Groups
For radio buttons, use the same `name` for the group and different `value` attributes:

```html
<ejs-radiobutton
  name="paymentMethod"  <!-- Same name for group -->
  [(ngModel)]="payment.method"
  value="credit">       <!-- Different values -->
</ejs-radiobutton>
<ejs-radiobutton
  name="paymentMethod"  <!-- Same name for group -->
  [(ngModel)]="payment.method"
  value="debit">        <!-- Different values -->
</ejs-radiobutton>
```

### 2. Array Forms (ngFor)
When using `ngFor`, make sure each item has a unique name:

```html
<div *ngFor="let item of items; let i = index">
  <ejs-textbox
    name="item{{i}}"  <!-- Unique name per iteration -->
    [(ngModel)]="item.name">
  </ejs-textbox>
</div>
```

### 3. Two-Way Binding Without Forms
If you're not using forms but just two-way binding, you still need the `name`:

```html
<ejs-textbox
  name="simpleValue"
  [(ngModel)]="simpleValue">
</ejs-textbox>
```

## Checklist for Syncfusion Forms

Before deploying or testing, verify:

- [ ] Every component with `[(ngModel)]` has a `name` attribute
- [ ] Radio buttons in the same group have the same `name`
- [ ] Radio buttons have different `value` attributes
- [ ] Dynamic forms (ngFor) use unique names with indices
- [ ] `CUSTOM_ELEMENTS_SCHEMA` is included in component schemas
- [ ] Required Syncfusion modules are imported in `imports` array

## Common Mistakes to Avoid

1. **Forgetting the name attribute** - Most common cause of NG01203
2. **Using the same name for different form fields** - Can cause binding conflicts
3. **Missing CUSTOM_ELEMENTS_SCHEMA** - Required for Syncfusion web components
4. **Not importing the required Syncfusion modules** - Components won't be recognized
5. **Using `checked` instead of `[checked]`** - For boolean properties, use property binding

## Debugging Tips

1. **Check the console** - NG01203 errors are clearly logged
2. **Verify component imports** - Ensure Syncfusion modules are imported
3. **Check syntax** - Make sure all brackets and quotes are correct
4. **Test incrementally** - Add components one by one to isolate issues
5. **Use browser dev tools** - Inspect elements to verify they render correctly

Following this guide will ensure your Syncfusion forms work without NG01203 errors.