import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-feedback-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, FormsModule, MatIconModule, MatRadioModule, MatInputModule ],
  templateUrl: './feedback-dialog.component.html',
  styleUrl: './feedback-dialog.component.scss'
})

export class FeedbackDialogComponent {
  feedback = {
    foodRating: 0,
    deliveryTimeRating: 0,
    deliveryDriverRating: 0,
    comments: '',
  };
  userId:number = Number(localStorage.getItem('userId'));

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private http: HttpClient,
    private dialogRef: MatDialogRef<FeedbackDialogComponent>,
    
  ) {}

  submitFeedback() {
    const feedbackData = {
      OrderId: this.data.orderId,
      RestaurantId: this.data.restaurantId,
      CustomerId: this.userId,
      DriveId: this.data.driverId,
      FoodRating: this.feedback.foodRating,
      DeliveryTimeRating: this.feedback.deliveryTimeRating,
      DeliveryDriverRating: this.feedback.deliveryDriverRating,
      Comments: this.feedback.comments,
      FeedbackDate: new Date().toISOString(),
    };

    this.http.post('https://localhost:7072/api/Feedback', feedbackData).subscribe({
      next: () => {
        this.dialogRef.close();
      },
      error: (err) => {
        console.error('Error submitting feedback', err);
      },
    });
  }
}