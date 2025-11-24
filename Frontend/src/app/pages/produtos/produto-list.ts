import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Produto } from '../../model/produto.model';
import { ProdutoService } from '../../service/produtos/produto.service';
import { Fornecedor } from '../../model/fornecedor.model';
import { FornecedorService } from '../../service/fornecedor/fornecedor';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './produto-list.html',
  styleUrls: ['./produto-list.scss']
})
export class ProdutoListComponent implements OnInit {
  private produtos = signal<Produto[]>([]);
  private fornecedorMap = new Map<number, string>();
  termoBusca = signal<string>('');

  produtosFiltrados = computed(() => {
    const produtos = this.produtos();
    const termo = this.termoBusca().toLowerCase();
    if (!termo) {
      return produtos;
    }
    return produtos.filter(produto =>
      produto.nome.toLowerCase().includes(termo) ||
      (produto.categoria && produto.categoria.toLowerCase().includes(termo))
    );
  });

  constructor(
    private produtoService: ProdutoService,
    private fornecedorService: FornecedorService
  ) {}

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    this.fornecedorService.listar().subscribe((fornecedores: Fornecedor[]) => {
      this.fornecedorMap = new Map(fornecedores.map((f: Fornecedor) => [f.id!, f.nome]));
      this.produtoService.listar().subscribe(produtos => {
        this.produtos.set(produtos);
      });
    });
  }

  getFornecedorNome(id: number | undefined): string {
    if (id === undefined) return 'N/A';
    return this.fornecedorMap.get(id) || 'Desconhecido';
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  excluir(id: number | undefined): void {
    if (id === undefined) return;
    if (confirm('Deseja realmente excluir este produto?')) {
      this.produtoService.delete(id).subscribe(() => {
        this.produtos.update(produtosAtuais => produtosAtuais.filter(p => p.id !== id));
      });
    }
  }
}
