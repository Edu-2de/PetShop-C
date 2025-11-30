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
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.carregarProduto(Number(id));
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

  setImagemPrincipal(url: string) {
    // Lógica para corrigir URL se vier do backend
    const fullUrl = url.startsWith('http') ? url : `http://localhost:5000/${url}`;
    this.imagemPrincipal.set(fullUrl);
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
