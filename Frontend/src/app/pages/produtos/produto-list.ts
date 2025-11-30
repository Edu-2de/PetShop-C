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
  public authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private produtoService = inject(ProdutoService);
  private categoriaService = inject(CategoriaService);

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
    const cat = this.categorias().find(c => c.id === id);
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
        this.produtos.update(curr => curr.filter(p => p.id !== id));
      });
    }
  }

  comprar(produto: Produto): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    alert(`Adicionado ao carrinho!`);
  }
}
