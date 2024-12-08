import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { OwnerSidebarComponent } from "../../ownerDashboard/owner-sidebar/owner-sidebar.component";
import { StatisticsService } from '../../services/statistics.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { MatSpinner } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-owner',
  standalone: true,
  imports: [MatCardModule, OwnerSidebarComponent, CommonModule, MatSpinner],
  templateUrl: './owner.component.html',
  styleUrl: './owner.component.scss'
})

export class OwnerDashboardComponent implements OnInit {
  ownerId:number = 0;
  ownerName: string = '';
  ownerEmail: string = '';
  restaurants: any[] = [];
  loading: boolean = true;

  constructor(
    private statisticsService: StatisticsService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    
    this.ownerId = this.authService.getUserId();
    this.ownerName = this.authService.getUserName();
    this.ownerEmail = this.authService.getUserEmail();
    this.loadStatistics();
  }

  loadStatistics(): void {
    this.statisticsService.getOwnerStatistics(this.ownerId).subscribe(
      (data) => {
        this.restaurants = data;
        this.loading = false;
      },
      (error) => {
        console.error('Error fetching statistics:', error);
        this.loading = false;
      }
    );
  }

  logout(): void {
    this.authService.logout(); 
  }
}