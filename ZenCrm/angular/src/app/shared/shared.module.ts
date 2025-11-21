import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Import Syncfusion modules groups
import {
  SyncfusionCommonModules,
  SyncfusionGridModules,
  SyncfusionFormModules
} from './syncfusion-modules';

// Import demo component
import { SyncfusionDemoComponent } from './syncfusion-demo.component';

@NgModule({
  declarations: [
    SyncfusionDemoComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    ...SyncfusionCommonModules,
    ...SyncfusionGridModules,
    ...SyncfusionFormModules
  ],
  exports: [
    CommonModule,
    FormsModule,
    RouterModule,
    ...SyncfusionCommonModules,
    ...SyncfusionGridModules,
    ...SyncfusionFormModules,
    SyncfusionDemoComponent
  ]
})
export class SharedModule { }