import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import {
  CustomerService,
  GetSalesOpportunitiesInput,
  PipelineStage,
  SalesOpportunityDto,
  SalesLeadDto,
  pipelineStageOptions,
} from '../proxy/sales';
import { SimpleClientService } from '../services/simple-client.service';
import { SimpleSalesOpportunityService } from '../services/simple-sales-opportunity.service';
import { SalesLeadService } from '../proxy/sales';

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
  imports: [CommonModule, FormsModule, LocalizationPipe],
})
export class SalesDashboardComponent implements OnInit {
  private readonly opportunityService = inject(SimpleSalesOpportunityService);
  private readonly leadService = inject(SalesLeadService);
  private readonly clientService = inject(SimpleClientService);
  private readonly customerService = inject(CustomerService);
  private readonly localization = inject(LocalizationService);

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
      leads: this.leadService.getList({ skipCount: 0, maxResultCount: 200, sorting: 'creationTime DESC', includeInactive: false }),
      clients: this.clientService.getList({ skipCount: 0, maxResultCount: 1 }),
      customers: this.customerService.getList({ skipCount: 0, maxResultCount: 1, includeInactive: false }),
    }).subscribe(({ opportunities, leads, clients, customers }) => {
      this.kpis = this.buildKpis({ items: opportunities.items || [], totalCount: opportunities.totalCount || 0 }, clients.totalCount || 0, customers.totalCount || 0);
      this.stageSummary = this.buildStageSummary(opportunities.items);
      this.upcomingOpportunities = this.buildUpcoming(opportunities.items);
      this.leadSummary = this.buildLeadSummary(leads.items || []);
      // TODO: Add interactions when InteractionService is fixed
      this.recentInteractions = [];
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
        title: this.localization.instant('::Dashboard:ActiveClients'),
        value: clientCount,
        description: this.localization.instant('::Dashboard:CompaniesManagedInCRM'),
        icon: 'fa-building',
      },
      {
        title: this.localization.instant('::Dashboard:Contacts'),
        value: customerCount,
        description: this.localization.instant('::Dashboard:CustomersAndDecisionMakers'),
        icon: 'fa-user-friends',
      },
      {
        title: this.localization.instant('::Dashboard:OpenOpportunities'),
        value: opportunities.totalCount,
        description: `${this.localization.instant('::Dashboard:WinRate')} ${winRate}%`,
        icon: 'fa-briefcase',
      },
      {
        title: this.localization.instant('::Dashboard:PipelineValue'),
        value: totalPipelineValue,
        description: `${this.localization.instant('::Dashboard:WeightedValue')} ${this.formatNumber(weightedValue)}`,
        icon: 'fa-chart-line',
        isCurrency: true,
      },
    ];
  }

  buildStageSummary(opportunities: SalesOpportunityDto[]): StageSummary[] {
    return this.stageOptions.map(option => {
      const deals = opportunities.filter(opportunity => opportunity.stage === option.value);
      const stageKey = option.key || option.value.toString();
      return {
        stage: option.value as PipelineStage,
        label: this.localization.instant(`::Enum:PipelineStage.${stageKey}`),
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

  buildLeadSummary(leads: SalesLeadDto[]): LeadSummary[] {
    const summary = [
      { label: this.localization.instant('::Dashboard:LeadFunnelNew'), count: 0 },
      { label: this.localization.instant('::Dashboard:LeadFunnelQualified'), count: 0 },
      { label: this.localization.instant('::Dashboard:LeadFunnelConverted'), count: 0 },
      { label: this.localization.instant('::Dashboard:LeadFunnelLost'), count: 0 },
    ];

    if (!leads || leads.length === 0) {
      return summary;
    }

    // Count leads by status or other criteria
    leads.forEach(lead => {
      // You can customize this logic based on your lead status system
      // For now, we'll categorize all leads as new since we don't have a specific status field
      summary[0].count++; // New leads

      // You might want to add more sophisticated logic here
      // For example, based on lead status, opportunity conversion, etc.
      if (lead.company) {
        summary[1].count++; // Qualified leads (with company info)
      }

      // For now, we'll consider all leads as potential (not lost)
      // You can implement more sophisticated logic later
    });

    return summary;
  }

  private formatNumber(value: number): string {
    return new Intl.NumberFormat(undefined, {
      maximumFractionDigits: 0,
    }).format(value);
  }

  getStageLabel(stage?: PipelineStage): string {
    const option = this.stageOptions.find(item => item.value === stage);
    const stageKey = option?.key ?? (stage?.toString() || '-');
    if (stageKey === '-') return '-';
    return this.localization.instant(`::Enum:PipelineStage.${stageKey}`);
  }
}
