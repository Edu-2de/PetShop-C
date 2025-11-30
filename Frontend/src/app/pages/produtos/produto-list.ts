import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Produto } from '../../model/produto.model';
import { ProdutoService } from '../../service/produtos/produto.service';
import { AuthService } from '../../service/auth/auth.service';
import { CategoriaService } from '../../service/categorias/categoria.service';
import { Categoria } from '../../model/categoria.model';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './produto-list.html',
  styleUrls: ['./produto-list.scss']
})
export class ProdutoListComponent implements OnInit {
  // Services
  public authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private produtoService = inject(ProdutoService);
  private categoriaService = inject(CategoriaService);

  // Signals de Dados
  produtos = signal<Produto[]>([]);
  categorias = signal<Categoria[]>([]);

  // Signals de Filtro
  termoBusca = signal<string>('');
  categoriaSelecionada = signal<string>(''); // Vazio = Todas
  filtroPrecoMax = signal<number>(1000); // Slider de preço

  // Lógica Reativa de Filtragem
  produtosFiltrados = computed(() => {
    let lista = this.produtos();
    const termo = this.termoBusca().toLowerCase();
    const catId = this.categoriaSelecionada();
    const precoMax = this.filtroPrecoMax();

    // 1. Filtro por Busca Texto
    if (termo) {
      lista = lista.filter(p =>
        p.nome.toLowerCase().includes(termo) ||
        (p.descricao && p.descricao.toLowerCase().includes(termo))
      );
    }

    // 2. Filtro por Categoria
    if (catId) {
      lista = lista.filter(p => p.categoria === catId || p.categoriaId?.toString() === catId);
    }

    // 3. Filtro por Preço
    lista = lista.filter(p => p.preco <= precoMax);

    return lista;
  });

  ngOnInit(): void {
    this.carregarDados();

    // Verifica se veio busca da home
    this.route.queryParams.subscribe(params => {
      if (params['busca']) {
        this.termoBusca.set(params['busca']);
      }
    });
  }

  carregarDados(): void {
    // Carrega Produtos com tratamento de erro
    this.produtoService.listar().subscribe({
      next: (data) => {
        const prods = this.authService.isAdmin() ? data : data.filter(p => p.ativo);
        this.produtos.set(prods);
      },
      error: (err) => {
        console.error('Erro crítico na API:', err);
        // Define lista vazia para evitar travamento da tela
        this.produtos.set([]);
      }
    });

    // Carrega Categorias
    this.categoriaService.listar().subscribe(cats => {
      this.categorias.set(cats);
    });
  }

  // --- Ações (Métodos da Classe) ---

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
    if (confirm('Deseja excluir este produto?')) {
      this.produtoService.delete(id).subscribe(() => {
        this.produtos.update(current => current.filter(p => p.id !== id));
      });
    }
  }

  comprar(produto: Produto): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    alert(`"${produto.nome}" adicionado ao carrinho!`);
  }

  handleImageError(event: any) {
    event.target.src = 'assets/images/no-image.png';
  }
}
