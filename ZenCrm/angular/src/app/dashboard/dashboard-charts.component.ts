import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocalizationService, LocalizationPipe } from '@abp/ng.core';
import { SalesLeadService } from '../proxy/sales';
import { SimpleSalesOpportunityService } from '../services/simple-sales-opportunity.service';
import { SalesOpportunityDto, SalesLeadDto, priorityOptions, PipelineStage, Priority } from '../proxy/sales';

interface ChartData {
  labels: string[];
  datasets: ChartDataset[];
}

interface ChartDataset {
  label: string;
  data: number[];
  backgroundColor?: string[];
  borderColor?: string[];
  borderWidth?: number;
}

interface ConversionData {
  stage: string;
  total: number;
  converted: number;
  rate: number;
}

interface StageData {
  stage: PipelineStage;
  count: number;
  value: number;
  label: string;
}

interface PriorityData {
  priority: Priority;
  count: number;
  value: number;
  label: string;
  color: string;
}

interface TrendData {
  month: string;
  created: number;
  won: number;
  value: number;
}

@Component({
  selector: 'app-dashboard-charts',
  standalone: true,
  imports: [CommonModule, LocalizationPipe],
  template: `
    <div class="row g-3">
      <!-- Sales Pipeline Funnel Chart -->
      <div class="col-lg-6">
        <div class="card h-100">
          <div class="card-header">
            <h6 class="card-title mb-0">{{ '::Dashboard:SalesPipelineFunnel' | abpLocalization }}</h6>
            <small class="text-muted">{{ '::Dashboard:OpportunitiesByStage' | abpLocalization }}</small>
          </div>
          <div class="card-body">
            @if(loadingCharts) {
              <div class="text-center py-5">
                <i class="fas fa-spinner fa-spin fa-2x text-primary mb-3"></i>
                <h6 class="text-muted">{{ '::Dashboard:Loading' | abpLocalization }}...</h6>
              </div>
            } @else {
              <div class="pipeline-funnel">
                @for(stage of stageData; track stage.stage) {
                  <div class="funnel-stage" [style.width.%]="getFunnelWidth(stage.count, stageData)">
                    <div class="funnel-content">
                      <span class="stage-label">{{ stage.label }}</span>
                      <span class="stage-count">{{ stage.count }}</span>
                    </div>
                    <div class="funnel-value">{{ stage.value | currency }}</div>
                  </div>
                }
              </div>
            }
          </div>
        </div>
      </div>

      <!-- Priority Distribution -->
      <div class="col-lg-6">
        <div class="card h-100">
          <div class="card-header">
            <h6 class="card-title mb-0">{{ '::Dashboard:PriorityDistribution' | abpLocalization }}</h6>
            <small class="text-muted">{{ '::Dashboard:ValueByPriority' | abpLocalization }}</small>
          </div>
          <div class="card-body">
            @if(loadingCharts) {
              <div class="text-center py-5">
                <i class="fas fa-spinner fa-spin fa-2x text-info mb-3"></i>
                <h6 class="text-muted">{{ '::Dashboard:Loading' | abpLocalization }}...</h6>
              </div>
            } @else {
              <div class="priority-chart">
                @for(priority of priorityData; track priority.priority) {
                  <div class="priority-item">
                    <div class="priority-label">
                      <span class="priority-dot" [style.background-color]="priority.color"></span>
                      {{ priority.label }}
                    </div>
                    <div class="priority-stats">
                      <span class="priority-count">{{ priority.count }}</span>
                      <span class="priority-value">{{ priority.value | currency }}</span>
                    </div>
                  </div>
                }
              </div>
            }
          </div>
        </div>
      </div>

      <!-- Sales Trends -->
      <div class="col-lg-8">
        <div class="card h-100">
          <div class="card-header">
            <h6 class="card-title mb-0">{{ '::Dashboard:SalesTrends' | abpLocalization }}</h6>
            <small class="text-muted">{{ '::Dashboard:MonthlyOpportunityTrends' | abpLocalization }}</small>
          </div>
          <div class="card-body">
            @if(loadingCharts) {
              <div class="text-center py-5">
                <i class="fas fa-spinner fa-spin fa-2x text-success mb-3"></i>
                <h6 class="text-muted">{{ '::Dashboard:Loading' | abpLocalization }}...</h6>
              </div>
            } @else {
              <div class="trends-chart">
                <div class="trends-header">
                  <span class="trend-item created">Created</span>
                  <span class="trend-item won">Won</span>
                </div>
                @for(trend of trendData; track trend.month) {
                  <div class="trend-row">
                    <span class="trend-month">{{ trend.month }}</span>
                    <div class="trend-bars">
                      <div class="trend-bar created" [style.width.%]="getTrendPercentage(trend.created, maxCreatedValue)">
                        {{ trend.created }}
                      </div>
                      <div class="trend-bar won" [style.width.%]="getTrendPercentage(trend.won, maxWonValue)">
                        {{ trend.won }}
                      </div>
                    </div>
                    <span class="trend-value">{{ trend.value | currency }}</span>
                  </div>
                }
              </div>
            }
          </div>
        </div>
      </div>

      <!-- Conversion Rates -->
      <div class="col-lg-4">
        <div class="card h-100">
          <div class="card-header">
            <h6 class="card-title mb-0">{{ '::Dashboard:ConversionRates' | abpLocalization }}</h6>
            <small class="text-muted">{{ '::Dashboard:StageToStageConversion' | abpLocalization }}</small>
          </div>
          <div class="card-body">
            @if(loadingCharts) {
              <div class="text-center py-5">
                <i class="fas fa-spinner fa-spin fa-2x text-warning mb-3"></i>
                <h6 class="text-muted">{{ '::Dashboard:Loading' | abpLocalization }}...</h6>
              </div>
            } @else {
              <div class="conversion-chart">
                @for(conversion of conversionData; track conversion.stage) {
                  <div class="conversion-item">
                    <div class="conversion-stage">{{ conversion.stage }}</div>
                    <div class="conversion-rate">{{ conversion.rate }}%</div>
                    <div class="conversion-details">
                      <span>{{ conversion.converted }}/{{ conversion.total }}</span>
                    </div>
                  </div>
                }
              </div>
            }
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .card-body {
      min-height: 300px;
    }

    .pipeline-funnel {
      padding: 10px 0;
    }

    .funnel-stage {
      background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
      margin: 8px 0;
      border-radius: 4px;
      padding: 12px;
      color: white;
      transition: all 0.3s ease;
      max-width: 100%;
    }

    .funnel-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      font-weight: 500;
    }

    .funnel-value {
      font-size: 0.9rem;
      opacity: 0.9;
      margin-top: 4px;
    }

    .priority-chart {
      padding: 10px 0;
    }

    .priority-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 10px 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .priority-label {
      display: flex;
      align-items: center;
      font-weight: 500;
    }

    .priority-dot {
      width: 12px;
      height: 12px;
      border-radius: 50%;
      margin-right: 8px;
    }

    .priority-stats {
      text-align: right;
    }

    .priority-count {
      display: block;
      font-weight: 600;
      font-size: 1.1rem;
    }

    .priority-value {
      font-size: 0.9rem;
      color: #6c757d;
    }

    .trends-chart {
      padding: 10px 0;
    }

    .trends-header {
      display: flex;
      justify-content: flex-end;
      margin-bottom: 15px;
      font-size: 0.85rem;
    }

    .trend-item {
      margin-left: 20px;
      padding: 2px 8px;
      border-radius: 3px;
      color: white;
      font-weight: 500;
    }

    .trend-item.created {
      background-color: #007bff;
    }

    .trend-item.won {
      background-color: #28a745;
    }

    .trend-row {
      display: flex;
      align-items: center;
      margin-bottom: 8px;
      font-size: 0.9rem;
    }

    .trend-month {
      width: 60px;
      font-weight: 500;
    }

    .trend-bars {
      flex: 1;
      margin: 0 15px;
      display: flex;
      gap: 5px;
    }

    .trend-bar {
      color: white;
      padding: 4px 8px;
      border-radius: 3px;
      font-weight: 500;
      text-align: right;
      min-width: 30px;
      font-size: 0.8rem;
    }

    .trend-bar.created {
      background-color: #007bff;
    }

    .trend-bar.won {
      background-color: #28a745;
    }

    .trend-value {
      width: 80px;
      text-align: right;
      font-weight: 600;
    }

    .conversion-chart {
      padding: 10px 0;
    }

    .conversion-item {
      text-align: center;
      padding: 15px 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .conversion-item:last-child {
      border-bottom: none;
    }

    .conversion-stage {
      font-weight: 600;
      font-size: 0.9rem;
      margin-bottom: 8px;
    }

    .conversion-rate {
      font-size: 2rem;
      font-weight: 700;
      color: #ffc107;
      margin: 10px 0;
    }

    .conversion-details {
      font-size: 0.85rem;
      color: #6c757d;
    }
  `]
})
export class DashboardChartsComponent implements OnInit {
  private readonly localization = inject(LocalizationService);
  private readonly opportunityService = inject(SimpleSalesOpportunityService);

