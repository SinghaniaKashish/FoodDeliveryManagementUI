import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item.model';
import { MatDialog } from '@angular/material/dialog';
import { PaymentModalComponent } from '../payment-modal/payment-modal.component';
import { MatDialogModule } from '@angular/material/dialog';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatCardModule } from '@angular/material/card';


@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule,MatDialogModule, MatCardModule, MatListModule, MatButtonModule, MatIconModule, NavbarComponent],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})


export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  totalAmount: number = 0;
  userId: number = Number(localStorage.getItem('userId') || 0);

  constructor(private cartService: CartService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.cartService.cartItems$.subscribe((items) =>
      {
        this.cartItems = items;
        this.totalAmount = this.cartService.calculateTotal();
      });
    this.cartService.loadCart(this.userId); 
  }

  incrementQuantity(itemId: number): void {
    this.cartService.incrementItem(this.userId, itemId);
  }

  decrementQuantity(itemId: number): void {
    this.cartService.decrementItem(this.userId, itemId);
  }

  removeItem(itemId: number): void {
    this.cartService.removeItem(this.userId, itemId);
  }

  calculateTotal(): number {
    return this.cartService.calculateTotal();
  }


  checkout(): void {
    const dialogRef = this.dialog.open(PaymentModalComponent, {
      width: '400px',
      data: {
        restaurantId: 0, 
        cartItems: this.cartItems, 
        totalAmount: this.totalAmount, 
      },
    });
  
    dialogRef.afterClosed().subscribe((result) => {
      if (result?.success) {
        this.cartService.clearCart(this.userId);
      }
    });
  }
  
}