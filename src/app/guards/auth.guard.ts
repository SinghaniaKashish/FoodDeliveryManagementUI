import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = this.authService.getToken();

    if (token) {
      const allowedRoles = route.data['roles'] as string[];
      const userRole = localStorage.getItem('userRole') || ''; 
      if (!allowedRoles || allowedRoles.includes(userRole)) {
        return true;
      } else {
        this.router.navigate(['/login']); 
        return false;
      }
    }

    this.router.navigate(['/login']);
    return false;
  }
}
