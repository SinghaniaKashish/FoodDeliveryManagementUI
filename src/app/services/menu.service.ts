import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem } from '../models/menu-item.model';



@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private baseUrl = 'https://localhost:7072/api/Menu'; 

  constructor(private http: HttpClient) {}

  // Get menu items for a specific restaurant
  getMenuItemsByRestaurantId(restaurantId: number): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(`${this.baseUrl}/restaurant/${restaurantId}`);
  }

  // Create a new menu item
  createMenuItem(menuItem: MenuItem): Observable<MenuItem> {
    return this.http.post<MenuItem>(this.baseUrl, menuItem);
  }


  // Delete a menu item by ID
  deleteMenuItem(itemId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${itemId}`);
  }


  addMenuItem(restaurantId: number, menuItem: any) {
    return this.http.post<any>(`https://localhost:7072/api/Menu`, menuItem);
  }
  
  updateMenuItem(itemId: number, menuItem: MenuItem): Observable<any> {
    return this.http.put(`${this.baseUrl}/${itemId}`, menuItem);
  }
  
}
