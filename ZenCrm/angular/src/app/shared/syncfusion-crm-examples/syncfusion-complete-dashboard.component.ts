import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { ChartAllModule, ChartComponent } from '@syncfusion/ej2-angular-charts';
import { ChangeDetectorRef } from '@angular/core';

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
  templateUrl: './syncfusion-complete-dashboard.component.html',
  styleUrl: './syncfusion-complete-dashboard.component.scss'
})
export class SyncfusionCompleteDashboardComponent implements OnInit {
  selectedPeriod: string = '2024';
  periodOptions: any[] = [
    { text: '2024', value: '2024' },
    { text: '2023', value: '2023' },
    { text: '2022', value: '2022' },
    { text: 'Last 6 Months', value: '6months' },
    { text: 'Last 3 Months', value: '3months' }
  ];

  // Chart configurations
  revenueChartConfig: any = {
    primaryXAxis: {
      valueType: 'Category',
      title: 'Month',
      labelRotation: -45,
      edgeLabelPlacement: 'Shift'
    },
    primaryYAxis: {
      title: 'Revenue ($)',
      labelFormat: '${value}',
      minimum: 0,
      maximum: 250000,
      interval: 50000
    },
    tooltip: {
      enable: true,
      format: '${series.name}: ${value}'
    }
  };

  regionChartConfig: any = {
    tooltip: {
      enable: true,
      format: '${series.name}: <b>${point.x}</b><br/>Revenue: ${point.y}'
    }
  };

  acquisitionChartConfig: any = {
    primaryXAxis: {
      valueType: 'Category',
      title: 'Month'
    },
    primaryYAxis: {
      title: 'New Clients',
      minimum: 0,
      maximum: 60,
      interval: 10
    },
    tooltip: {
      enable: true
    }
  };

  pipelineChartConfig: any = {
    primaryXAxis: {
      valueType: 'Category',
      title: 'Pipeline Stage'
    },
    primaryYAxis: {
      title: 'Value ($)',
      labelFormat: '${value}',
      minimum: 0,
      maximum: 600000,
      interval: 100000
    },
    tooltip: {
      enable: true
    }
  };

  leadSourcesChartConfig: any = {
    tooltip: {
      enable: true
    }
  };

  // Sample data
  revenueData: any[] = [
    { month: 'Jan', revenue: 180000 },
    { month: 'Feb', revenue: 195000 },
    { month: 'Mar', revenue: 210000 },
    { month: 'Apr', revenue: 195000 },
    { month: 'May', revenue: 220000 },
    { month: 'Jun', revenue: 235000 },
    { month: 'Jul', revenue: 210000 },
    { month: 'Aug', revenue: 225000 },
    { month: 'Sep', revenue: 240000 },
    { month: 'Oct', revenue: 220000 },
    { month: 'Nov', revenue: 235000 },
    { month: 'Dec', revenue: 245000 }
  ];

  regionData: any[] = [
    { region: 'North America', revenue: 850000 },
    { region: 'Europe', revenue: 620000 },
    { region: 'Asia', revenue: 580000 },
    { region: 'South America', revenue: 280000 },
    { region: 'Africa', revenue: 120000 }
  ];

  acquisitionData: any[] = [
    { month: 'Jan', clients: 35 },
    { month: 'Feb', clients: 42 },
    { month: 'Mar', clients: 38 },
    { month: 'Apr', clients: 45 },
    { month: 'May', clients: 48 },
    { month: 'Jun', clients: 52 }
  ];

  pipelineData: any[] = [
    { stage: 'Prospecting', value: 450000 },
    { stage: 'Qualification', value: 380000 },
    { stage: 'Proposal', value: 280000 },
    { stage: 'Negotiation', value: 180000 },
    { stage: 'Closing', value: 95000 }
  ];

  leadSourcesData: any[] = [
    { source: 'Website', count: 342 },
    { source: 'Referral', count: 215 },
    { source: 'Social Media', count: 156 },
    { source: 'Email', count: 89 },
    { source: 'Other', count: 45 }
  ];

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    // Initialize data when component loads
    this.updateChartData();
  }

  onPeriodChange(event: any): void {
    this.selectedPeriod = event.value;
    this.updateChartData();
  }

  updateChartData(): void {
    // Simulate data changes based on period selection
    // In a real app, this would fetch data from an API
    console.log('Period changed to:', this.selectedPeriod);
    this.cdr.detectChanges();
  }

  exportReport(): void {
    // Simulate report export
    console.log('Exporting report for period:', this.selectedPeriod);
    alert('Report exported successfully!');
  }

  onChartLoad(args: any): void {
    // Apply dark theme if needed
    args.chart.theme = 'Material';
  }

  onAxisLabelRender(args: any): void {
    // Customize axis labels if needed
    if (args.axis.valueType === 'Category') {
      // Category labels
      args.label = args.label;
    } else if (args.axis.labelFormat) {
      // Numeric labels
      args.label = args.text;
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