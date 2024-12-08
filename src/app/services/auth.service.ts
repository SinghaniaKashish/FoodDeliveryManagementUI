import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class AuthService {

  private apiUrl = 'https://localhost:7072/api'; 

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: { email: string; password: string }) {
    return this.http.post<{ token: string; role: string; userId:number }>(
      `${this.apiUrl}/Login/login`,credentials);
  }

  fetchUserById(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/User/${userId}`); 
  }

  signup(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/signup`, user);
  }
  saveToken(token: string) {
    localStorage.setItem('authToken', token);
  }
    
  saveTokenAndUserData(token: string, user: any) {
    localStorage.setItem('authToken', token);
    localStorage.setItem('userId', user.userId.toString());
    localStorage.setItem('userName', user.username || ''); 
    localStorage.setItem('userEmail', user.email || '');
  }
  

  logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userId');
    localStorage.removeItem('userName');
    localStorage.removeItem('userEmail');
    this.router.navigate(['/login']);
  }

  getToken() {
    return localStorage.getItem('authToken');
  }

  getUserId(): number {
    return parseInt(localStorage.getItem('userId') || '0', 10);
  }

  getUserName(): string {
    return localStorage.getItem('userName') || '';
  }

  getUserEmail(): string {    
    return localStorage.getItem('userEmail') || '';
  }
}
