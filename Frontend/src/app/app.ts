import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; // Importação única
import { FormsModule } from '@angular/forms';
import { AuthService } from './service/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule, // Substitui RouterOutlet, RouterLink, RouterLinkActive
    FormsModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent {
  title = 'SIGA-PET';
  searchQuery: string = '';

  constructor(public authService: AuthService, private router: Router) { }

  logout(event: Event): void {
    event.preventDefault();
    this.authService.logout();
  }

  currentUser() {
    return this.authService.getCurrentUser();
  }

  searchProducts(): void {
    if (this.searchQuery.trim()) {
      // Navega para a rota de produtos com o parâmetro de busca (exemplo)
      // Você pode precisar ajustar a lógica do ProdutoListComponent para ler isso
      console.log('Buscando por:', this.searchQuery);
      // this.router.navigate(['/produtos'], { queryParams: { q: this.searchQuery } });
    }
  }
}
