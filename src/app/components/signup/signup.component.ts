import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterModule
  ],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent {
  user = {
    username: '',
    email: '',
    phoneNumber: '',
    passwordHash: '',
    role: '',
  };
  errorMessage: string = '';
  roles = ['Customer', 'Owner', 'Driver', 'Admin'];

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.signup(this.user).subscribe({
      next: () => {
        this.errorMessage = '';
        this.router.navigate(['/login']);
      },
      error: (err) => {
        if (err.status === 400 && err.error.message === 'Email is already registered.') {
          this.errorMessage = 'Email is already registered.';
        } 
        else if (err.error.errors.Email && err.error.errors.Email.includes('Invalid email address.')) {
          this.errorMessage = 'Invalid email address.';
        } else {
          this.errorMessage = 'An error occurred. Please try again later.';
        }
        console.error('Signup failed', err);
      },
    });
  }
}
