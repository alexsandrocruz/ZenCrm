import { Injectable } from '@angular/core';
import { Observable, of, race } from 'rxjs';
import { UserData } from '@abp/ng.identity/proxy';
import { IdentityUserService } from '@abp/ng.identity/proxy';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { environment } from '../../environments/environment';
import { GetIdentityUsersInput } from '@abp/ng.identity/proxy';

@Injectable({
  providedIn: 'root'
})
export class UserSearchService {
  constructor(
    private userService: IdentityUserService,
    private http: HttpClient,
    private oauth: OAuthService,
  ) {}

  searchUsers(query: string): Observable<UserData[]> {
    console.log('UserSearchService: Searching with query:', query);

    if (!query || query.trim().length < 2) {
      console.log('UserSearchService: Query too short, returning empty array');
      return of([]);
    }

    const input: GetIdentityUsersInput = {
      maxResultCount: 10,
      skipCount: 0,
      filter: query.trim(),
      sorting: 'name'
    };

    console.log('UserSearchService: Making API call with input:', input);

    const rest$ = this.userService.getList(input).pipe(
      tap(response => console.log('UserSearchService: GetList response:', response)),
      map(response => (response.items || []).map(user => ({
        id: user.id,
        userName: user.userName,
        name: user.name,
        surname: user.surname,
        email: user.email,
        emailConfirmed: user.emailConfirmed,
        phoneNumber: user.phoneNumber,
        phoneNumberConfirmed: user.phoneNumberConfirmed,
        isActive: user.isActive,
        displayName: this.getUserDisplayName(user)
      }))),
    );

    // Raw HttpClient fallback in case RestService/interceptors hang
    const token = this.oauth.getAccessToken();
    const url = `${environment.apis.default.url}/api/identity/users`;
    const params = new HttpParams()
      .set('Filter', query.trim())
      .set('SkipCount', 0)
      .set('MaxResultCount', 10)
      .set('Sorting', 'name');
    const headers = new HttpHeaders({ Authorization: token ? `Bearer ${token}` : '' });
    const http$ = this.http.get<any>(url, { params, headers }).pipe(
      tap(response => console.log('UserSearchService: Raw HTTP response:', response)),
      map(response => (response.items || []).map((user: any) => ({
        id: user.id,
        userName: user.userName,
        name: user.name,
        surname: user.surname,
        email: user.email,
        emailConfirmed: user.emailConfirmed,
        phoneNumber: user.phoneNumber,
        phoneNumberConfirmed: user.phoneNumberConfirmed,
        isActive: user.isActive,
        displayName: this.getUserDisplayName(user)
      }))),
    );

    return race(rest$, http$).pipe(
      catchError(error => {
        console.error('UserSearchService: Error searching users (race fallback):', error);
        return of(this.getMockUsers(query));
      })
    );
  }

  getUserDisplayName(user: any): string {
    const parts = [];
    if (user.name) parts.push(user.name);
    if (user.surname) parts.push(user.surname);
    if (user.userName && parts.length === 0) parts.push(user.userName);
    if (user.email && parts.length === 0) parts.push(user.email);
    return parts.join(' ') || user.userName || 'Unknown User';
  }

  private getMockUsers(query: string): UserData[] {
    console.log('UserSearchService: Using mock users for query:', query);

    const mockUsers = [
      {
        id: '1',
        userName: 'admin',
        name: 'Admin',
        surname: 'User',
        email: 'admin@example.com',
        emailConfirmed: true,
        phoneNumber: null,
        phoneNumberConfirmed: false,
        isActive: true
      },
      {
        id: '2',
        userName: 'user1',
        name: 'Demo',
        surname: 'User',
        email: 'user1@example.com',
        emailConfirmed: true,
        phoneNumber: null,
        phoneNumberConfirmed: false,
        isActive: true
      }
    ];

    const filtered = mockUsers.filter(user =>
      user.userName.toLowerCase().includes(query.toLowerCase()) ||
      user.name.toLowerCase().includes(query.toLowerCase()) ||
      user.surname.toLowerCase().includes(query.toLowerCase()) ||
      user.email?.toLowerCase().includes(query.toLowerCase())
    );

    return filtered.map(user => ({
      ...user,
      displayName: this.getUserDisplayName(user)
    }));
  }
}
