import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { OwnerSidebarComponent } from "../owner-sidebar/owner-sidebar.component";
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-restaurantcrud',
  standalone: true,
  imports: [CommonModule, MatInputModule, MatCardModule, ReactiveFormsModule, FormsModule, MatFormFieldModule, MatSlideToggleModule, OwnerSidebarComponent],
  templateUrl: './restaurantcrud.component.html',
  styleUrl: './restaurantcrud.component.scss'
})


export class RestaurantcrudComponent implements OnInit{
  restaurantForm!: FormGroup;
  restaurants: any[] = [];
  isEditMode: boolean = false;
  editingRestaurantId: number | null = null;

  constructor(
    private fb: FormBuilder, 
    private http: HttpClient,
    private snackBar:MatSnackBar
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.fetchRestaurants();
  }

  initForm(): void {
    this.restaurantForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      address: ['', [Validators.required, Validators.maxLength(250)]],
      cuisineTypes: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      hoursOfOperation: ['', Validators.required],
      imagePath: [''],
      status: [true]
    });
  }

  fetchRestaurants(): void {
    const userId = localStorage.getItem('userId');
    console.log("user id =", userId);
    this.http.get<any[]>(`https://localhost:7072/api/Restaurant/ownerId?ownerId=${userId}`).subscribe({
      next: (data) => this.restaurants = data,
      error: (err) => console.error('Error fetching restaurants:', err)
    });
  }
  

  onSubmit(): void {
    const userId = localStorage.getItem('userId');
    const restaurantData = {
      ...this.restaurantForm.value,
      cuisineTypes: this.restaurantForm.value.cuisineTypes.split(',').map((type: string) => type.trim()),
      userId: userId
    };
    const payload = {
      restaurantId: this.editingRestaurantId,
      userId: parseInt(restaurantData.userId, 10),
      name: restaurantData.name,
      address: restaurantData.address,
      cuisineTypes: Array.isArray(restaurantData.cuisineTypes) ? restaurantData.cuisineTypes : [restaurantData.cuisineTypes],
      phone: restaurantData.phone,
      hoursOfOperation: restaurantData.hoursOfOperation,
      status: restaurantData.status,
      imagePath: restaurantData.imagePath
    };

    if (this.isEditMode && this.editingRestaurantId) {
      console.log(restaurantData);
      this.http.put(`https://localhost:7072/api/Restaurant/${this.editingRestaurantId}`, payload).subscribe({
        next: () => {
          this.fetchRestaurants();
          this.resetForm();
        },
        error: (err) => console.error('Error updating restaurant:', err)
      });
    } else {
      this.http.post('https://localhost:7072/api/Restaurant', restaurantData).subscribe({
        next: () => {
          this.fetchRestaurants();
          this.resetForm();
          this.snackBar.open('Restaurant updated successfully!', 'Close', { duration: 3000 });

        },
        error: (err) => {
          console.error('Update failed:', err.error);
          if (err.status === 400 && err.error.message) {
            this.snackBar.open(err.error.message, 'Close', { duration: 5000 });
          } else {
            this.snackBar.open('Failed to update restaurant. Please try again.', 'Close', { duration: 3000 });
          }
        },      });
    }
  }

  editRestaurant(restaurant: any): void {
    this.isEditMode = true;
    this.editingRestaurantId = restaurant.restaurantId;
    this.restaurantForm.patchValue({
      ...restaurant,
      cuisineTypes: restaurant.cuisineTypes.join(', ')
    });
  }

  deleteRestaurant(restaurantId: number): void {
    this.http.delete(`https://localhost:7072/api/Restaurant/${restaurantId}`).subscribe({
      next: () => this.fetchRestaurants(),
      error: (err) => console.error('Error deleting restaurant:', err)
    });
  }

  cancelEdit(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.restaurantForm.reset({
      name: '',
      address: '',
      cuisineTypes: '',
      phone: '',
      hoursOfOperation: '',
      imagePath: '',
      status: true
    });
    this.isEditMode = false;
    this.editingRestaurantId = null;
  }

  getErrorMessage(controlName: string): string {
    const control = this.restaurantForm.get(controlName);
    if (control?.hasError('required')) return 'This field is required.';
    if (control?.hasError('maxlength')) return 'Exceeds maximum allowed characters.';
    if (control?.hasError('pattern')) return 'Invalid format.';
    return '';
  }

  
  
}
