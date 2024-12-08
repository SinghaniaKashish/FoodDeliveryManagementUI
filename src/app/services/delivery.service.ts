import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DeliveryService {
  private apiUrl = 'https://localhost:7072/api';

  constructor(private http: HttpClient) {}

  getDeliveriesByDriver(driverId: number): Observable<any> {
    return this.http.get<any[]>(`${this.apiUrl}/Delivery/driver/${driverId}`);
  }  
  updateDeliveryStatus(deliveryId: number, status: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/Order/deliveries/${deliveryId}/status`, { status });
  }

  getFeedbackByDriver(driverId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Feedback/driver/${driverId}`);
  }
  
}
