import { Routes } from '@angular/router';
import { Home } from './components/pages/home/home';
import { LoginComponent } from './components/shared/login-component/login-component';
import { guestGuard } from './guards/GuestGuards';
import { authGuard } from './guards/AuthGuards';

export const routes: Routes = [
    { 
        path: '', 
        component: LoginComponent ,
        canActivate: [guestGuard]
    },
   { 
        path: 'login', 
        component: LoginComponent,
        canActivate: [guestGuard] 
        
    },
    { 
        path: 'home', 
        canActivate: [authGuard],
        component: Home
    },
   {
        path:'new-request',
        canActivate: [authGuard],
        data: {expectedRole: 'user'},
        loadComponent() {
            return import('./components/pages/new-request/new-request').then(m => m.NewRequest);
        }
   },
   {
    path:'request-detail/:id',
    canActivate: [authGuard],
    loadComponent() {
        return import('./components/pages/request-detail/request-detail').then(m => m.RequestDetail);
    } 
   },
       { 
        path: '**', 
        redirectTo: '' 
    },
];
