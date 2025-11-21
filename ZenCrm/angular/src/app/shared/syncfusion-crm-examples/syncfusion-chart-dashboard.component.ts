import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ButtonModule, CheckBoxComponent } from '@syncfusion/ej2-angular-buttons';
import { DropDownListAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { ChartAllModule, ChartComponent } from '@syncfusion/ej2-angular-charts';

@Component({
  selector: 'app-syncfusion-chart-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ButtonModule,
    DropDownListAllModule,
    ChartAllModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="container-fluid mt-4">
      <div class="row mb-4">
        <div class="col-md-12">
          <h2>
            <i class="fas fa-chart-bar me-2"></i>
            CRM Sales Dashboard
          </h2>
        </div>
      </div>

      <!-- Header Section -->
      <div class="card mb-4">
        <div class="card-body">
          <div class="row align-items-center">
            <div class="col-md-6">
              <h4 class="mb-1">Sales Performance Analysis</h4>
              <p class="text-muted mb-0">Period: <span class="fw-medium">2024 Annual Overview</span></p>
            </div>
            <div class="col-md-6 text-end">
              <ejs-button
                class="e-outline me-2"
                content="Export Report"
                (click)="exportReport()">
              </ejs-button>
              <ejs-button
                class="e-primary"
                content="Refresh Data"
                (click)="refreshData()">
              </ejs-button>
            </div>
          </div>
        </div>
      </div>

      <!-- Chart Controls -->
      <div class="card mb-4">
        <div class="card-body">
          <div class="row align-items-center">
            <div class="col-md-3">
              <p class="mb-2 fw-bold">Select Period</p>
              <div class="d-flex gap-2 mb-3 mb-md-0">
                <ejs-button
                  class="e-outline"
                  content="M"
                  [cssClass]="selectedPeriod === 'M' ? 'e-primary' : ''"
                  (click)="selectPeriod('M')">
                </ejs-button>
                <ejs-button
                  class="e-outline"
                  content="Q"
                  [cssClass]="selectedPeriod === 'Q' ? 'e-primary' : ''"
                  (click)="selectPeriod('Q')">
                </ejs-button>
                <ejs-button
                  class="e-outline"
                  content="Y"
                  [cssClass]="selectedPeriod === 'Y' ? 'e-primary' : ''"
                  (click)="selectPeriod('Y')">
                </ejs-button>
              </div>
            </div>
            <div class="col-md-3">
              <p class="mb-2 fw-bold">Metric Type</p>
              <ejs-dropdownlist
                [(value)]="selectedMetric"
                (change)="onMetricChange($event)"
                [dataSource]="metricOptions"
                [fields]="{ text: 'text', value: 'value' }"
                placeholder="Select metric">
              </ejs-dropdownlist>
            </div>
            <div class="col-md-3">
              <p class="mb-2 fw-bold">Chart Type</p>
              <ejs-dropdownlist
                [(value)]="selectedChartType"
                (change)="onChartTypeChange($event)"
                [dataSource]="chartTypeOptions"
                [fields]="{ text: 'text', value: 'value' }"
                placeholder="Select chart type">
              </ejs-dropdownlist>
            </div>
            <div class="col-md-3">
              <div class="d-flex align-items-center h-100">
                <ejs-checkbox
                  label="Show Target"
                  [(checked)]="showTarget"
                  (change)="updateChart()">
                </ejs-checkbox>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Key Metrics -->
      <div class="row mb-4">
        <div class="col-md-3">
          <div class="card bg-primary text-white">
            <div class="card-body">
              <h6 class="card-title">Total Revenue</h6>
              <h3>{{ formatCurrency(totalRevenue) }}</h3>
              <small>{{ revenueChange > 0 ? '+' : '' }}{{ revenueChange.toFixed(1) }}% from last period</small>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card bg-success text-white">
            <div class="card-body">
              <h6 class="card-title">Deals Won</h6>
              <h3>{{ dealsWon }}</h3>
              <small>{{ dealsChange > 0 ? '+' : '' }}{{ dealsChange }} deals this period</small>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card bg-warning text-white">
            <div class="card-body">
              <h6 class="card-title">Pipeline Value</h6>
              <h3>{{ formatCurrency(pipelineValue) }}</h3>
              <small>{{ pipelineChange > 0 ? '+' : '' }}{{ pipelineChange.toFixed(1) }}% growth</small>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card bg-info text-white">
            <div class="card-body">
              <h6 class="card-title">Win Rate</h6>
              <h3>{{ winRate }}%</h3>
              <small>{{ winRateChange > 0 ? '+' : '' }}{{ winRateChange.toFixed(1) }}% improvement</small>
            </div>
          </div>
        </div>
      </div>

      <!-- Main Chart -->
      <div class="card">
        <div class="card-body">
          <h5 class="card-title mb-4">Performance Overview</h5>
          <ejs-chart
            #chart
            width="100%"
            height="400px"
            [chartArea]="{border: {width: 0}}"
            [primaryXAxis]="primaryXAxis"
            [primaryYAxis]="primaryYAxis"
            [crosshair]="crosshair"
            [tooltip]="tooltip"
            [legendSettings]="legendSettings"
            (load)="chartLoad($event)">

            <e-series-collection>
              <!-- Actual Performance -->
              <e-series
                [dataSource]="chartData"
                [type]="selectedChartType"
                xName="period"
                yName="actual"
                name="Actual"
                fill="#267DDA"
                [cornerRadius]="{ bottomLeft: 4, bottomRight: 4, topLeft: 4, topRight: 4 }"
                columnSpacing="0.1">
              </e-series>

              <!-- Target Line (if enabled) -->
              <e-series
                *ngIf="showTarget"
                [dataSource]="targetData"
                type="Line"
                xName="period"
                yName="target"
                name="Target"
                fill="#FF6B6B"
                width="3"
                marker="{ visible: true, width: 8, height: 8, shape: 'Circle', fill: '#FF6B6B' }">
              </e-series>
            </e-series-collection>
          </ejs-chart>
        </div>
      </div>

      <!-- Additional Stats -->
      <div class="row mt-4">
        <div class="col-md-6">
          <div class="card">
            <div class="card-body">
              <h6 class="card-title">Performance Highlights</h6>
              <ul class="list-unstyled">
                <li class="mb-2">
                  <i class="fas fa-arrow-up text-success me-2"></i>
                  Best month: {{ bestMonth }} ({{ formatCurrency(bestMonthValue) }})
                </li>
                <li class="mb-2">
                  <i class="fas fa-arrow-down text-danger me-2"></i>
                  Improvement needed: {{ worstMonth }}
                </li>
                <li class="mb-2">
                  <i class="fas fa-chart-line text-info me-2"></i>
                  Average deal size: {{ formatCurrency(averageDealSize) }}
                </li>
                <li>
                  <i class="fas fa-trophy text-warning me-2"></i>
                  Top performer: {{ topPerformer }}
                </li>
              </ul>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="card">
            <div class="card-body">
              <h6 class="card-title">Conversion Metrics</h6>
              <div class="row text-center">
                <div class="col-4">
                  <h4 class="text-primary">{{ conversionRate.leadToOpp }}%</h4>
                  <small>Lead → Opp</small>
                </div>
                <div class="col-4">
                  <h4 class="text-success">{{ conversionRate.oppToDeal }}%</h4>
                  <small>Opp → Deal</small>
                </div>
                <div class="col-4">
                  <h4 class="text-info">{{ conversionRate.overall }}%</h4>
                  <small>Overall</small>
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
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      border: none;
      margin-bottom: 1rem;
    }

    .card-header {
      background-color: #f8f9fa;
      border-bottom: 1px solid #dee2e6;
    }

    .bg-primary { background-color: #007bff !important; }
    .bg-success { background-color: #28a745 !important; }
    .bg-warning { background-color: #ffc107 !important; }
    .bg-info { background-color: #17a2b8 !important; }

    .text-primary { color: #007bff !important; }
    .text-success { color: #28a745 !important; }
    .text-warning { color: #ffc107 !important; }
    .text-info { color: #17a2b8 !important; }
    .text-danger { color: #dc3545 !important; }

    .list-unstyled {
      list-style: none;
      padding-left: 0;
    }

    @media (max-width: 768px) {
      .text-end {
        text-align: left !important;
        margin-top: 1rem;
      }

      .card-body {
        padding: 1rem;
      }
    }
  `]
})
export class SyncfusionChartDashboardComponent implements OnInit {
  @ViewChild('chart') public chart!: ChartComponent;

  // Form controls
  selectedPeriod = 'Y';
  selectedMetric = 'revenue';
  selectedChartType = 'Column';
  showTarget = false;

  // Metrics data
  totalRevenue = 2450000;
  revenueChange = 12.5;
  dealsWon = 48;
  dealsChange = 8;
  pipelineValue = 8700000;
  pipelineChange = 23.0;
  winRate = 72;
  winRateChange = 5.2;

  // Chart data
  chartData: any[] = [];
  targetData: any[] = [];

  // Options
  metricOptions = [
    { text: 'Revenue', value: 'revenue' },
    { text: 'Deals', value: 'deals' },
    { text: 'Pipeline Value', value: 'pipeline' },
    { text: 'Activities', value: 'activities' }
  ];

  chartTypeOptions = [
    { text: 'Column', value: 'Column' },
    { text: 'Line', value: 'Line' },
    { text: 'Area', value: 'Area' },
    { text: 'Bar', value: 'Bar' }
  ];

  // Chart configuration
  primaryXAxis: any = {
    valueType: 'Category',
    majorGridLines: { width: 0 },
    minorTickLines: { width: 0 },
    labelIntersectAction: 'None',
    labelStyle: { fontWeight: '500' },
    interval: 1
  };

  primaryYAxis: any = {
    minimum: 0,
    interval: 100000,
    edgeLabelPlacement: 'Shift',
    lineStyle: { width: 0 },
    labelStyle: { fontWeight: '500' },
    majorTickLines: { width: 0 },
    labelFormat: '${value}'
  };

  crosshair: any = {
    enable: true,
    lineType: 'Vertical',
    highlightCategory: true,
    verticalLineColor: '#267DDA',
    opacity: 0.5
  };

  tooltip: any = {
    enable: true,
    format: '${series.name}: <b>${point.y}</b>',
    header: '${point.x}'
  };

  legendSettings: any = {
    visible: true,
    position: 'Top'
  };

  // Additional stats
  bestMonth = 'March';
  bestMonthValue = 320000;
  worstMonth = 'January';
  averageDealSize = 51000;
  topPerformer = 'Sarah Johnson';

  conversionRate = {
    leadToOpp: 35,
    oppToDeal: 28,
    overall: 9.8
  };

  constructor() { }

  ngOnInit(): void {
    this.loadChartData();
  }

  loadChartData(): void {
    // Generate sample data based on selected metric
    if (this.selectedMetric === 'revenue') {
      this.chartData = [
        { period: 'Jan', actual: 180000 },
        { period: 'Feb', actual: 220000 },
        { period: 'Mar', actual: 320000 },
        { period: 'Apr', actual: 280000 },
        { period: 'May', actual: 290000 },
        { period: 'Jun', actual: 310000 },
        { period: 'Jul', actual: 260000 },
        { period: 'Aug', actual: 240000 },
        { period: 'Sep', actual: 270000 },
        { period: 'Oct', actual: 300000 },
        { period: 'Nov', actual: 330000 },
        { period: 'Dec', actual: 350000 }
      ];

      this.targetData = [
        { period: 'Jan', target: 200000 },
        { period: 'Feb', target: 210000 },
        { period: 'Mar', target: 250000 },
        { period: 'Apr', target: 270000 },
        { period: 'May', target: 280000 },
        { period: 'Jun', target: 290000 },
        { period: 'Jul', target: 275000 },
        { period: 'Aug', actual: 265000 },
        { period: 'Sep', actual: 280000 },
        { period: 'Oct', actual: 295000 },
        { period: 'Nov', actual: 310000 },
        { period: 'Dec', actual: 320000 }
      ];
    } else if (this.selectedMetric === 'deals') {
      this.chartData = [
        { period: 'Jan', actual: 12 },
        { period: 'Feb', actual: 15 },
        { period: 'Mar', actual: 22 },
        { period: 'Apr', actual: 18 },
        { period: 'May', actual: 20 },
        { period: 'Jun', actual: 21 },
        { period: 'Jul', actual: 17 },
        { period: 'Aug', actual: 16 },
        { period: 'Sep', actual: 19 },
        { period: 'Oct', actual: 20 },
        { period: 'Nov', actual: 23 },
        { period: 'Dec', actual: 25 }
      ];

      this.primaryYAxis = {
        ...this.primaryYAxis,
        interval: 5,
        labelFormat: '${value}'
      };
    }
  }

  selectPeriod(period: string): void {
    this.selectedPeriod = period;
    this.updateChart();
  }

  onMetricChange(event: any): void {
    this.selectedMetric = event.value;
    this.loadChartData();
    this.updateChart();
  }

  onChartTypeChange(event: any): void {
    this.selectedChartType = event.value;
    this.updateChart();
  }

  updateChart(): void {
    // Refresh chart when settings change
    setTimeout(() => {
      if (this.chart) {
        this.chart.refresh();
      }
    }, 100);
  }

  exportReport(): void {
    alert('Export functionality would be implemented here');
  }

  refreshData(): void {
    this.loadChartData();
    this.updateChart();
    // Show success message
    console.log('Data refreshed successfully');
  }

  chartLoad(args: any): void {
    // Set theme based on system preference or user setting
    args.chart.theme = 'Material';
  }

  @HostListener('window:resize')
  onResize(): void {
    if (this.chart) {
      this.chart.refresh();
    }
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(value);
  }
}