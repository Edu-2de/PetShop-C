import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { Produto } from '../../model/produto.model';
import { ProdutoService } from '../../service/produtos/produto.service';
import { Fornecedor } from '../../model/fornecedor.model';
// CORREÇÃO: Importando do arquivo 'forncedor' (sem 'e')
import { FornecedorService } from '../../service/fornecedor/forncedor';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../service/auth/auth.service';

@Component({
    selector: 'app-produto-list',
    standalone: true,
    imports: [CommonModule, RouterLink, FormsModule],
    templateUrl: './produto-list.html',
    styleUrls: ['./produto-list.scss']
})
export class ProdutoListComponent implements OnInit {
    private produtos = signal<Produto[]>([]);
    termoBusca = signal<string>('');

    public authService = inject(AuthService);
    private router = inject(Router);
    private produtoService = inject(ProdutoService);
    private fornecedorService = inject(FornecedorService);

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

    constructor() { }

    ngOnInit(): void {
        this.carregarDados();
    }

    carregarDados(): void {
        this.produtoService.listar().subscribe((produtos: Produto[]) => {
            this.produtos.set(produtos);
        });
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

    comprar(produto: Produto): void {
        if (!this.authService.isAuthenticated()) {
            this.router.navigate(['/login']);
            return;
        }
        alert(`Produto "${produto.nome}" adicionado ao carrinho!`);
    }
}
