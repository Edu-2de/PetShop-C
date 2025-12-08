import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Produto } from '../../../model/produto.model';
import { ProdutoService } from '../../../service/produtos/produto.service';
import { AuthService } from '../../../service/auth/auth.service';
import { CarrinhoService } from '../../../service/carrinho/carrinho.service';
import { VendaService } from '../../../service/vendas/venda.service';
import { CreateVendaDto } from '../../../model/venda.model';

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
  processandoCompra = false;

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private produtoService = inject(ProdutoService);
  public authService = inject(AuthService);
  private carrinhoService = inject(CarrinhoService);
  private vendaService = inject(VendaService);

  ngOnInit() {
    const idCodificado = this.route.snapshot.paramMap.get('id');

    if (idCodificado) {
      try {
        const idReal = Number(atob(idCodificado));

        if (!isNaN(idReal)) {
          this.carregarProduto(idReal);
        } else {
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
    if (url.startsWith('http')) {
      this.imagemPrincipal.set(url);
    } else if (url.startsWith('assets')) {
      this.imagemPrincipal.set(url);
    } else {
      this.imagemPrincipal.set(`http://localhost:5000/${url}`);
    }
  }

  inc() { 
    const produto = this.produto();
    if (produto && this.quantidade() < produto.quantidadeEstoque) {
      this.quantidade.update(q => q + 1);
    } else {
      alert('Quantidade máxima atingida (estoque disponível)');
    }
  }
  
  dec() { 
    this.quantidade.update(q => (q > 1 ? q - 1 : 1)); 
  }

  adicionarAoCarrinho() {
    const produto = this.produto();
    if (!produto) return;

    this.carrinhoService.adicionarItem(produto, this.quantidade());
    alert(`${produto.nome} adicionado ao carrinho!`);
  }

  comprarAgora() {
    const produto = this.produto();
    if (!produto) return;

    if (!this.authService.isAuthenticated()) {
      alert('Você precisa fazer login para comprar.');
      this.router.navigate(['/login']);
      return;
    }

    const currentUser = this.authService.getCurrentUserValue();
    if (!currentUser) {
      alert('Erro: Usuário não identificado.');
      return;
    }

    console.log('🛒 DEBUG: Iniciando compra para USUÁRIO:', currentUser);

    this.processandoCompra = true;

    // ✅ CORRIGIDO: Sempre incluir usuarioId para rastreamento da compra
    const novaVenda: CreateVendaDto = {
      usuarioId: currentUser.usuarioId, // ✅ SEMPRE enviar o ID do usuário
      tutorId: currentUser.tutorId || null, // Pode ser null se usuário não for tutor
      // Dados do usuário para criar tutor se necessário
      nomeCliente: currentUser.nome,
      emailCliente: currentUser.email,
      telefoneCliente: '', // Opcional
      enderecoCliente: '', // Opcional
      formaPagamento: 'Cartão de Crédito',
      observacoes: `Compra do usuário: ${currentUser.nome} (${currentUser.email})`,
      itens: [{
        produtoId: produto.produtoId,
        quantidade: this.quantidade()
      }]
    };

    console.log('🛒 DEBUG: Dados da venda:', novaVenda);

    this.vendaService.criar(novaVenda).subscribe({
      next: (vendaCriada) => {
        console.log('✅ DEBUG: Venda criada com sucesso:', vendaCriada);
        alert(`Compra realizada com sucesso! 
        
Produto: ${produto.nome}
Quantidade: ${this.quantidade()}
Total: ${(produto.preco * this.quantidade()).toLocaleString('pt-BR', {style: 'currency', currency: 'BRL'})}`);
        
        this.router.navigate(['/vendas/minhas']);
      },
      error: (err) => {
        console.error('❌ Erro ao realizar compra:', err);
        const mensagem = err.error?.message || err.error || 'Falha ao realizar a compra. Tente novamente.';
        alert(`Erro na compra: ${mensagem}`);
        this.processandoCompra = false;
      },
      complete: () => {
        this.processandoCompra = false;
      }
    });
  }

  handleImageError(event: any) {
    event.target.src = 'assets/images/no-image.png';
  }
}
