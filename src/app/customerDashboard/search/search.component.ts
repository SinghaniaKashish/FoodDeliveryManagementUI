import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatIconModule } from '@angular/material/icon';



@Component({
  selector: 'app-search',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatCardModule, CommonModule, ReactiveFormsModule, FormsModule, MatIconModule, NavbarComponent],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent {
  keyword: string = '';
  cuisineType: string = '';
  restaurants: any[] = [];

  private apiUrl = 'https://localhost:7072/api';

  constructor(private http: HttpClient, private router: Router) {}

  searchRestaurants() {
    const params = {
      keyword: this.keyword || '',
      cuisineType: this.cuisineType || '',
    };

    this.http
      .get<any[]>(`${this.apiUrl}/Restaurant/search`, { params })
      .subscribe({
        next: (data) => (this.restaurants = data),
        error: (err) => console.error('Search failed:', err),
      });
  }

  viewMenu(restaurantId: number) :void{
    console.log("res id", restaurantId);
    this.router.navigate([`/restaurants/${restaurantId}/menu`]);
  }
}