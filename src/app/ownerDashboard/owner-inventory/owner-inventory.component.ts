import { Component, OnInit } from '@angular/core';
import { InventoryService } from '../../services/inventory.service';
import { RestaurantService } from '../../services/restaurant.service';
import { AuthService } from '../../services/auth.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {MatTableModule} from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { OwnerSidebarComponent } from "../owner-sidebar/owner-sidebar.component";


@Component({
  selector: 'app-owner-inventory',
  standalone: true,
  imports: [MatFormFieldModule, MatCardModule, MatListModule, FormsModule, CommonModule, MatTableModule, MatInputModule, OwnerSidebarComponent],
  templateUrl: './owner-inventory.component.html',
  styleUrl: './owner-inventory.component.scss'
})
export class OwnerInventoryComponent {
  ownerId: number = 0;
  restaurants: any[] = [];
  selectedRestaurant: any = null;
  inventoryItems: any[] = [];
  newInventory = {
    restaurantId: 0,
    ingredientName: '',
    quantity: 0,
    reorderLevel: 0,
    status: '',
  };

  constructor(
    private inventoryService: InventoryService,
    private restaurantService: RestaurantService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.ownerId = this.authService.getUserId();
    this.fetchRestaurants(this.ownerId);
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
    this.fetchInventory();
  }

  fetchInventory() {
    this.inventoryService.getInventoryByRestaurantId(this.selectedRestaurant.restaurantId).subscribe(
      (data) => {
        this.inventoryItems = data;
      },
      (error) => {
        console.error('Error fetching inventory:', error);
      }
    );
  }

  addQuantity(inventoryId: number) {
    const quantityToAdd = prompt('Enter quantity to add:');
    if (quantityToAdd && !isNaN(Number(quantityToAdd)) && Number(quantityToAdd) > 0) {
      this.inventoryService.addQuantityToInventory(inventoryId, Number(quantityToAdd)).subscribe(
        (response) => {
          this.fetchInventory(); 
        },
        (error) => {
          console.error('Error updating inventory:', error);
        }
      );
    } else {
      alert('Invalid quantity. Please enter a valid number greater than zero.');
    }
  }

  addInventory() {
    this.newInventory.restaurantId = this.selectedRestaurant.restaurantId;
    this.inventoryService.addInventoryItem(this.newInventory).subscribe(
      () => {
        this.fetchInventory();
        this.resetNewInventory();
      },
      (error) => {
        console.error('Error adding inventory:', error);
      }
    );
  }

  resetNewInventory() {
    this.newInventory = {
      restaurantId: this.selectedRestaurant.restaurantId,
      ingredientName: ' ',
      quantity: 0,
      reorderLevel: 0,
      status: ' ',
    };
  }
}
