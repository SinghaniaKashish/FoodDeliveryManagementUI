import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private baseUrl = 'https://localhost:7072/api/Payment';

  constructor(private http: HttpClient) {}

  addPayment(payment: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, payment);
  }
}
