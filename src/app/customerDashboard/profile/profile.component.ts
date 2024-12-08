import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog } from '@angular/material/dialog';
import { FeedbackDialogComponent } from '../feedback-dialog/feedback-dialog.component';
import { NavbarComponent } from '../navbar/navbar.component';


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, 
    NavbarComponent, 
    MatCardModule, 
    MatDividerModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})


export class ProfileComponent implements OnInit {
  userId: number = Number(localStorage.getItem('userId'));
  userName = this.authService.getUserName();
  userEmail = this.authService.getUserEmail();
  orders: any[] = []; 

  constructor(
    private orderService: OrderService,
    private authService: AuthService, 
    private router: Router,
    private dialog: MatDialog
  ) {}


  ngOnInit(): void {
    this.userId = this.authService.getUserId(); 
    if (this.userId) {
      this.authService.fetchUserById(this.userId).subscribe(
        (user) => {
          if (user) {
            this.userName = user.username; 
            this.userEmail = user.email;
  
            this.authService.saveTokenAndUserData(this.authService.getToken() || '', user);
          }
        },
        (error) => {
          console.error('Error fetching user data:', error);
        }
      );
    } else {
      console.error('No userId found!');
      this.router.navigate(['/login']); 
    }
  
    this.loadOrders();
  }
  

  loadOrders(): void {
    this.orderService.getOrdersByUserId(this.userId).subscribe(
      (response) => {
        this.orders = response;
      },
      (error) => {
        console.error('Error fetching orders:', error);
      }
    );
  }

  logout() {
    this.authService.logout();
  }

  openRatingDialog(order: any) {
    this.dialog.open(FeedbackDialogComponent, {
      width: '400px',
      data: {
        orderId: order.orderId,
        restaurantId: order.restaurantId,
        customerId: order.customerId,
        driverId: order.driverId,
      },
    });
  }

}
