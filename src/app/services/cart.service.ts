import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CartItem } from '../models/cart-item.model';
import { MenuItem } from '../models/menu-item.model';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private baseUrl = 'https://localhost:7072/api/Cart';
  private cartItemsSubject = new BehaviorSubject<CartItem[]>([]);
  cartItems$ = this.cartItemsSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Load cart from backend
  loadCart(userId: number): void {
    this.http.get<{ status: string; data: CartItem[] }>(`${this.baseUrl}/${userId}`).subscribe(
      (response) => {
        this.cartItemsSubject.next(response.data); 
      },
      (error) => console.error('Failed to load cart:', error)
    );
  }

  

  // Add item to cart
  addToCart(userId: number, item: MenuItem, quantity: number = 1): void {
    const cartItem = { UserId: userId, menuItemId: item.itemId, Quantity: quantity, price: item.price, TotalPrice: quantity * item.price };
    this.http.post<CartItem>(this.baseUrl, cartItem).subscribe(
      () => this.loadCart(userId), 
      (error) => console.error('Failed to add item to cart:', error)
    );
    
  }

  incrementItem(userId: number, itemId: number): void {
    this.http.patch(`${this.baseUrl}/${userId}/increment/${itemId}`, null).subscribe(
      () => this.loadCart(userId), 
      (error) => console.error('Failed to increment item quantity:', error)
    );
  }

  decrementItem(userId: number, itemId: number): void {
    this.http.patch(`${this.baseUrl}/${userId}/decrement/${itemId}`, null).subscribe(
      () => this.loadCart(userId), 
      (error) => console.error('Failed to decrement item quantity:', error)
    );
  }

  removeItem(userId: number, itemId: number): void {
    this.http.delete(`${this.baseUrl}/${userId}/${itemId}`).subscribe(
      () => this.loadCart(userId), 
      (error) => console.error('Failed to remove item from cart:', error)
    );
  }

  // Clear cart
  clearCart(userId: number): void {
    this.http.delete(`${this.baseUrl}/${userId}`).subscribe(
      () => this.cartItemsSubject.next([]), 
      (error) => console.error('Failed to clear cart:', error)
    );
  }

  calculateTotal(): number {
    const cartItems = this.cartItemsSubject.value;
    return cartItems.reduce((total, item) => total + item.quantity * item.price, 0);
  }

  isInCart(itemId: number): boolean {
    return this.cartItemsSubject.value.some((cartItem) => cartItem.menuItemId === itemId);
  }

  getItemQuantity(itemId: number): number {
    const cartItems = this.cartItemsSubject.value;
    const item = cartItems.find(cartItem => cartItem.menuItemId == itemId);
    return item ? item.quantity : 0;
  }

  
}

