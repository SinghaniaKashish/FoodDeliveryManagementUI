import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private baseUrl = 'https://localhost:7072/api/Order';

  constructor(private http: HttpClient) {}

  placeOrder(orderPayload: any): Observable<any> {
    return this.http.post(this.baseUrl, orderPayload);
  }

  getOrdersByUserId(userId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/customer/${userId}`);
  }

  getOrdersByRestaurant(restaurantId: number): Observable<any> {
    return this.http.get<any[]>(`${this.baseUrl}/restaurant/${restaurantId}`);
  }

  assignDriverToOrder(orderId: number): Observable<any> {
    return this.http.patch(`${this.baseUrl}/${orderId}/assign-driver`, {});
  }

  
  updateOrderStatus(orderId: number, newStatus: string): Observable<any> {
    const payload = { newStatus }; 
    return this.http.patch(`${this.baseUrl}/${orderId}/status`, payload, {
      headers: { 'Content-Type': 'application/json' }, 
    });
  }
  
  
  // https://localhost:7072/api/Order/7998/status  

}
