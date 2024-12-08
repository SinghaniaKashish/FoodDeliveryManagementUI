import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuService } from '../../services/menu.service';
import { CartService } from '../../services/cart.service';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { CommonModule } from '@angular/common';
import { MenuItem } from '../../models/menu-item.model';
import { NavbarComponent } from '../navbar/navbar.component';
import { HttpClient } from '@angular/common/http';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';


@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, 
    MatCardModule, MatGridListModule, 
    NavbarComponent, MatSlideToggleModule, FormsModule, MatInputModule
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})


export class MenuComponent implements OnInit {
  menuItems: MenuItem[] = [];
  isLoading: boolean = true;
  userId: number = Number(localStorage.getItem('userId') || 0);
  restaurantId: string | null = null;
  cuisine: string | null = null;

  filteredMenuItems: MenuItem[] = [];
  showVegOnly = true;
  showNonVegOnly = true;

  constructor(
    private route: ActivatedRoute,
    private menuService: MenuService,
    private cartService: CartService,
    private http: HttpClient
  ) {}


  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.restaurantId = params['restaurantId'] || null;
      this.cuisine = params['cuisine'] || null;
      if (this.cuisine) {
        this.fetchMenuByCuisine(this.cuisine);
      }
      else {
        this.restaurantId = (this.route.snapshot.paramMap.get('restaurantId')) || '';
        this.fetchMenu(this.restaurantId);
      } 
    });
  }

  filterMenu() {
    if (this.showVegOnly && this.showNonVegOnly) {
      this.filteredMenuItems = this.menuItems;
    } else if (this.showVegOnly) {
      this.filteredMenuItems = this.menuItems.filter(item => item.isVeg);
    } else if (this.showNonVegOnly) {
      this.filteredMenuItems = this.menuItems.filter(item => !item.isVeg);
    } else {
      this.filteredMenuItems = this.menuItems;
    }
  }


  fetchMenuByCuisine(cuisine: string): void {
    const apiUrl = `https://localhost:7072/api/Menu/cuisine?cuisineTypes=${cuisine}`;

    this.http.get<any[]>(apiUrl).subscribe(
      data => {
        this.menuItems = data;
        this.isLoading = false;
        this.filterMenu();
      },
      error => {
        console.error('Error fetching menu by cuisine:', error);
      }
    );
  }

  fetchMenu(restaurantId: string): void {
    this.menuService.getMenuItemsByRestaurantId(Number(restaurantId))
    .subscribe((data) => {
      console.log("res", data);
      this.menuItems = data;
      this.isLoading = false;
      this.filterMenu();
    });
  }

  addToCart(item: MenuItem): void {
    this.cartService.addToCart(this.userId, item);
  }

  incrementQuantity(itemId: number): void {
    this.cartService.incrementItem(this.userId, itemId);
  }

  decrementQuantity(itemId: number): void {
    this.cartService.decrementItem(this.userId, itemId);
  }


  isInCart(itemId: number): boolean {
    return this.cartService.isInCart(itemId);
  }

  getItemQuantity(itemId: number): number {
    return this.cartService.getItemQuantity(itemId);
  }

  
  
}
