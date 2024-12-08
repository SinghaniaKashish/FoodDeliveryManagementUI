import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withFetch,withInterceptors } from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { authInterceptor } from './app/interceptors/auth.interceptor';



bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withFetch(),withInterceptors([authInterceptor])), 
    provideRouter(routes),
    importProvidersFrom(NoopAnimationsModule),
    MatToolbarModule,
    MatIconModule
  ],
}).catch(err => console.error(err));