  loadingCharts = true;
  stageData: StageData[] = [];
  priorityData: PriorityData[] = [];
  trendData: TrendData[] = [];
  conversionData: ConversionData[] = [];

  maxCreatedValue = 0;
  maxWonValue = 0;

  ngOnInit(): void {
    this.loadChartData();
  }

  loadChartData(): void {
    this.opportunityService.getList({
      skipCount: 0,
      maxResultCount: 1000,
      sorting: 'creationTime DESC'
    }).subscribe({
      next: (opportunities) => {
        this.stageData = this.buildStageData(opportunities.items || []);
        this.priorityData = this.buildPriorityData(opportunities.items || []);
        this.trendData = this.buildTrendData(opportunities.items || []);
        this.conversionData = this.buildConversionData(opportunities.items || []);

        this.maxCreatedValue = Math.max(...this.trendData.map(t => t.created), 1);
        this.maxWonValue = Math.max(...this.trendData.map(t => t.won), 1);

        this.loadingCharts = false;
      },
      error: (error) => {
        console.error('Error loading chart data:', error);
        this.loadingCharts = false;
      }
    });
  }

  buildStageData(opportunities: SalesOpportunityDto[]): StageData[] {
    const stages = [
      PipelineStage.Lead,
      PipelineStage.Qualifying,
      PipelineStage.Qualified,
      PipelineStage.Analysis,
      PipelineStage.Proposal,
      PipelineStage.ProposalSent,
      PipelineStage.Negotiation,
      PipelineStage.VerbalCommitment,
      PipelineStage.Closing,
      PipelineStage.Won,
      PipelineStage.Lost,
      PipelineStage.OnHold
    ];

    return stages.map(stage => {
      const stageOpportunities = opportunities.filter(o => o.stage === stage);
      const stageKey = stage.toString();
      return {
        stage,
        count: stageOpportunities.length,
        value: stageOpportunities.reduce((sum, o) => sum + o.estimatedValue, 0),
        label: this.localization.instant(`::Enum:PipelineStage.${stageKey}`)
      };
    }).filter(s => s.count > 0);
  }

