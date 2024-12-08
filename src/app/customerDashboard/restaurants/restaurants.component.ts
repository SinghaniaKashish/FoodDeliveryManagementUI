import { Component, HostListener, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatGridList, MatGridListModule} from '@angular/material/grid-list';
import { MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { RestaurantService } from '../../services/restaurant.service';
import { Router } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-restaurants',
  standalone: true,
  imports: [MatCardModule, MatGridListModule, 
    MatProgressSpinnerModule, CommonModule, NavbarComponent, MatIconModule
  ],
  templateUrl: './restaurants.component.html',
  styleUrl: './restaurants.component.scss'
})

export class RestaurantsComponent implements OnInit {
  restaurants: any[] = [];
  isLoading: boolean = true;

  gridCols = 4;
  gutterSize = '16px';

  constructor(private restaurantService: RestaurantService,private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchRestaurants();
    this.calculateGridCols(window.innerWidth);
  }

  fetchRestaurants(): void {
    this.http.get<any[]>('https://localhost:7072/api/Restaurant').subscribe({
      next: (data) => {
        console.log('restaurants fetched');
        this.restaurants = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching restaurants:', err);
        this.isLoading = false;
      },
    });
  }

  viewMenu(restaurantId: number): void {
    this.router.navigate([`/restaurants/${restaurantId}/menu`]);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event): void {
    const width = (event.target as Window).innerWidth;
    this.calculateGridCols(width);
  }

  private calculateGridCols(width: number): void {
    if (width < 576) {
      this.gridCols = 1;
    } else if (width >= 576 && width < 768) {
      this.gridCols = 2; 
    } else if (width >= 768 && width < 992) {
      this.gridCols = 3; 
    } else {
      this.gridCols = 4;
    }
  }

}
