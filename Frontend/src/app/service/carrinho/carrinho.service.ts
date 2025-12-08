import { Injectable, signal, computed } from '@angular/core';
import { Produto } from '../../model/produto.model';

export interface ItemCarrinho {
  produto: Produto;
  quantidade: number;
}

@Injectable({
  providedIn: 'root'
})
export class CarrinhoService {
  private itens = signal<ItemCarrinho[]>([]);
  private readonly STORAGE_KEY = 'sigapet_carrinho';

  // Computed para valores derivados
  totalItens = computed(() => 
    this.itens().reduce((total, item) => total + item.quantidade, 0)
  );

  valorTotal = computed(() => 
    this.itens().reduce((total, item) => total + (item.produto.preco * item.quantidade), 0)
  );

  constructor() {
    this.carregarDoLocalStorage();
  }

  getItens() {
    return this.itens.asReadonly();
  }

  adicionarItem(produto: Produto, quantidade: number = 1): void {
    const itensAtuais = [...this.itens()];
    const itemExistente = itensAtuais.find(i => i.produto.produtoId === produto.produtoId);

    if (itemExistente) {
      // Verifica estoque antes de adicionar mais
      const novaQuantidade = itemExistente.quantidade + quantidade;
      if (novaQuantidade > produto.quantidadeEstoque) {
        // Sem alert - apenas retorna silenciosamente
        return;
      }
      itemExistente.quantidade = novaQuantidade;
    } else {
      // Novo item
      if (quantidade > produto.quantidadeEstoque) {
        // Sem alert - apenas retorna silenciosamente
        return;
      }
      itensAtuais.push({ produto, quantidade });
    }

    this.itens.set(itensAtuais);
    this.salvarNoLocalStorage();
  }

  removerItem(produtoId: number): void {
    this.itens.update(itens => itens.filter(i => i.produto.produtoId !== produtoId));
    this.salvarNoLocalStorage();
  }

  atualizarQuantidade(produtoId: number, quantidade: number): void {
    if (quantidade <= 0) {
      this.removerItem(produtoId);
      return;
    }

    const itensAtuais = [...this.itens()];
    const item = itensAtuais.find(i => i.produto.produtoId === produtoId);
    
    if (item) {
      if (quantidade > item.produto.quantidadeEstoque) {
        // Sem alert - apenas retorna silenciosamente
        return;
      }
      item.quantidade = quantidade;
      this.itens.set(itensAtuais);
      this.salvarNoLocalStorage();
    }
  }

  limpar(): void {
    this.itens.set([]);
    localStorage.removeItem(this.STORAGE_KEY);
  }

  // Método para carregar o carrinho do localStorage na inicialização
  carregarCarrinho(): void {
    const carrinhoSalvo = localStorage.getItem('carrinho');
    if (carrinhoSalvo) {
      try {
        const itens = JSON.parse(carrinhoSalvo);
        this.itens.set(itens);
      } catch (error) {
        console.error('Erro ao carregar carrinho do localStorage:', error);
        localStorage.removeItem('carrinho');
      }
    }
  }

  // Método para obter quantidade total de itens
  getQuantidadeTotal(): number {
    return this.itens().reduce((total, item) => total + item.quantidade, 0);
  }

  private salvarNoLocalStorage(): void {
    try {
      localStorage.setItem(this.STORAGE_KEY, JSON.stringify(this.itens()));
    } catch (error) {
      console.error('Erro ao salvar carrinho:', error);
    }
  }

  private carregarDoLocalStorage(): void {
    try {
      const dados = localStorage.getItem(this.STORAGE_KEY);
      if (dados) {
        const itensCarregados = JSON.parse(dados) as ItemCarrinho[];
        this.itens.set(itensCarregados);
      }
    } catch (error) {
      console.error('Erro ao carregar carrinho:', error);
      this.limpar();
    }
  }
}
