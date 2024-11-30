import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from './envornments/enviornment';
import { containerdetails } from './models/container-details';


@Injectable({
  providedIn: 'root',
})
export class containerdetailsService {
  private apiUrl = `${environment.apiBaseUrl}/api/RequiredFields`;

  constructor(private http: HttpClient) { }

  getRequiredFields(): Observable<containerdetails[]> {
    return this.http.get<containerdetails[]>(this.apiUrl);
  }

  getRequiredField(containerNumber: string): Observable<containerdetails> {
    return this.http.get<containerdetails>(`${this.apiUrl}/${containerNumber}`);
  }

  // addRequiredField(requiredField: RequiredFields): Observable<RequiredFields> {
  //   return this.http.post<RequiredFields>(this.apiUrl, requiredField);
  // }

  // updateRequiredField(containerNumber: string, requiredField: RequiredFields): Observable<void> {
  //   return this.http.put<void>(`${this.apiUrl}/${containerNumber}`, requiredField);
  // }

  deleteRequiredField(containerNumber: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${containerNumber}`);
  }

  // GET: Fetch demurrage fees for a specific container number
  getDemurrageFees(containerNumber: string): Observable<{ containerNumber: string; demurrageFees: { feesDue: number; feesPaid: number } }> {
    return this.http.get<{ containerNumber: string; demurrageFees: { feesDue: number; feesPaid: number } }>(
      `${this.apiUrl}/demurrage/${containerNumber}`
    );
  }
  // PUT: Update demurrage fees for a specific container number

  updateDemurrageFees(containerNumber: string, paymentDetails: { feesDue: number, feesPaid: number }) {
    return this.http.put<void>(`${this.apiUrl}/demurrage/${containerNumber}`, paymentDetails);
  }

  // Watchlist Management

  // Add a container to the watchlist (in localStorage)
  addToWatchlist(container: containerdetails): void {
    const watchlist = this.getWatchlist();
    if (!watchlist.find(c => c.containerNumber === container.containerNumber)) {
      watchlist.push(container);
      localStorage.setItem('watchlist', JSON.stringify(watchlist)); // Store updated watchlist in localStorage
    }
  }

  // Get all containers in the watchlist from localStorage
  getWatchlist(): containerdetails[] {
    const watchlist = localStorage.getItem('watchlist');
    return watchlist ? JSON.parse(watchlist) : [];
  }

  // Remove a container from the watchlist
  removeFromWatchlist(containerNumber: string): void {
    let watchlist = this.getWatchlist();
    watchlist = watchlist.filter(container => container.containerNumber !== containerNumber);
    localStorage.setItem('watchlist', JSON.stringify(watchlist)); // Update watchlist in localStorage
  }
}


