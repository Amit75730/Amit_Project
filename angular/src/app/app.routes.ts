import { Routes } from '@angular/router';
import { containerDetailsComponent } from './container-details/container-details.component';
import { WatchlistComponent } from './watchlist/watchlist.component';
import { CartComponent } from './cart/cart.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth-guard.service';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' }, // Redirect to login on root path
    { path: 'login', component: LoginComponent }, // Login page
    { path: 'required-fields', component: containerDetailsComponent, canActivate: [AuthGuard] }, // Requires authentication
    { path: 'watchlist', component: WatchlistComponent, canActivate: [AuthGuard] }, // Watchlist, authentication required
    { path: 'cart/:containerNumber', component: CartComponent, canActivate: [AuthGuard] }, // Cart for specific container, auth required
    { path: '**', redirectTo: 'login' }, // Redirect all undefined routes to login
];
