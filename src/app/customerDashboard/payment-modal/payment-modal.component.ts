import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { PaymentService } from '../../services/payment.service';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../services/order.service';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';



@Component({
  selector: 'app-payment-modal',
  standalone: true,
  imports: [MatRadioModule,CommonModule, MatInputModule, MatDialogModule, FormsModule, MatIconModule, MatCardModule, MatFormFieldModule],
  templateUrl: './payment-modal.component.html',
  styleUrl: './payment-modal.component.scss'
})


export class PaymentModalComponent {
  userId: number = Number(localStorage.getItem('userId') || 0);
  paymentMethod: string = '';
  deliveryAddress: string = '';
  restaurantId: number;
  cartItems: any[];
  totalAmount: number;


  constructor(
    private dialogRef: MatDialogRef<PaymentModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private orderService: OrderService,
    private paymentService: PaymentService
  ) {
    this.restaurantId = data.restaurantId;
    this.cartItems = data.cartItems;
    this.totalAmount = data.totalAmount;
  }

  onCancel(): void {
    this.dialogRef.close({ success: false });
  }

  onPay(): void {
    const orderPayload = {
      customerId: this.userId,
      restaurantId: this.restaurantId,
      totalAmount: this.totalAmount,
      orderDate: new Date(),
      status: 'Pending',
      deliveryAddress: this.deliveryAddress,
      paymentMethod : this.paymentMethod,
      orderDetails: this.cartItems.map((item: any) => ({
        itemId: item.menuItemId,
        quantity: item.quantity,
        price: item.price,
        totalPrice: item.quantity * item.price,
      })),
    };

    console.log('placing order:', orderPayload);

      
// Place Order
this.orderService.placeOrder(orderPayload).subscribe(
  (orderResponse) => {
    console.log('Order placed successfully:', orderResponse);

    // Payment Payload
    const paymentPayload = {
      orderId: orderResponse.orderId, 
      amount: this.totalAmount,
      paymentMethod: this.paymentMethod,
      paymentStatus: 'Paid',
      paymentDate: new Date(),
    };

    console.log('Processing payment:', paymentPayload);
    this.paymentService.addPayment(paymentPayload).subscribe(
      (paymentResponse) => {
        console.log('Payment processed successfully:', paymentResponse);
        setTimeout(() => {
          this.dialogRef.close({ success: true });
        }, 2000);
      },
      (error) => {
        console.error('Error processing payment:', error);
      }
    );
  },
  (error) => {
    console.error('Error placing order:', error);
  }
);

  }
}
