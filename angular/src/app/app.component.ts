import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { containerDetailsComponent } from './container-details/container-details.component';

import { ReactiveFormsModule } from '@angular/forms';
import { WatchlistComponent } from './watchlist/watchlist.component';
import { CartComponent } from './cart/cart.component';
import { LoginComponent } from './login/login.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, containerDetailsComponent, WatchlistComponent, CartComponent, LoginComponent, ReactiveFormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'EDI_Angular';
}