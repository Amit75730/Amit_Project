import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loginUrl = 'http://localhost:5266/api/auth/login'; // Backend API URL

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(this.loginUrl, { username, password });
  }

  saveToken(token: string): void {
    localStorage.setItem('authToken', token); // Save token in local storage
  }

  logout(): void {
    localStorage.removeItem('authToken'); // Clear token on logout
  }

  // isAuthenticated(): boolean {
  //   return !!localStorage.getItem('authToken'); // Check token existence
  // }
  isAuthenticated(): boolean {
    if (typeof localStorage === 'undefined') {
        return false; // Return a default value when localStorage is unavailable
    }
    const token = localStorage.getItem('authtoken');
    return !!token;
}

}