  buildPriorityData(opportunities: SalesOpportunityDto[]): PriorityData[] {
    const priorities = [Priority.Low, Priority.Normal, Priority.High, Priority.Critical];

    return priorities.map(priority => {
      const priorityOpportunities = opportunities.filter(o => o.priority === priority);
      const priorityKey = priority.toString();
      const colors = {
        [Priority.Low]: '#6c757d',
        [Priority.Normal]: '#007bff',
        [Priority.High]: '#fd7e14',
        [Priority.Critical]: '#dc3545'
      };

      return {
        priority,
        count: priorityOpportunities.length,
        value: priorityOpportunities.reduce((sum, o) => sum + o.estimatedValue, 0),
        label: this.localization.instant(`::Enum:Priority.${priorityKey}`),
        color: colors[priority]
      };
    }).filter(p => p.count > 0);
  }

  buildTrendData(opportunities: SalesOpportunityDto[]): TrendData[] {
    const monthlyData: { [key: string]: TrendData } = {};
    const currentDate = new Date();

    // Generate last 6 months
    for (let i = 5; i >= 0; i--) {
      const date = new Date(currentDate.getFullYear(), currentDate.getMonth() - i, 1);
      const monthKey = date.toLocaleDateString('en', { month: 'short', year: 'numeric' });
      monthlyData[monthKey] = {
        month: date.toLocaleDateString('pt-BR', { month: 'short' }),
        created: 0,
        won: 0,
        value: 0
      };
    }

    opportunities.forEach(opportunity => {
      const creationDate = new Date(opportunity.creationTime);
      const monthKey = creationDate.toLocaleDateString('en', { month: 'short', year: 'numeric' });

      if (monthlyData[monthKey]) {
        monthlyData[monthKey].created++;
        if (opportunity.stage === PipelineStage.Won) {
          monthlyData[monthKey].won++;
          monthlyData[monthKey].value += opportunity.estimatedValue;
        }
      }
    });

    return Object.values(monthlyData);
  }

  buildConversionData(opportunities: SalesOpportunityDto[]): ConversionData[] {
    const stageFlow = [
      { stage: 'Lead → Qualified', current: PipelineStage.Lead, next: PipelineStage.Qualified },
      { stage: 'Qualified → Proposal', current: PipelineStage.Qualified, next: PipelineStage.Proposal },
      { stage: 'Proposal → Negotiation', current: PipelineStage.Proposal, next: PipelineStage.Negotiation },
      { stage: 'Negotiation → Won', current: PipelineStage.Negotiation, next: PipelineStage.Won }
    ];

    return stageFlow.map(flow => {
      const currentStage = opportunities.filter(o => o.stage === flow.current).length;
      const nextStage = opportunities.filter(o => o.stage === flow.next).length;
      const rate = currentStage > 0 ? Math.round((nextStage / currentStage) * 100) : 0;

      return {
        stage: flow.stage,
        total: currentStage,
        converted: nextStage,
        rate
      };
    });
  }

  getFunnelWidth(count: number, allStages: StageData[]): number {
    const maxCount = Math.max(...allStages.map(s => s.count), 1);
    return Math.max(20, (count / maxCount) * 100);
  }

  getTrendPercentage(value: number, maxValue: number): number {
    return Math.max(5, (value / maxValue) * 100);
  }
}