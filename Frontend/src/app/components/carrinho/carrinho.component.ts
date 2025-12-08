import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { CarrinhoService } from '../../service/carrinho/carrinho.service';
import { VendaService } from '../../service/vendas/venda.service';
import { AuthService } from '../../service/auth/auth.service';
import { CreateVendaDto } from '../../model/venda.model';

@Component({
  selector: 'app-carrinho',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <!-- Dropdown do Carrinho (como no perfil do usuário) -->
    <div class="dropdown">
      <a href="#" 
         class="d-flex align-items-center gap-2 text-decoration-none position-relative" 
         data-bs-toggle="dropdown"
         title="Meu Carrinho">
        <i class="bi bi-cart3 text-secondary" style="font-size: 1.25rem;"></i>
        <span *ngIf="totalItens() > 0" 
              class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
              style="font-size: 0.65rem; padding: 0.25em 0.5em; margin-top: -8px;">
          {{ totalItens() }}
        </span>
      </a>

      <!-- Dropdown Menu -->
      <div class="dropdown-menu dropdown-menu-end shadow-lg border-0 mt-2 rounded-3 carrinho-dropdown">
        <!-- Header -->
        <div class="dropdown-header bg-light border-bottom px-3 py-2">
          <h6 class="mb-0 fw-bold text-dark">
            <i class="bi bi-cart3 me-2"></i>Meu Carrinho
          </h6>
        </div>

        <!-- Itens -->
        <div class="carrinho-itens-container" *ngIf="itens().length > 0">
          <div *ngFor="let item of itens()" class="dropdown-item px-2 py-2 border-bottom">
            <div class="d-flex gap-2">
              <!-- Imagem -->
              <div class="bg-light rounded d-flex align-items-center justify-content-center flex-shrink-0" 
                   style="width: 50px; height: 50px; overflow: hidden;">
                <img [src]="getImagemUrl(item.produto)" 
                     [alt]="item.produto.nome"
                     style="width: 100%; height: 100%; object-fit: cover;"
                     (error)="handleImageError($event)">
              </div>

              <!-- Info -->
              <div class="flex-grow-1 min-w-0">
                <h6 class="mb-1 fw-bold small text-truncate" [title]="item.produto.nome">
                  {{ item.produto.nome }}
                </h6>
                <p class="mb-1 text-primary fw-semibold small">
                  {{ item.produto.preco | currency:'BRL' }}
                </p>

                <!-- Quantidade -->
                <div class="btn-group btn-group-sm" role="group">
                  <button type="button" 
                          class="btn btn-outline-secondary btn-sm"
                          (click)="diminuirQuantidade(item.produto.produtoId!)"
                          style="padding: 0.15rem 0.4rem; font-size: 0.7rem;">
                    <i class="bi bi-dash"></i>
                  </button>
                  <button type="button" class="btn btn-outline-secondary btn-sm" disabled
                          style="padding: 0.15rem 0.4rem; font-size: 0.7rem;">
                    {{ item.quantidade }}
                  </button>
                  <button type="button" 
                          class="btn btn-outline-secondary btn-sm"
                          (click)="aumentarQuantidade(item.produto.produtoId!)"
                          style="padding: 0.15rem 0.4rem; font-size: 0.7rem;">
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
              </div>

              <!-- Remover -->
              <button class="btn btn-sm btn-link text-danger p-0" 
                      (click)="remover(item.produto.produtoId!)"
                      title="Remover"
                      style="font-size: 0.9rem;">
                <i class="bi bi-trash"></i>
              </button>
            </div>
          </div>
        </div>

        <!-- Vazio -->
        <div *ngIf="itens().length === 0" class="dropdown-item text-center py-4 px-3">
          <i class="bi bi-cart-x d-block mb-2" style="font-size: 2rem; color: #d1d5db;"></i>
          <p class="text-muted small mb-1 fw-medium">Seu carrinho está vazio</p>
          <p class="text-muted small mb-0" style="font-size: 0.8rem;">Adicione produtos para continuar</p>
        </div>

        <!-- Divider se tiver itens -->
        <div *ngIf="itens().length > 0" class="dropdown-divider m-0"></div>

        <!-- Footer com Total e Botões -->
        <div *ngIf="itens().length > 0" class="px-3 py-2 bg-light">
          <!-- Erro -->
          <div *ngIf="mensagemErro" class="alert alert-danger py-1 px-2 mb-2 small" role="alert">
            <i class="bi bi-exclamation-triangle me-1"></i>
            {{ mensagemErro }}
          </div>

          <!-- Total -->
          <div class="d-flex justify-content-between align-items-center mb-2 pb-2 border-bottom">
            <span class="fw-bold small">Total:</span>
            <span class="fw-bold text-primary">{{ valorTotal() | currency:'BRL' }}</span>
          </div>

          <!-- Botões -->
          <button class="btn btn-primary btn-sm w-100 mb-2 fw-semibold"
                  (click)="finalizarCompra()"
                  [disabled]="processando"
                  style="font-size: 0.9rem; padding: 0.4rem;">
            <i class="bi bi-check-circle me-1"></i>
            {{ processando ? 'Processando...' : 'Finalizar Compra' }}
          </button>

          <button class="btn btn-outline-danger btn-sm w-100 fw-semibold"
                  (click)="limparCarrinho()"
                  style="font-size: 0.85rem; padding: 0.3rem;">
            <i class="bi bi-trash me-1"></i>Limpar
          </button>
        </div>

        <!-- Link para ver produtos -->
        <div *ngIf="itens().length === 0" class="px-3 py-2">
          <a routerLink="/produtos" 
             class="btn btn-primary btn-sm w-100"
             style="font-size: 0.9rem;">
            Ver Produtos
          </a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .carrinho-dropdown {
      width: 400px;
      max-height: 600px;
      overflow-y: auto;
    }

    .carrinho-itens-container {
      max-height: 350px;
      overflow-y: auto;

      &::-webkit-scrollbar {
        width: 6px;
      }

      &::-webkit-scrollbar-track {
        background: #f3f4f6;
      }

      &::-webkit-scrollbar-thumb {
        background: #d1d5db;
        border-radius: 3px;

        &:hover {
          background: #9ca3af;
        }
      }
    }

    .min-w-0 {
      min-width: 0;
    }

    @media (max-width: 576px) {
      .carrinho-dropdown {
        width: 100vw;
        max-width: calc(100% + 32px);
        left: -16px;
      }
    }
  `]
})
export class CarrinhoComponent {
  private carrinhoService = inject(CarrinhoService);
  private vendaService = inject(VendaService);
  private authService = inject(AuthService);
  private router = inject(Router);

  itens = this.carrinhoService.getItens();
  totalItens = this.carrinhoService.totalItens;
  valorTotal = this.carrinhoService.valorTotal;
  processando = false;
  mensagemErro = '';

  getImagemUrl(produto: any): string {
    if (!produto.imagens || produto.imagens.length === 0) {
      return 'assets/images/no-image.png';
    }
    const url = produto.imagens[0].url;
    if (url.startsWith('http') || url.startsWith('assets')) {
      return url;
    }
    return `http://localhost:5000/${url}`;
  }

  handleImageError(event: any) {
    event.target.src = 'assets/images/no-image.png';
  }

  aumentarQuantidade(produtoId: number): void {
    const item = this.itens().find(i => i.produto.produtoId === produtoId);
    if (item) {
      this.carrinhoService.atualizarQuantidade(produtoId, item.quantidade + 1);
    }
  }

  diminuirQuantidade(produtoId: number): void {
    const item = this.itens().find(i => i.produto.produtoId === produtoId);
    if (item && item.quantidade > 1) {
      this.carrinhoService.atualizarQuantidade(produtoId, item.quantidade - 1);
    }
  }

  remover(produtoId: number): void {
    if (confirm('Remover este item do carrinho?')) {
      this.carrinhoService.removerItem(produtoId);
    }
  }

  limparCarrinho(): void {
    if (confirm('Deseja realmente limpar todo o carrinho?')) {
      this.carrinhoService.limpar();
    }
  }

  finalizarCompra(): void {
    this.mensagemErro = '';

    if (!this.authService.isAuthenticated()) {
      this.mensagemErro = 'Você precisa fazer login para finalizar a compra.';
      setTimeout(() => {
        this.router.navigate(['/login']);
      }, 1500);
      return;
    }

    const currentUser = this.authService.getCurrentUserValue();
    if (!currentUser) {
      this.mensagemErro = 'Erro: Usuário não identificado.';
      return;
    }

    this.processando = true;

    // 🆕 NOVO: Permite compra mesmo sem tutorId
    // Se não tem tutorId, vai criar tutor automaticamente no backend
    const novaVenda: CreateVendaDto = {
      tutorId: currentUser.tutorId || null, // Permite null
      formaPagamento: 'Cartão de Crédito',
      itens: this.itens().map(item => ({
        produtoId: item.produto.produtoId,
        quantidade: item.quantidade
      })),
      // 🆕 Se não tem tutorId, fornecer dados para criar tutor
      nomeCliente: !currentUser.tutorId ? currentUser.nome : undefined,
      emailCliente: !currentUser.tutorId ? currentUser.email : undefined,
      telefoneCliente: !currentUser.tutorId ? '' : undefined, // Usuário pode atualizar depois
      enderecoCliente: !currentUser.tutorId ? 'A definir' : undefined
    };

    this.vendaService.criar(novaVenda).subscribe({
      next: (vendaCriada) => {
        this.carrinhoService.limpar();
        
        // Fechar dropdown
        const dropdown = document.querySelector('.dropdown-toggle[aria-expanded="true"]');
        if (dropdown) {
          (dropdown as HTMLElement).click();
        }
        
        // Mostrar sucesso e redirecionar
        if (!currentUser.tutorId) {
          alert('✅ Compra realizada com sucesso!\n\n📝 Agora você é um cliente cadastrado e pode fazer agendamentos.');
          // Atualizar user no AuthService para incluir tutorId se foi criado
          // Idealmente, deveria fazer reload do token ou atualizar o user
          this.router.navigate(['/']);
        } else {
          alert('✅ Compra realizada com sucesso!');
          this.router.navigate(['/minhas-compras']);
        }
      },
      error: (err) => {
        console.error('Erro ao finalizar compra:', err);
        this.mensagemErro = err.error?.message || 'Falha ao realizar a compra. Tente novamente.';
      },
      complete: () => {
        this.processando = false;
      }
    });
  }
}
