import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { AuthService } from './service/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { CarrinhoComponent } from './components/carrinho/carrinho.component';
import { CarrinhoService } from './service/carrinho/carrinho.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet, RouterLinkActive, FormsModule, CarrinhoComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class AppComponent implements OnInit {
  title = 'SIGA-PET';
  
  public authService = inject(AuthService);
  public carrinhoService = inject(CarrinhoService);
  private router = inject(Router);

  currentUser = this.authService.getCurrentUser();
  searchQuery: string = '';

  ngOnInit(): void {
    // Carrega o carrinho do localStorage na inicialização
    this.carrinhoService.carregarCarrinho();
  }

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

  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  getCurrentUser() {
    return this.authService.getCurrentUser()();
  }

  getQuantidadeItensCarrinho(): number {
    return this.carrinhoService.getQuantidadeTotal();
  }
}
