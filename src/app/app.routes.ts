import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { CartComponent } from './customerDashboard/cart/cart.component';
import { CustomerComponent } from './customerDashboard/customer/customer.component';
import { MenuComponent } from './customerDashboard/menu/menu.component';
import { ProfileComponent } from './customerDashboard/profile/profile.component';
import { RestaurantsComponent } from './customerDashboard/restaurants/restaurants.component';
import { SearchComponent } from './customerDashboard/search/search.component';
import { DriverDashboardComponent } from './driverDashboard/driver/driver.component';
import { MenuCrudComponent } from './ownerDashboard/menu-crud/menu-crud.component';
import { OwnerFeedbackComponent } from './ownerDashboard/owner-feedback/owner-feedback.component';
import { OwnerInventoryComponent } from './ownerDashboard/owner-inventory/owner-inventory.component';
import { OwnerOrdersComponent } from './ownerDashboard/owner-order/owner-order.component';
import { OwnerDashboardComponent } from './ownerDashboard/owner/owner.component';
import { RestaurantcrudComponent } from './ownerDashboard/restaurantcrud/restaurantcrud.component';
import { HeroSectionComponent } from './customerDashboard/hero-section/hero-section.component';
import { AuthGuard } from './guards/auth.guard';


export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'signup', component: SignupComponent },
    { path: 'customer/dashboard', component: CustomerComponent },
    { path: 'herosection', component: HeroSectionComponent },
    { path: 'profile', component: ProfileComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Customer'] }},
    { path: 'search', component: SearchComponent },
    { path: 'cart', component: CartComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Customer'] }},
    { path: 'restaurants', component: RestaurantsComponent },
    { path: 'restaurants/:restaurantId/menu', component: MenuComponent },
    { path: 'owner/dashboard', component: OwnerDashboardComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Owner'] }  },
    { path: 'owner/restaurant', component:RestaurantcrudComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Owner'] } },
    { path: 'owner/menu', component:MenuCrudComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Owner'] } },
    { path: 'owner/order', component:OwnerOrdersComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Owner'] } },
    { path: 'owner/feedback', component:OwnerFeedbackComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Owner'] } },
    { path: 'owner/inventory', component:OwnerInventoryComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Owner'] } },
    { path: 'driver/dashboard', component: DriverDashboardComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Driver'] }  },
    
    { path: '', redirectTo: '/customer/dashboard', pathMatch: 'full' },
    { path: '**', redirectTo: 'login', pathMatch: 'full' },
];
