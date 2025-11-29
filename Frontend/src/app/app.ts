import { Component, inject } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from './service/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent {
  title = 'SIGA-PET';
  searchQuery: string = '';
  
  // Injeção de dependência correta
  public authService = inject(AuthService);
  private router = inject(Router);

  // CORREÇÃO CRÍTICA: Atribuir o Signal a uma propriedade
  currentUser = this.authService.getCurrentUser();

  searchProducts(): void {
    if (this.searchQuery.trim()) {
      this.router.navigate(['/produtos'], { 
        queryParams: { search: this.searchQuery } 
      });
    }
  }

  logout(event: Event): void {
    event.preventDefault();
    this.authService.logout();
  }
}
