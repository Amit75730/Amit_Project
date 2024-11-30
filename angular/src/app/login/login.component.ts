import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  onLogin(): void {
    if (!this.username.trim() || !this.password.trim()) {
      this.errorMessage = 'Username and password are required.';
      return;
    }

    this.isLoading = true;
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.token) {
          this.authService.saveToken(response.token);
          this.router.navigate(['/required-fields']);
        } else {
          this.errorMessage = 'Login successful, but no token received.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Invalid credentials.';
      },
    });
  }
}
