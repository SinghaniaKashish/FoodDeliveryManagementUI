import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private apiUrl = 'https://localhost:7072/api/Inventory'; 

  constructor(private http: HttpClient) {}

  getInventoryByRestaurantId(restaurantId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/restaurant/${restaurantId}`);
  }

  addInventoryItem(inventory: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, inventory);
  }

  updateInventory(inventory: any): Observable<any> {
    return this.http.put<any>(this.apiUrl, inventory);
  }

  addQuantityToInventory(inventoryId: number, quantityToAdd: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/add-quantity/${inventoryId}`, quantityToAdd);
  }
  
}
