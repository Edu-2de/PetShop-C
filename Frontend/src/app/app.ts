import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
// CORREÇÃO: Adicionado 'RouterLinkActive' aos imports
import { Router, RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { AuthService } from './service/auth/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  // CORREÇÃO: Adicionado 'RouterLinkActive' na lista de imports do componente
  imports: [CommonModule, RouterLink, RouterOutlet, RouterLinkActive, FormsModule],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class AppComponent {
  title = 'SIGA-PET';
  authService = inject(AuthService);
  private router = inject(Router);

  currentUser = this.authService.getCurrentUser();
  searchQuery: string = '';

  logout(event: Event) {
    event.preventDefault();
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  searchProducts() {
    if (this.searchQuery.trim()) {
      this.router.navigate(['/produtos'], { queryParams: { busca: this.searchQuery } });
    }
  }
}
