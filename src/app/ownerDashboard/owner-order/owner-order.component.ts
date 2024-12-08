import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { CommonModule } from '@angular/common';
import { OwnerSidebarComponent } from "../owner-sidebar/owner-sidebar.component";
import { MatCardModule } from '@angular/material/card';
import { RestaurantService } from '../../services/restaurant.service';
import { AuthService } from '../../services/auth.service';
import { MenuService } from '../../services/menu.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-owner-order',
  standalone: true,
  imports: [CommonModule, OwnerSidebarComponent, MatCardModule],
  templateUrl: './owner-order.component.html',
  styleUrl: './owner-order.component.scss'
})


export class OwnerOrdersComponent implements OnInit {
  orders: any[] = [];
  restaurants: any[] = [];
  selectedRestaurant: any = null;


  constructor(private orderService: OrderService,
    private authService: AuthService,
    private menuService: MenuService,
    private restaurantService: RestaurantService,
    private http: HttpClient) {}

  ngOnInit(): void {
    const ownerId = this.authService.getUserId();
    this.fetchRestaurants(ownerId);
  }

  fetchRestaurants(ownerId: number) {
    this.restaurantService.getRestaurantsByOwner(ownerId).subscribe(
      (data) => {
        this.restaurants = data;
      },
      (error) => {
        console.error('Error fetching restaurants:', error);
      }
    );
  }

  selectRestaurant(restaurant: any) {
    this.selectedRestaurant = restaurant;
    this.loadOrders();
  }

  
  loadOrders(): void {
    this.orderService.getOrdersByRestaurant(this.selectedRestaurant.restaurantId).subscribe(
      (response) => {
        this.orders = response;
      },
      (error) => {
        console.error('Error fetching orders:', error);
      }
    );
  }

  updateOrderStatus(orderId: number, newStatus: string): void {
    this.orderService.updateOrderStatus(orderId, newStatus).subscribe(
      (response) => {
        console.log(response.message);
        this.loadOrders(); 
        if (newStatus === 'Prepared') {
          this.deductIngredients(orderId);  
        }
      },
      (error) => {
        console.error('Error updating order status:', error);
      }
    );
  }
  
  canUpdateStatus(status: string): boolean {
    return ['Pending', 'Accepted'].includes(status);
  }
  
  deductIngredients(orderId: number): void {
    this.http.post(`https://localhost:7072/api/Inventory/deduct-ingredients/${orderId}`, {}).subscribe(
      (response) => {
        console.log('Ingredients deducted successfully:', response);
      },
      (error) => {
        console.error('Error deducting ingredients:', error);
      }
    );
  }

  assignDriver(orderId: number): void {
    this.orderService.assignDriverToOrder(orderId).subscribe(
      (response) => {
        console.log(response.message);
        this.loadOrders();
      },
      (error) => {
        console.error('Error assigning driver:', error);
      }
    );
  }
}
