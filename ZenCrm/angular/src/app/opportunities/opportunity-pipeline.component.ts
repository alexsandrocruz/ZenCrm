import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DragDropModule, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { RestService } from '@abp/ng.core';
import { ClientDto, PipelineStage, SalesOpportunityDto, pipelineStageOptions } from '../proxy/sales';

interface PipelineColumn {
  stage: PipelineStage;
  label: string;
  opportunities: SalesOpportunityDto[];
  totalValue: number;
  dropListId: string;
}

@Component({
  selector: 'app-opportunity-pipeline',
  templateUrl: './opportunity-pipeline.component.html',
  styleUrls: ['./opportunity-pipeline.component.scss'],
  imports: [CommonModule, FormsModule, DragDropModule],
})
export class OpportunityPipelineComponent implements OnInit {
  private readonly restService = inject(RestService);

  readonly boardStages: PipelineStage[] = [
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
    PipelineStage.OnHold,
  ];

  columns: PipelineColumn[] = [];
  lostOpportunities: SalesOpportunityDto[] = [];
  clients: ClientDto[] = [];
  isLoading = false;
  filterText = '';
  clientFilter: string | null = null;

  ngOnInit(): void {
    this.initializeColumns();
    this.loadClients();
    this.loadPipeline();
  }

  initializeColumns(): void {
    this.columns = this.boardStages.map(stage => {
      const option = pipelineStageOptions.find(x => x.value === stage);
      return {
        stage,
        label: option?.key ?? PipelineStage[stage],
        opportunities: [],
        totalValue: 0,
        dropListId: this.getDropListId(stage),
      };
    });
  }

  get connectedDropLists(): string[] {
    return this.columns.map(column => column.dropListId);
  }

  loadClients(): void {
    this.restService.request<void, any>({
      method: 'GET',
      url: '/api/app/client',
      params: { skipCount: 0, maxResultCount: 100, sorting: 'name' }
    }, { apiName: 'Default' }).subscribe(result => (this.clients = result.items));
  }

  loadPipeline(): void {
    this.isLoading = true;
    this.restService.request<void, any>({
      method: 'GET',
      url: '/api/app/sales-opportunity',
      params: {
        skipCount: 0,
        maxResultCount: 500,
        filter: this.filterText?.trim() || undefined,
        clientId: this.clientFilter || undefined,
        sorting: 'stage, expectedCloseDate',
      }
    }, { apiName: 'Default' }).subscribe(result => {
      this.columns.forEach(column => {
        column.opportunities = [];
        column.totalValue = 0;
      });
      this.lostOpportunities = [];

      result.items.forEach(opportunity => {
        const column = this.columns.find(col => col.stage === opportunity.stage);
        if (column) {
          column.opportunities.push(opportunity);
          column.totalValue += opportunity.estimatedValue;
        } else if (opportunity.stage === PipelineStage.Lost) {
            this.lostOpportunities.push(opportunity);
          }
        });

        this.isLoading = false;
      });
  }

  drop(event: CdkDragDrop<SalesOpportunityDto[]>, column: PipelineColumn): void {
    if (column.stage === PipelineStage.Lost) {
      return;
    }

    if (event.previousContainer === event.container) {
      moveItemInArray(column.opportunities, event.previousIndex, event.currentIndex);
      return;
    }

    const movedOpportunity = event.previousContainer.data[event.previousIndex];

    transferArrayItem(
      event.previousContainer.data,
      event.container.data,
      event.previousIndex,
      event.currentIndex,
    );

    movedOpportunity.stage = column.stage;
    this.restService.request<void, any>({
      method: 'POST',
      url: `/api/app/sales-opportunity/${movedOpportunity.id}/move-to-stage`,
      params: { newStage: column.stage.toString() }
    }, { apiName: 'Default' }).subscribe({
      next: updated => {
        Object.assign(movedOpportunity, updated);
        this.recalculateTotals();
      },
      error: () => this.loadPipeline(),
    });
  }

  recalculateTotals(): void {
    this.columns.forEach(column => {
      column.totalValue = column.opportunities.reduce((sum, opportunity) => sum + opportunity.estimatedValue, 0);
    });
  }

  applyFilters(): void {
    this.loadPipeline();
  }

  resetFilters(): void {
    this.filterText = '';
    this.clientFilter = null;
    this.loadPipeline();
  }

  trackByOpportunity(_index: number, item: SalesOpportunityDto): string {
    return item.id;
  }

  getDropListId(stage: PipelineStage): string {
    return `stage-column-${stage}`;
  }

  isDropDisabled(stage: PipelineStage): boolean {
    return stage === PipelineStage.Lost;
  }
}
