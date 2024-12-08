import { Component } from '@angular/core';
import { RestaurantService } from '../../services/restaurant.service';
import { AuthService } from '../../services/auth.service';
import { OwnerSidebarComponent } from "../owner-sidebar/owner-sidebar.component";
import { FeedbackService } from '../../services/feedback.service';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';


@Component({
  selector: 'app-owner-feedback',
  standalone: true,
  imports: [OwnerSidebarComponent, MatCardModule, CommonModule, MatListModule],
  templateUrl: './owner-feedback.component.html',
  styleUrl: './owner-feedback.component.scss'
})
export class OwnerFeedbackComponent {
  ownerId:number = 0;
  restaurants: any[] = [];
  selectedRestaurant: any = null;
  feedbacks: any[] = [];



  constructor(
    private restaurantService: RestaurantService,
    private authService: AuthService,
    private feedbackService: FeedbackService
  ) {}

  ngOnInit():void{
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
    this.fetchFeedback(restaurant.restaurantId);
  }
  fetchFeedback(restaurantId: number) {
    this.feedbackService.getFeedbackByRestaurant(restaurantId).subscribe(
      (data) => {
        this.feedbacks = data;
      },
      (error) => {
        console.error('Error fetching feedback:', error);
      }
    );
  }

}
