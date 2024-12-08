import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';


@Component({
  selector: 'app-hero-section',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './hero-section.component.html',
  styleUrl: './hero-section.component.scss'
})
export class HeroSectionComponent {
  constructor(private router: Router) {}

  viewMenuByCuisine(cuisine: string): void {
    this.router.navigate(['/restaurants/:restaurantId/menu'], { queryParams: { cuisine } });
  }
}
