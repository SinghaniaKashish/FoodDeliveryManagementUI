import { Component } from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import {MatFormFieldModule} from '@angular/material/form-field';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatCardModule, MatFormFieldModule, CommonModule, FormsModule, MatInputModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})

export class LoginComponent {
  credentials = { email: '', password: '' };
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.errorMessage = ''; 
    this.authService.login(this.credentials).subscribe(
      (response) => {
        this.authService.saveToken(response.token);
        localStorage.setItem('userId', response.userId.toString());
        const role = response.role;
        localStorage.setItem('userRole', role);
  
        if (role === 'Customer') {
          this.router.navigate(['/customer/dashboard']);
        } else if (role === 'Owner') {
          this.router.navigate(['/owner/dashboard']);
        } else if (role === 'Driver') {
          this.router.navigate(['/driver/dashboard']);
        } else if (role === 'Admin') {
          this.router.navigate(['/admin/dashboard']);
        }
      },
      (error) => {
        this.errorMessage = 'Invalid email or password. Please try again.';
        console.error('Login failed', error);
      }
    );
  }
  
}
