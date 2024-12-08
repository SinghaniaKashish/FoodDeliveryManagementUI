import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { MenuItem } from '../../models/menu-item.model';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-menu-item-dialog',
  standalone: true,
  imports: [MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    FormsModule,
    CommonModule,
    MatIconModule],
  templateUrl: './menu-item-dialog.component.html',
  styleUrl: './menu-item-dialog.component.scss'
})

export class MenuItemDialogComponent {
  menuItem: MenuItem = {
    itemId: 0,
    restaurantId: 0,
    name: '',
    category: '',
    price: 0,
    cuisineType: '',
    availability: true,
    isVeg: false,
    imagePath: '',
    ingredients: [{
        ingredientUsageId: 0, 
        itemId: 0,           
        ingredientName: '',
        quantity: 0
    }]
  };

  constructor(
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<MenuItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MenuItem
  ) {
    if (data) {
      this.menuItem = { ...data };
    }
  }

  addIngredient() {
    this.menuItem.ingredients.push({ ingredientUsageId: 0, 
      itemId: 0,  ingredientName: '', quantity: 0 });
  }

  removeIngredient(index: number) {
    this.menuItem.ingredients.splice(index, 1);
  }
  onSave() {
    const invalidIngredients = this.menuItem.ingredients.filter(ingredient =>
      !ingredient.ingredientName || ingredient.quantity < 0.01 || ingredient.quantity > 1000
    );
  
    if (invalidIngredients.length > 0) {
      console.error('Validation errors:', invalidIngredients);
      this.snackBar.open('Please fix validation errors before saving.', 'Close', { duration: 3000 });
      return;
    }
    console.log('Menu Item Data:', this.menuItem);
    this.dialogRef.close(this.menuItem);
  }


  onCancel() {
    this.dialogRef.close();
  }


}
