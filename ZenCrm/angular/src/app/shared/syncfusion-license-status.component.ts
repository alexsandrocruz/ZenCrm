import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { validateSyncfusionLicense, SYNCUSION_LICENSE_KEY } from './syncfusion-license';
import { SyncfusionCommonModules } from './syncfusion-modules';

@Component({
  selector: 'app-syncfusion-license-status',
  standalone: true,
  imports: [CommonModule, ...SyncfusionCommonModules],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="container-fluid mt-4">
      <div class="row">
        <div class="col-md-12">
          <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
              <h5 class="mb-0">
                <i class="fas fa-key me-2"></i>
                Syncfusion License Status
              </h5>
              <span [class]="licenseStatusClass">
                <i [class]="licenseIconClass" class="me-2"></i>
                {{ licenseStatusText }}
              </span>
            </div>
            <div class="card-body">
              <div class="row">
                <div class="col-md-6">
                  <h6>License Information</h6>
                  <table class="table table-sm">
                    <tr>
                      <td><strong>License Key:</strong></td>
                      <td>
                        <code>{{ formattedLicenseKey }}</code>
                      </td>
                    </tr>
                    <tr>
                      <td><strong>Status:</strong></td>
                      <td>
                        <span [class]="licenseStatusClass">
                          <i [class]="licenseIconClass" class="me-1"></i>
                          {{ licenseStatusText }}
                        </span>
                      </td>
                    </tr>
                    <tr>
                      <td><strong>Validation Date:</strong></td>
                      <td>{{ validationDate | date:'medium' }}</td>
                    </tr>
                  </table>
                </div>
                <div class="col-md-6">
                  <h6>License Benefits</h6>
                  <ul class="list-unstyled">
                    <li *ngIf="licenseValid">
                      <i class="fas fa-check text-success me-2"></i>
                      No watermarks on components
                    </li>
                    <li *ngIf="licenseValid">
                      <i class="fas fa-check text-success me-2"></i>
                      Full component functionality
                    </li>
                    <li *ngIf="licenseValid">
                      <i class="fas fa-check text-success me-2"></i>
                      Commercial use permitted
                    </li>
                    <li *ngIf="!licenseValid">
                      <i class="fas fa-times text-danger me-2"></i>
                      Watermarks will be visible
                    </li>
                    <li *ngIf="!licenseValid">
                      <i class="fas fa-times text-danger me-2"></i>
                      Limited functionality
                    </li>
                  </ul>
                </div>
              </div>

              <div class="row mt-4">
                <div class="col-md-12">
                  <h6>Test Components</h6>
                  <p class="text-muted">
                    The buttons below should not display "Trial" or "Evaluation" watermarks when the license is valid:
                  </p>
                  <div class="d-flex gap-2 flex-wrap">
                    <ejs-button content="Primary Button" cssClass="e-primary"></ejs-button>
                    <ejs-button content="Success Button" cssClass="e-success"></ejs-button>
                    <ejs-button content="Info Button" cssClass="e-info"></ejs-button>
                    <ejs-button content="Warning Button" cssClass="e-warning"></ejs-button>
                    <ejs-button content="Danger Button" cssClass="e-danger"></ejs-button>
                  </div>
                </div>
              </div>

              <div class="row mt-4">
                <div class="col-md-12">
                  <div class="alert" [class]="licenseAlertClass" role="alert">
                    <i [class]="licenseAlertIcon" class="me-2"></i>
                    {{ licenseAlertMessage }}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .card {
      box-shadow: 0 0 15px rgba(0,0,0,0.1);
    }

    .card-header {
      background-color: #f8f9fa;
      border-bottom: 1px solid #dee2e6;
    }

    code {
      background-color: #f8f9fa;
      padding: 2px 4px;
      border-radius: 3px;
      font-size: 0.875em;
    }

    .table td {
      border-top: 1px solid #f8f9fa;
    }

    .list-unstyled li {
      padding: 4px 0;
    }

    .badge-success {
      background-color: #28a745;
    }

    .badge-danger {
      background-color: #dc3545;
    }
  `]
})
export class SyncfusionLicenseStatusComponent implements OnInit {
  licenseValid = false;
  validationDate = new Date();
  licenseKey = SYNCUSION_LICENSE_KEY;

  get formattedLicenseKey(): string {
    if (!this.licenseKey) return 'Not configured';
    return this.licenseKey.substring(0, 20) + '...';
  }

  get licenseStatusText(): string {
    return this.licenseValid ? 'Valid' : 'Invalid';
  }

  get licenseStatusClass(): string {
    return this.licenseValid ? 'badge bg-success' : 'badge bg-danger';
  }

  get licenseIconClass(): string {
    return this.licenseValid ? 'fas fa-check-circle' : 'fas fa-exclamation-triangle';
  }

  get licenseAlertClass(): string {
    return this.licenseValid ? 'alert alert-success' : 'alert alert-warning';
  }

  get licenseAlertIcon(): string {
    return this.licenseValid ? 'fas fa-check-circle' : 'fas fa-exclamation-triangle';
  }

  get licenseAlertMessage(): string {
    if (this.licenseValid) {
      return 'Syncfusion license is successfully registered and active. All components should work without watermarks.';
    } else {
      return 'Syncfusion license is not valid or not properly registered. Components may display evaluation watermarks.';
    }
  }

  ngOnInit(): void {
    this.validateLicense();
  }

  validateLicense(): void {
    try {
      this.licenseValid = validateSyncfusionLicense();
    } catch (error) {
      console.error('License validation error:', error);
      this.licenseValid = false;
    }
  }
}