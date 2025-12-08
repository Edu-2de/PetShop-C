import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ProdutoService } from '../../service/produtos/produto.service';
import { CarrinhoService } from '../../service/carrinho/carrinho.service';
import { Produto } from '../../model/produto.model';

@Component({
  selector: 'app-card-produtos',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './card-produtos.html',
  styleUrls: ['./card-produtos.scss']
})
export class CardProdutosComponent implements OnInit {
  produtos: Produto[] = [];
  produtosCarregando = true;

  private produtoService = inject(ProdutoService);
  private carrinhoService = inject(CarrinhoService);
  private router = inject(Router);

  ngOnInit() {
    this.carregarProdutos();
  }

  carregarProdutos() {
    this.produtoService.listar().subscribe({
      next: (data: Produto[]) => {
        this.produtos = data.filter((p: Produto) => p.ativo).slice(0, 8);
        this.produtosCarregando = false;
      },
      error: (err: any) => {
        console.error('Erro ao carregar produtos', err);
        this.produtosCarregando = false;
      }
    });
  }

  getImagem(produto: Produto): string {
    if (produto.imagens && produto.imagens.length > 0) {
      const url = produto.imagens[0].url;
      return url.startsWith('assets') ? url : `http://localhost:5000/${url}`;
    }
    return 'assets/images/no-image.png';
  }

  codificarId(id: number | undefined): string {
    return id ? btoa(id.toString()) : '';
  }

  irParaDetalhes(produto: Produto): void {
    const idCodificado = this.codificarId(produto.produtoId);
    this.router.navigate(['/produtos', idCodificado]);
  }

  adicionarAoCarrinho(event: Event, produto: Produto): void {
    event.preventDefault();
    event.stopPropagation();

    if (!produto.produtoId) {
      console.error('Produto sem ID');
      return;
    }

    if (produto.quantidadeEstoque <= 0) {
      this.mostrarToast('Produto sem estoque', 'error');
      return;
    }

    this.carrinhoService.adicionarItem(produto, 1);
    this.mostrarToast(`${produto.nome} adicionado ao carrinho!`, 'success');
  }

  private mostrarToast(mensagem: string, tipo: 'success' | 'error'): void {
    // Criar elemento toast
    const toastContainer = document.getElementById('toast-container') || this.criarToastContainer();
    
    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${tipo === 'success' ? 'success' : 'danger'} border-0`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    
    toast.innerHTML = `
      <div class="d-flex">
        <div class="toast-body">
          <i class="bi bi-${tipo === 'success' ? 'check-circle' : 'exclamation-circle'} me-2"></i>
          ${mensagem}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
      </div>
    `;
    
    toastContainer.appendChild(toast);
    
    // Inicializar e mostrar o toast usando Bootstrap
    const bsToast = new (window as any).bootstrap.Toast(toast, {
      delay: 3000
    });
    bsToast.show();
    
    // Remover toast após ser ocultado
    toast.addEventListener('hidden.bs.toast', () => {
      toast.remove();
    });
  }

  private criarToastContainer(): HTMLElement {
    const container = document.createElement('div');
    container.id = 'toast-container';
    container.className = 'toast-container position-fixed top-0 end-0 p-3';
    container.style.zIndex = '9999';
    document.body.appendChild(container);
    return container;
  }
}
