import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import {
  CustomerService,
  GetSalesOpportunitiesInput,
  PipelineStage,
  SalesOpportunityDto,
  pipelineStageOptions,
} from '../proxy/sales';
import { SimpleClientService } from '../services/simple-client.service';
import { SimpleSalesOpportunityService } from '../services/simple-sales-opportunity.service';

interface DashboardKpi {
  title: string;
  value: number;
  description: string;
  icon: string;
  isCurrency?: boolean;
}

interface StageSummary {
  stage: PipelineStage;
  label: string;
  count: number;
  value: number;
}

interface LeadSummary {
  label: string;
  count: number;
}

@Component({
  selector: 'app-sales-dashboard',
  templateUrl: './sales-dashboard.component.html',
  styleUrls: ['./sales-dashboard.component.scss'],
  imports: [CommonModule, FormsModule],
})
export class SalesDashboardComponent implements OnInit {
  private readonly opportunityService = inject(SimpleSalesOpportunityService);
  // TODO: Fix SalesLeadService when backend is ready
  // private readonly leadService = inject(SalesLeadService);
  // TODO: Fix InteractionService when backend is ready
  // private readonly interactionService = inject(InteractionService);
  private readonly clientService = inject(SimpleClientService);
  private readonly customerService = inject(CustomerService);

  readonly stageOptions = pipelineStageOptions;

  isLoading = false;
  kpis: DashboardKpi[] = [];
  stageSummary: StageSummary[] = [];
  upcomingOpportunities: SalesOpportunityDto[] = [];
  recentInteractions: any[] = [];
  leadSummary: LeadSummary[] = [];
  selectedRangeInDays = 30;

  get totalStageCount(): number {
    return this.stageSummary.reduce((sum, stage) => sum + stage.count, 0);
  }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    const opportunityRequest: GetSalesOpportunitiesInput = {
      skipCount: 0,
      maxResultCount: 200,
      sorting: 'expectedCloseDate',
    };

    forkJoin({
      opportunities: this.opportunityService.getList(opportunityRequest),
      clients: this.clientService.getList({ skipCount: 0, maxResultCount: 1 }),
      customers: this.customerService.getList({ skipCount: 0, maxResultCount: 1, includeInactive: false }),
    }).subscribe(({ opportunities, clients, customers }) => {
      this.kpis = this.buildKpis({ items: opportunities.items || [], totalCount: opportunities.totalCount || 0 }, clients.totalCount || 0, customers.totalCount || 0);
      this.stageSummary = this.buildStageSummary(opportunities.items);
      this.upcomingOpportunities = this.buildUpcoming(opportunities.items);
      // TODO: Add interactions when InteractionService is fixed
      this.recentInteractions = [];
      // TODO: Add leads when SalesLeadService is fixed
      this.leadSummary = [];
      this.isLoading = false;
    });
  }

  buildKpis(opportunities: { items: SalesOpportunityDto[]; totalCount: number }, clientCount: number, customerCount: number): DashboardKpi[] {
    const totalPipelineValue = opportunities.items.reduce((sum, opportunity) => sum + opportunity.estimatedValue, 0);
    const weightedValue = opportunities.items.reduce((sum, opportunity) => sum + (opportunity.getWeightedValue || 0), 0);
    const wonCount = opportunities.items.filter(opportunity => opportunity.stage === PipelineStage.Won).length;
    const winRate = opportunities.totalCount ? Math.round((wonCount / opportunities.totalCount) * 100) : 0;

    return [
      {
        title: 'Active Clients',
        value: clientCount,
        description: 'Companies managed in the CRM',
        icon: 'fa-building',
      },
      {
        title: 'Contacts',
        value: customerCount,
        description: 'Customers and decision makers',
        icon: 'fa-user-friends',
      },
      {
        title: 'Open Opportunities',
        value: opportunities.totalCount,
        description: `Win rate ${winRate}%`,
        icon: 'fa-briefcase',
      },
      {
        title: 'Pipeline Value',
        value: totalPipelineValue,
        description: `Weighted value ${this.formatNumber(weightedValue)}`,
        icon: 'fa-chart-line',
        isCurrency: true,
      },
    ];
  }

  buildStageSummary(opportunities: SalesOpportunityDto[]): StageSummary[] {
    return this.stageOptions.map(option => {
      const deals = opportunities.filter(opportunity => opportunity.stage === option.value);
      return {
        stage: option.value as PipelineStage,
        label: option.key || option.value.toString(),
        count: deals.length,
        value: deals.reduce((sum, opportunity) => sum + opportunity.estimatedValue, 0),
      };
    });
  }

  buildUpcoming(opportunities: SalesOpportunityDto[]): SalesOpportunityDto[] {
    const now = new Date();
    return opportunities
      .filter(opportunity => opportunity.expectedCloseDate && new Date(opportunity.expectedCloseDate) >= now)
      .sort((a, b) => new Date(a.expectedCloseDate).getTime() - new Date(b.expectedCloseDate).getTime())
      .slice(0, 5);
  }

  buildLeadSummary(leads: any[]): LeadSummary[] {
    // TODO: Implement when SalesLeadDto is available
    return [
      { label: 'New', count: 0 },
      { label: 'Qualified', count: 0 },
      { label: 'Converted', count: 0 },
      { label: 'Lost', count: 0 },
    ];
  }

  private formatNumber(value: number): string {
    return new Intl.NumberFormat(undefined, {
      maximumFractionDigits: 0,
    }).format(value);
  }

  getStageLabel(stage?: PipelineStage): string {
    const option = this.stageOptions.find(item => item.value === stage);
    return option?.key ?? (stage?.toString() || '-');
  }
}
