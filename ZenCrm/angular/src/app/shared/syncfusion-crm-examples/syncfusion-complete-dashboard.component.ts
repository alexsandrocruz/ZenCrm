import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { ChartAllModule, ChartComponent } from '@syncfusion/ej2-angular-charts';

@Component({
  selector: 'app-syncfusion-complete-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    DropDownListAllModule,
    ChartAllModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <section class="bg-white dark:bg-gray-950 min-h-screen p-4 sm:p-6">
        <div class="w-full" style="max-width: 1200px;">

            <!-- Header Section -->
            <div class="flex justify-between items-start mb-6">
                <div>
                    <h1 class="text-xl font-semibold text-gray-900 dark:text-white mb-2">CRM Sales Dashboard</h1>
                    <p class="text-sm text-gray-500 dark:text-gray-400">Period: <span class="font-medium">2024 Annual Overview</span></p>
                </div>
                <div class="flex gap-2">
                    <button ejs-button class="e-outline hidden sm:block" content="Export Report" type="button" (click)="exportReport()"></button>
                    <button ejs-button class="e-outline block sm:hidden" iconCss="e-icons e-download" type="button" (click)="exportReport()"></button>
                </div>
            </div>

            <!-- Period Selection and Metrics -->
            <div class="mb-6 text-gray-900 dark:text-white">
                <p class="text-xs hidden sm:block mb-2">Select Period</p>
                <div class="flex justify-between flex-col-reverse gap-6 sm:flex-row sm:items-center">
                    <div>
                        <p class="text-xs block sm:hidden mb-2">Current Period</p>
                        <div class="flex gap-3 items-end">
                            <h1 class="text-2xl font-semibold">{{ formatCurrency(currentPeriodRevenue) }}</h1>
                            <p class="text-xs mb-1 text-gray-500">Total Revenue</p>
                        </div>
                    </div>
                    <div class="flex items-center justify-between gap-3">
                        <div class="e-btn-group hidden sm:block">
                            <input type="radio" id="month" name="period" value="M" aria-label="month" role="button" [(ngModel)]="selectedPeriod" (change)="updateData()"/>
                            <label class="e-btn" for="month">M</label>
                            <input type="radio" id="quarter" name="period" value="Q" aria-label="quarter" role="button" [(ngModel)]="selectedPeriod" (change)="updateData()"/>
                            <label class="e-btn" for="quarter">Q</label>
                            <input type="radio" id="year" name="period" value="Y" checked aria-label="year" role="button" [(ngModel)]="selectedPeriod" (change)="updateData()"/>
                            <label class="e-btn" for="year">Y</label>
                            <input type="radio" id="custom" name="period" value="Custom" aria-label="custom" role="button" [(ngModel)]="selectedPeriod" (change)="updateData()"/>
                            <label class="e-btn" for="custom">Custom</label>
                        </div>
                        <button ejs-dropdownbutton class="e-outline block sm:hidden" content="Year" [items]="periodOptions" (select)="onPeriodSelect($event)" type="button"></button>
                        <button ejs-dropdownbutton class="e-outline" content="Revenue" [items]="metricOptions" (select)="onMetricSelect($event)" type="button"></button>
                    </div>
                </div>
            </div>

            <!-- Key Metrics Cards -->
            <div class="flex flex-wrap items-center gap-3 text-gray-500 text-xs mb-6 dark:text-white">
                <div class="flex flex-col sm:flex-row sm:items-center sm:gap-1">
                    <p>Record revenue:</p>
                    <p class="font-medium dark:text-white sm:ml-1">{{ formatCurrency(recordRevenue) }}</p>
                </div>
                <div class="border-l h-6 border-gray-200 dark:border-gray-600"></div>
                <div class="flex flex-col sm:flex-row sm:items-center sm:gap-1">
                    <p>Avg deal size:</p>
                    <p class="font-medium dark:text-white sm:ml-1">{{ formatCurrency(averageDealSize) }}</p>
                </div>
                <div class="border-l h-6 border-gray-200 dark:border-gray-600"></div>
                <div class="flex flex-col sm:flex-row sm:items-center sm:gap-1">
                    <p>Win rate:</p>
                    <p class="font-medium dark:text-white sm:ml-1">{{ winRate }}%</p>
                </div>
            </div>

            <!-- Metrics Cards Grid -->
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
                <div class="bg-gradient-to-r from-blue-500 to-blue-600 text-white rounded-lg p-4">
                    <h6 class="text-sm opacity-90">Total Revenue</h6>
                    <h3 class="text-xl font-bold">{{ formatCurrency(totalRevenue) }}</h3>
                    <p class="text-xs mt-1">{{ revenueChange > 0 ? '+' : '' }}{{ revenueChange }}% from last period</p>
                </div>
                <div class="bg-gradient-to-r from-green-500 to-green-600 text-white rounded-lg p-4">
                    <h6 class="text-sm opacity-90">Deals Won</h6>
                    <h3 class="text-xl font-bold">{{ dealsWon }}</h3>
                    <p class="text-xs mt-1">{{ dealsChange > 0 ? '+' : '' }}{{ dealsChange }} deals this period</p>
                </div>
                <div class="bg-gradient-to-r from-orange-500 to-orange-600 text-white rounded-lg p-4">
                    <h6 class="text-sm opacity-90">Pipeline Value</h6>
                    <h3 class="text-xl font-bold">{{ formatCurrency(pipelineValue) }}</h3>
                    <p class="text-xs mt-1">{{ pipelineChange > 0 ? '+' : '' }}{{ pipelineChange }}% growth</p>
                </div>
                <div class="bg-gradient-to-r from-purple-500 to-purple-600 text-white rounded-lg p-4">
                    <h6 class="text-sm opacity-90">Active Leads</h6>
                    <h3 class="text-xl font-bold">{{ activeLeads }}</h3>
                    <p class="text-xs mt-1">{{ leadsChange > 0 ? '+' : '' }}{{ leadsChange }} new leads</p>
                </div>
            </div>

            <!-- Main Chart -->
            <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700">
                <div class="p-4">
                    <h5 class="text-lg font-semibold text-gray-900 dark:text-white mb-4">Performance Analysis</h5>
                    <ejs-chart
                        #chart
                        [chartArea]="{border: {width: 0}}"
                        width="100%"
                        height="350px"
                        [primaryXAxis]="primaryXAxis"
                        [primaryYAxis]="primaryYAxis"
                        [crosshair]="crosshair"
                        [tooltip]="tooltip"
                        [legendSettings]="legendSettings"
                        (load)="chartLoad($event)"
                        aria-label="sales performance analysis"
                        role="region">

                        <e-series-collection>
                            <e-series
                                [dataSource]="chartData"
                                type="Column"
                                xName="month"
                                yName="revenue"
                                name="Revenue"
                                fill="#267DDA"
                                [cornerRadius]="{ bottomLeft: 4, bottomRight: 4, topLeft: 4, topRight: 4 }"
                                columnSpacing="0.1">
                            </e-series>
                            <e-series
                                [dataSource]="chartData"
                                type="Line"
                                xName="month"
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

            <!-- Additional Charts Row -->
            <div class="grid grid-cols-1 lg:grid-cols-2 gap-6 mt-6">
                <!-- Conversion Funnel Chart -->
                <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700">
                    <div class="p-4">
                        <h5 class="text-lg font-semibold text-gray-900 dark:text-white mb-4">Sales Pipeline</h5>
                        <ejs-chart
                            width="100%"
                            height="250px"
                            [primaryXAxis]="funnelXAxis"
                            [primaryYAxis]="funnelYAxis"
                            [tooltip]="tooltip">

                            <e-series-collection>
                                <e-series
                                    [dataSource]="pipelineData"
                                    type="Funnel"
                                    xName="stage"
                                    yName="value"
                                    name="Pipeline"
                                    fill="#4CAF50">
                                </e-series>
                            </e-series-collection>
                        </ejs-chart>
                    </div>
                </div>

                <!-- Performance by Region -->
                <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700">
                    <div class="p-4">
                        <h5 class="text-lg font-semibold text-gray-900 dark:text-white mb-4">Regional Performance</h5>
                        <ejs-chart
                            width="100%"
                            height="250px"
                            [primaryXAxis]="pieXAxis"
                            [legendSettings]="pieLegendSettings"
                            [tooltip]="tooltip">

                            <e-series-collection>
                                <e-series
                                    [dataSource]="regionData"
                                    type="Pie"
                                    xName="region"
                                    yName="revenue"
                                    name="Revenue by Region"
                                    [dataLabel]="{ visible: true, position: 'Outside', name: 'region', format: '${point.percentage}%' }">
                                </e-series>
                            </e-series-collection>
                        </ejs-chart>
                    </div>
                </div>
            </div>

            <!-- Performance Summary -->
            <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 mt-6">
                <div class="p-4">
                    <h5 class="text-lg font-semibold text-gray-900 dark:text-white mb-4">Performance Summary</h5>
                    <div class="grid grid-cols-1 md:grid-cols-3 gap-4 text-sm">
                        <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
                            <h6 class="font-semibold text-blue-600 dark:text-blue-400">Best Performer</h6>
                            <p class="text-lg font-bold text-gray-900 dark:text-white">{{ topPerformer }}</p>
                            <p class="text-gray-600 dark:text-gray-400">{{ formatCurrency(topPerformerRevenue) }} revenue</p>
                        </div>
                        <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
                            <h6 class="font-semibold text-green-600 dark:text-green-400">Conversion Rate</h6>
                            <p class="text-lg font-bold text-gray-900 dark:text-white">{{ conversionRate }}%</p>
                            <p class="text-gray-600 dark:text-gray-400">Lead to Deal</p>
                        </div>
                        <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
                            <h6 class="font-semibold text-orange-600 dark:text-orange-400">Sales Cycle</h6>
                            <p class="text-lg font-bold text-gray-900 dark:text-white">{{ avgSalesCycle }} days</p>
                            <p class="text-gray-600 dark:text-gray-400">Average length</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
  `,
  styles: [`
    /* Custom CSS for responsive layout */
    .grid {
      display: grid;
    }

    .grid-cols-1 {
      grid-template-columns: repeat(1, minmax(0, 1fr));
    }

    .md\\:grid-cols-2 {
      @media (min-width: 768px) {
        grid-template-columns: repeat(2, minmax(0, 1fr));
      }
    }

    .lg\\:grid-cols-4 {
      @media (min-width: 1024px) {
        grid-template-columns: repeat(4, minmax(0, 1fr));
      }
    }

    .gap-4 {
      gap: 1rem;
    }

    .gap-6 {
      gap: 1.5rem;
    }

    .bg-gradient-to-r {
      background-image: linear-gradient(to right, var(--tw-gradient-stops));
    }

    .from-blue-500 {
      --tw-gradient-from: #3b82f6;
      --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(59 130 246 / 0));
    }

    .to-blue-600 {
      --tw-gradient-to: #2563eb;
    }

    .from-green-500 {
      --tw-gradient-from: #10b981;
      --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(16 185 129 / 0));
    }

    .to-green-600 {
      --tw-gradient-to: #059669;
    }

    .from-orange-500 {
      --tw-gradient-from: #f97316;
      --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(249 115 22 / 0));
    }

    .to-orange-600 {
      --tw-gradient-to: #ea580c;
    }

    .from-purple-500 {
      --tw-gradient-from: #a855f7;
      --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(168 85 247 / 0));
    }

    .to-purple-600 {
      --tw-gradient-to: #9333ea;
    }

    .text-white {
      color: white;
    }

    .rounded-lg {
      border-radius: 0.5rem;
    }

    .shadow-sm {
      box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
    }

    .border {
      border-width: 1px;
    }

    .border-gray-200 {
      border-color: #e5e7eb;
    }

    .p-4 {
      padding: 1rem;
    }
  `]
})
export class SyncfusionCompleteDashboardComponent implements OnInit {
  @ViewChild('chart') public chart!: ChartComponent;

  // Form controls
  selectedPeriod = 'Y';
  selectedMetric = 'revenue';

  // Period options
  periodOptions = [
    { text: 'Month', value: 'M' },
    { text: 'Quarter', value: 'Q' },
    { text: 'Year', value: 'Y' },
    { text: 'Custom', value: 'Custom' }
  ];

  metricOptions = [
    { text: 'Revenue', value: 'revenue' },
    { text: 'Deals', value: 'deals' },
    { text: 'Pipeline', value: 'pipeline' },
    { text: 'Activities', value: 'activities' }
  ];

  // Metrics data
  currentPeriodRevenue = 2450000;
  recordRevenue = 320000;
  totalRevenue = 2450000;
  revenueChange = 12.5;
  dealsWon = 48;
  dealsChange = 8;
  pipelineValue = 8700000;
  pipelineChange = 23.0;
  winRate = 72;
  activeLeads = 156;
  leadsChange = 24;
  averageDealSize = 51000;
  conversionRate = 35.2;
  avgSalesCycle = 45;

  // Top performer
  topPerformer = 'Sarah Johnson';
  topPerformerRevenue = 450000;

  // Chart data
  chartData: any[] = [
    { month: 'Jan', revenue: 180000, target: 200000 },
    { month: 'Feb', revenue: 220000, target: 210000 },
    { month: 'Mar', revenue: 320000, target: 250000 },
    { month: 'Apr', revenue: 280000, target: 270000 },
    { month: 'May', revenue: 290000, target: 280000 },
    { month: 'Jun', revenue: 310000, target: 290000 },
    { month: 'Jul', revenue: 260000, target: 275000 },
    { month: 'Aug', revenue: 240000, target: 265000 },
    { month: 'Sep', revenue: 270000, target: 280000 },
    { month: 'Oct', revenue: 300000, target: 295000 },
    { month: 'Nov', revenue: 330000, target: 310000 },
    { month: 'Dec', revenue: 350000, target: 320000 }
  ];

  // Pipeline funnel data
  pipelineData: any[] = [
    { stage: 'Leads', value: 500 },
    { stage: 'Qualified', value: 350 },
    { stage: 'Proposal', value: 200 },
    { stage: 'Negotiation', value: 120 },
    { stage: 'Closed Won', value: 48 }
  ];

  // Regional performance data
  regionData: any[] = [
    { region: 'North America', revenue: 1200000 },
    { region: 'Europe', revenue: 800000 },
    { region: 'Asia Pacific', revenue: 350000 },
    { region: 'Latin America', revenue: 100000 }
  ];

  // Chart configurations
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
    maximum: 400000,
    interval: 100000,
    edgeLabelPlacement: 'Shift',
    lineStyle: { width: 0 },
    labelStyle: { fontWeight: '500' },
    majorTickLines: { width: 0 },
    labelFormat: '${value}'
  };

  funnelXAxis: any = {
    valueType: 'Category',
    majorGridLines: { width: 0 }
  };

  funnelYAxis: any = {
    minimum: 0,
    interval: 100,
    labelFormat: '${value}'
  };

  pieXAxis: any = {
    valueType: 'Category'
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

  pieLegendSettings: any = {
    visible: true,
    position: 'Right'
  };

  constructor() { }

  ngOnInit(): void {
    this.updateData();
  }

  updateData(): void {
    // Update data based on selected period
    console.log('Updating data for period:', this.selectedPeriod);
    if (this.chart) {
      setTimeout(() => {
        this.chart.refresh();
      }, 100);
    }
  }

  onPeriodSelect(event: any): void {
    this.selectedPeriod = event.item.value;
    this.updateData();
  }

  onMetricSelect(event: any): void {
    this.selectedMetric = event.item.value;
    this.updateData();
  }

  exportReport(): void {
    console.log('Exporting report...');
    // Implement export functionality
  }

  chartLoad(args: any): void {
    args.chart.theme = 'Material';
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
      notation: 'compact',
      compactDisplay: 'short'
    }).format(value);
  }
}