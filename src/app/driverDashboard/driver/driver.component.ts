import { Component, OnInit } from '@angular/core';
import { DeliveryService } from '../../services/delivery.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { MatCardModule } from '@angular/material/card';
import {MatExpansionModule} from '@angular/material/expansion';
import { Router } from '@angular/router';

@Component({
  selector: 'app-driver',
  standalone: true,
  imports: [CommonModule, MatCardModule,MatExpansionModule ],
  templateUrl: './driver.component.html',
  styleUrl: './driver.component.scss'
})



export class DriverDashboardComponent implements OnInit {

  userId: number = Number(localStorage.getItem('userId'));
  userName = this.authService.getUserName();
  userEmail = this.authService.getUserEmail();
  deliveries: any[] = [];
  feedback: any = null;


  constructor(
    public authService: AuthService,
    private deliveryService: DeliveryService,
    private router: Router

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
    this.loadDeliveries();
    this.fetchFeedback(this.userId);
  }

  
  loadDeliveries(): void {  
    this.deliveryService.getDeliveriesByDriver(this.userId).subscribe(
      (response) => {
        this.deliveries = response;
      },
      (error) => {
        console.error('Error fetching deliveries:', error);
      }
    );
  }

  markAsDelivered(deliveryId: number): void {
    this.deliveryService.updateDeliveryStatus(deliveryId, 'Delivered').subscribe(
      () => {
        this.loadDeliveries();
      },
      (error) => {
        console.error('Error updating delivery status:', error);
      }
    );
  }

  fetchFeedback(driverId: number) {
    this.deliveryService.getFeedbackByDriver(driverId).subscribe(
      (data) => {
        this.feedback = data;
        console.log(this.feedback);
      },
      (error) => {
        console.error('Error fetching feedback:', error);
      }
    );
  }

  
}