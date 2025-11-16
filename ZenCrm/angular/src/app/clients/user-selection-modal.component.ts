import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSearchService } from '../services/user-search.service';
import { UserData } from '@abp/ng.identity/proxy';
import { NgIf, NgFor, AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-user-selection-modal',
  templateUrl: './user-selection-modal.component.html',
  styleUrls: ['./user-selection-modal.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  })
export class UserSelectionModalComponent implements OnInit {
  @Input() title: string = 'Select User';
  @Input() selectedUserId?: string;
  @Output() userSelected = new EventEmitter<UserData>();

  searchForm: FormGroup;
  users$: Observable<UserData[]> = of([]);
  selectedUser: UserData | null = null;
  isLoading = false;
  searchTerms = new Subject<string>();

  private fb = inject(FormBuilder);
  public modal = inject(NgbActiveModal);
  private userSearchService = inject(UserSearchService);

  constructor() {
    this.searchForm = this.fb.group({
      searchTerm: ['']
    });
  }

  ngOnInit(): void {
    this.setupSearch();

    // If a selected user ID is provided, we could pre-load the user details
    if (this.selectedUserId) {
      // TODO: Load user details by ID
      console.log('Pre-selected user ID:', this.selectedUserId);
    }
  }

  setupSearch(): void {
    this.users$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.isLoading = true),
      switchMap(query => {
        if (!query || query.trim().length < 2) {
          this.isLoading = false;
          return of([]);
        }
        return this.userSearchService.searchUsers(query).pipe(
          tap(() => this.isLoading = false)
        );
      })
    );

    // Initialize with empty search to show no users
    this.searchTerms.next('');
  }

  onSearchChange(event: any): void {
    const value = event.target ? event.target.value : event;
    this.searchTerms.next(value);
  }

  selectUser(user: UserData): void {
    this.selectedUser = user;
  }

  confirmSelection(): void {
    if (this.selectedUser) {
      this.userSelected.emit(this.selectedUser);
      this.modal.close(this.selectedUser);
    } else {
      // Still close modal but emit null
      this.userSelected.emit(null);
      this.modal.close(null);
    }
  }

  cancel(): void {
    this.modal.dismiss();
  }

  // Helper method to get user display name
  getUserDisplayName(user: UserData): string {
    return this.userSearchService.getUserDisplayName(user);
  }

  trackUserById(index: number, user: UserData): string {
    return user.id || '';
  }

  // Clear selection
  clearSelection(): void {
    this.selectedUser = null;
    this.searchForm.get('searchTerm')?.setValue('');
    this.searchTerms.next('');
  }
}