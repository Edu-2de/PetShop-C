import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Produto } from '../../model/produto.model';
import { ProdutoService } from '../../service/produtos/produto.service';
import { AuthService } from '../../service/auth/auth.service';
import { CategoriaService } from '../../service/categorias/categoria.service';
import { Categoria } from '../../model/categoria.model';
import { CarrinhoService } from '../../service/carrinho/carrinho.service';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './produto-list.html',
  styleUrls: ['./produto-list.scss']
})
export class ProdutoListComponent implements OnInit {
  public authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private produtoService = inject(ProdutoService);
  private categoriaService = inject(CategoriaService);
  private carrinhoService = inject(CarrinhoService);

  produtos = signal<Produto[]>([]);
  categorias = signal<Categoria[]>([]);

  termoBusca = signal<string>('');
  categoriaSelecionada = signal<string>('');
  filtroPrecoMax = signal<number>(1000);

  produtosFiltrados = computed(() => {
    let lista = this.produtos();
    const termo = this.termoBusca().toLowerCase();
    const catNome = this.categoriaSelecionada();
    const precoMax = this.filtroPrecoMax();

    // Filtro Busca
    if (termo) {
      lista = lista.filter(p =>
        p.nome.toLowerCase().includes(termo) ||
        (p.descricao && p.descricao.toLowerCase().includes(termo))
      );
    }

    // Filtro Categoria (Compara o NOME da categoria recuperado)
    if (catNome) {
      lista = lista.filter(p => this.getNomeCategoria(p.categoriaId) === catNome);
    }

    // Filtro Preço
    lista = lista.filter(p => p.preco <= precoMax);

    return lista;
  });

  ngOnInit(): void {
    this.carregarDados();

    // ATUALIZAÇÃO: Ler parâmetros da URL (Busca OU Categoria)
    this.route.queryParams.subscribe(params => {
      // Se veio busca por texto
      if (params['busca']) {
        this.termoBusca.set(params['busca']);
      }

      // Se veio clique na categoria da Home
      if (params['categoria']) {
        this.categoriaSelecionada.set(params['categoria']);
      }
    });
  }

  carregarDados(): void {
    // Carrega Produtos
    this.produtoService.listar().subscribe({
      next: (data) => {
        const prods = this.authService.isAdmin() ? data : data.filter(p => p.ativo);
        this.produtos.set(prods);
      },
      error: (err) => {
        console.error(err);
        this.produtos.set([]);
      }
    });

    // Carrega Categorias
    this.categoriaService.listar().subscribe(cats => {
      this.categorias.set(cats);
    });
  }

  // --- CORREÇÕES E UTILITÁRIOS ---

  // Converte o ID da categoria em Nome
  getNomeCategoria(id: number | undefined): string {
    if (!id) return 'Geral';
    const cat = this.categorias().find(c => c.categoriaId === id);
    return cat ? cat.nome : 'Geral';
  }

  // Corrige URL da imagem
  getImagemUrl(produto: Produto): string {
    if (!produto.imagens || produto.imagens.length === 0) {
      return 'assets/images/no-image.png';
    }
    const url = produto.imagens[0].url;
    if (url.startsWith('http') || url.startsWith('assets')) {
      return url;
    }
    return `http://localhost:5000/${url}`;
  }

  codificarId(id: number | undefined): string {
    return id ? btoa(id.toString()) : '';
  }

  handleImageError(event: any) {
    event.target.src = 'assets/images/no-image.png';
  }

  // --- AÇÕES ---

  selecionarCategoria(catNome: string) {
    this.categoriaSelecionada.set(catNome);
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  atualizarPreco(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.filtroPrecoMax.set(Number(target.value));
  }

  excluir(id: number | undefined): void {
    if (!id) return;
    if (confirm('Deseja excluir?')) {
      this.produtoService.delete(id).subscribe(() => {
        this.produtos.update(curr => curr.filter(p => p.produtoId !== id));
      });
    }
  }

  adicionarAoCarrinho(produto: Produto): void {
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
