import { Injectable } from '@angular/core';
import { ClientService, GetClientsInput } from '../proxy/clients';
import type { PagedResultDto } from '@abp/ng.core';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClientSearchService {
  constructor(private clientService: ClientService) {}

  searchClientsByName(query: string): Observable<{ id: string, name: string }[]> {
    console.log('ClientSearchService: Searching with query:', query);

    if (!query || query.trim().length < 2) {
      console.log('ClientSearchService: Query too short, returning empty array');
      return of([]);
    }

    const input: GetClientsInput = {
      maxResultCount: 10,
      skipCount: 0,
      filter: query.trim(),
      sorting: 'name'
    };

    console.log('ClientSearchService: Making API call with input:', input);

    return this.clientService.getList(input).pipe(
      tap(response => console.log('ClientSearchService: API response:', response)),
      map((response: PagedResultDto<any>) => {
        const mapped = response.items.map((client: any) => ({
          id: client.id,
          name: client.name
        }));
        console.log('ClientSearchService: Mapped results:', mapped);
        return mapped;
      })
    );
  }
}