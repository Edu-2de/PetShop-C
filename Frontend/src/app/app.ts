import { Component, signal } from '@angular/core';
import { RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './service/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  title = 'siga-pet-app';
  searchQuery: string = '';

  constructor(
    public authService: AuthService,
    private router: Router
  ) {}

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  searchProducts(): void {
    if (this.searchQuery.trim()) {
      this.router.navigate(['/produtos'], { 
        queryParams: { search: this.searchQuery } 
      });
    }
  }

  logout(event: Event): void {
    event.preventDefault();
    if (confirm('Deseja realmente sair?')) {
      this.authService.logout();
    }
  }
}
