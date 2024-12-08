// src/app/services/restaurant.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  private apiUrl = 'https://localhost:7072/api/Restaurant'; 

  constructor(private http: HttpClient) {}

  // Fetch restaurants by ownerId
  getRestaurantsByOwner(ownerId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/ownerId?ownerId=${ownerId}`);
  }
}
