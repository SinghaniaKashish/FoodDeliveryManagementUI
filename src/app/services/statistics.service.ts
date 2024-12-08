import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private baseUrl = 'https://localhost:7072/api/RestaurantStatistic';
  // https://localhost:7072/api/RestaurantStatistic/7


  constructor(private http: HttpClient) {}

  getOwnerStatistics(ownerId:number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/${ownerId}`);
  }
}
