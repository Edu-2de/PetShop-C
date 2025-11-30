import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Produto } from '../../../model/produto.model';
import { ProdutoService } from '../../../service/produtos/produto.service';
import { AuthService } from '../../../service/auth/auth.service';

@Component({
  selector: 'app-produto-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './produto-detail.html'
})
export class ProdutoDetailComponent implements OnInit {
  produto = signal<Produto | null>(null);
  imagemPrincipal = signal<string>('');
  quantidade = signal<number>(1);

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private produtoService = inject(ProdutoService);
  public authService = inject(AuthService);

  ngOnInit() {
    // 1. Pega o ID "codificado" da URL
    const idCodificado = this.route.snapshot.paramMap.get('id');

    if (idCodificado) {
      try {
        // 2. Decodifica para número (atob converte Base64 para texto)
        const idReal = Number(atob(idCodificado));

        if (!isNaN(idReal)) {
          this.carregarProduto(idReal);
        } else {
          // Se alguém tentar digitar um ID inválido manual
          this.router.navigate(['/produtos']);
        }
      } catch (e) {
        this.router.navigate(['/produtos']);
      }
    }
  }

  carregarProduto(id: number) {
    this.produtoService.findById(id).subscribe({
      next: (prod) => {
        this.produto.set(prod);
        // Define imagem inicial
        if (prod.imagens && prod.imagens.length > 0) {
          this.setImagemPrincipal(prod.imagens[0].url);
        } else {
          this.imagemPrincipal.set('assets/images/no-image.png');
        }
      },
      error: () => this.router.navigate(['/produtos'])
    });
  }

  // --- CORREÇÃO DA IMAGEM ---
  setImagemPrincipal(url: string) {
    if (url.startsWith('http')) {
      // Se já é um link completo (externo)
      this.imagemPrincipal.set(url);
    } else if (url.startsWith('assets')) {
      // Se é uma imagem local do projeto
      this.imagemPrincipal.set(url);
    } else {
      // Se veio do backend (caminho relativo), adiciona o servidor
      this.imagemPrincipal.set(`http://localhost:5000/${url}`);
    }
  }

  inc() { this.quantidade.update(q => q + 1); }
  dec() { this.quantidade.update(q => (q > 1 ? q - 1 : 1)); }

  comprar() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    alert(`Adicionado ${this.quantidade()}x "${this.produto()?.nome}" ao carrinho!`);
  }

  handleImageError(event: any) {
    event.target.src = 'assets/images/no-image.png';
  }
}
