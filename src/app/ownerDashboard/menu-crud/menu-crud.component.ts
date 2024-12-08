import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatDialog } from '@angular/material/dialog';
import { MenuService } from '../../services/menu.service';
import { RestaurantService } from '../../services/restaurant.service';
import { MenuItemDialogComponent } from '../menu-item-dialog/menu-item-dialog.component';
import { MenuItem } from '../../models/menu-item.model'; 
import { MatSnackBar } from '@angular/material/snack-bar';
import { OwnerSidebarComponent } from "../owner-sidebar/owner-sidebar.component";
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-menu-crud',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatCardModule, MatListModule, OwnerSidebarComponent],
  templateUrl: './menu-crud.component.html',
  styleUrls: ['./menu-crud.component.scss']
})


export class MenuCrudComponent implements OnInit {
  restaurants: any[] = [];
  selectedRestaurant: any = null;
  menuItems: MenuItem[] = []; 
  ownerId:number = 0;

  constructor(
    private menuService: MenuService,
    private restaurantService: RestaurantService,
    private dialog: MatDialog ,
    private snackBar: MatSnackBar,
    private authService :AuthService
  ) {}

  ngOnInit(): void {
    this.ownerId = this.authService.getUserId();
    if (this.ownerId) {
      this.fetchRestaurants(this.ownerId);
    } else {
      console.error('Owner ID not found in local storage.');
    }
  }

  getOwnerIdFromLocalStorage(): number | null {
    const userId = localStorage.getItem('userId');
    return userId ? parseInt(userId, 10) : null;
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
    this.getMenuItems();
  }

  getMenuItems() {
    if (this.selectedRestaurant) {
      this.menuService.getMenuItemsByRestaurantId(this.selectedRestaurant.restaurantId).subscribe(
        (data: MenuItem[]) => {
          this.menuItems = data;
        },
        (error) => {
          console.error('Error fetching menu items:', error);
        }
      );
    }
  }

  deleteMenuItem(itemId: number) {
    this.menuService.deleteMenuItem(itemId).subscribe(() => {
      this.menuItems = this.menuItems.filter((item) => item.itemId !== itemId);
    });
  }


  addMenuItem() {
  const dialogRef = this.dialog.open(MenuItemDialogComponent, {
    width: '400px'
  });

  dialogRef.afterClosed().subscribe((result: MenuItem | undefined) => {
    if (result) {
      const menuItemWithIngredients = {
        ...result,
        ingredients: result.ingredients && result.ingredients.length > 0 ? result.ingredients : [{
          ingredientName: "Default Ingredient", 
          quantity: 1
        }]
      };

      menuItemWithIngredients.restaurantId = this.selectedRestaurant.restaurantId;

      // Send the menu item data to the service
      this.menuService.addMenuItem(this.selectedRestaurant.restaurantId, menuItemWithIngredients)
        .subscribe({
          next: (newItem) => {
            this.menuItems.push(newItem); 
            console.log('Menu item added successfully:', newItem);
          },
          error: (err) => {
            console.error('Error adding menu item:', err);
            console.error('Validation errors:', err.error.errors); 
          }
        });
    }
  });
}

  

editMenuItem(menuItem: MenuItem) {
  console.log('Editing menu item:', menuItem);
  const dialogRef = this.dialog.open(MenuItemDialogComponent, {
    width: '400px',
    data: { ...menuItem } 
  });

  dialogRef.afterClosed().subscribe((updatedMenuItem: MenuItem | undefined) => {
    if (updatedMenuItem) {
      // Update logic here
      this.menuService.updateMenuItem(updatedMenuItem.itemId, updatedMenuItem)
  .subscribe(() => {
    const index = this.menuItems.findIndex(item => item.itemId === updatedMenuItem.itemId);
    if (index > -1) {
      this.menuItems[index] = updatedMenuItem; 
    }
    this.snackBar.open('Menu item updated successfully!', 'Close', { duration: 3000 });
  });

    }
  });
}

}
