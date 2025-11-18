import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule, NgIf, NgFor, AsyncPipe } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, take, tap } from 'rxjs/operators';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSearchService } from '../services/user-search.service';
import { UserData } from '@abp/ng.identity/proxy';

@Component({
  selector: 'app-user-selection-modal',
  templateUrl: './user-selection-modal.component.html',
  styleUrls: ['./user-selection-modal.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    NgFor,
    AsyncPipe,
  ],
  })
export class UserSelectionModalComponent implements OnInit {
  @Input() title: string = 'Select User';
  @Input() selectedUserId?: string;
  @Output() userSelected = new EventEmitter<UserData>();

  searchForm: FormGroup;
  users$: Observable<UserData[]> = of([]);
  filteredUsers$: Observable<UserData[]> = of([]);
  selectedUser: UserData | null = null;
  isLoading = false;
  searchTerms = new Subject<string>();
  errorMessage = '';
  private latestUsers: UserData[] = [];

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

    // React to typing in the textbox and feed the search stream
    this.searchForm.get('searchTerm')?.valueChanges.subscribe(value => {
      this.onSearchChange(value ?? '');
    });

    // Keep local cache of latest users for datalist selection matching
    this.filteredUsers$.subscribe(users => (this.latestUsers = users || []));

    // If a selected user ID is provided, we could pre-load the user details
    if (this.selectedUserId) {
      // TODO: Load user details by ID
      console.log('Pre-selected user ID:', this.selectedUserId);
    }
  }

  setupSearch(): void {
    this.filteredUsers$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(raw => {
        const q = (raw || '').trim();
        this.errorMessage = '';
        if (!q || q.length < 2) {
          this.isLoading = false;
          return of([]);
        }
        this.isLoading = true;
        return this.userSearchService.searchUsers(q).pipe(
          tap(() => (this.isLoading = false))
        );
      })
    );
  }

  onSearchChange(event: any): void {
    const value = event?.target ? event.target.value : event;
    this.searchTerms.next(value);
    // Try to auto-select if input exactly matches an option
    this.syncSelectedFromInput((value || '').toString());
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

  onUserSelect(event: any): void {
    const selectedValue = event?.target?.value;
    if (!selectedValue || selectedValue.trim() === '') {
      this.selectedUser = null;
      return;
    }
    this.syncSelectedFromInput(selectedValue);
  }

  private syncSelectedFromInput(value: string): void {
    const v = (value || '').trim();
    if (!v) {
      this.selectedUser = null;
      return;
    }
    const matched = (this.latestUsers || []).find(u =>
      this.getUserDisplayName(u) === v ||
      u.userName === v ||
      `${u.name ?? ''} ${u.surname ?? ''}`.trim() === v ||
      u.email === v
    );
    if (matched) {
      this.selectedUser = matched;
    }
  }
}
