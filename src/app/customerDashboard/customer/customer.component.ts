import { Component } from '@angular/core';
import { RestaurantsComponent } from "../../customerDashboard/restaurants/restaurants.component";
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { HeroSectionComponent } from "../hero-section/hero-section.component";

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [NavbarComponent, RouterOutlet, HeroSectionComponent],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.scss'
})
export class CustomerComponent {

}
