import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProdutoService } from '../../service/produtos/produto.service';
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

  // Injeção de dependência moderna
  private produtoService = inject(ProdutoService);

  ngOnInit() {
    this.carregarProdutos();
  }

  carregarProdutos() {
    this.produtoService.listar().subscribe({
      next: (data) => {
        // Pega apenas produtos ativos e limita a 8 para a vitrine
        this.produtos = data.filter(p => p.ativo).slice(0, 8);
        this.produtosCarregando = false;
      },
      error: (err) => {
        console.error('Erro ao carregar produtos', err);
        this.produtosCarregando = false;
      }
    });
  }

  getImagem(produto: Produto): string {
    if (produto.imagens && produto.imagens.length > 0) {
      return produto.imagens[0].url;
    }
    return 'assets/images/no-image.png'; // Placeholder
  }
}
