import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Fornecedor } from '../../../model/fornecedor.model';
import { FormsModule } from '@angular/forms';
// CORREÇÃO: Importando do arquivo 'forncedor' (sem 'e', conforme seu arquivo real)
import { FornecedorService } from '../../../service/fornecedor/forncedor';

@Component({
    selector: 'app-fornecedor-list',
    standalone: true,
    imports: [CommonModule, RouterLink, FormsModule],
    templateUrl: './forncedor-list.html',
    styleUrls: ['./forncedor-list.scss']
})
export class ForncedorListComponent implements OnInit {
    private fornecedorService = inject(FornecedorService);

    fornecedores = signal<Fornecedor[]>([]);
    termoBusca = signal<string>('');

    fornecedoresFiltrados = computed(() => {
        const fornecedores = this.fornecedores();
        const termo = this.termoBusca().toLowerCase();
        if (!termo) return fornecedores;
        return fornecedores.filter(f => f.nome.toLowerCase().includes(termo));
    });

    constructor() { }

    ngOnInit(): void {
        this.carregarFornecedores();
    }

    carregarFornecedores(): void {
        this.fornecedorService.listar().subscribe((data: Fornecedor[]) => {
            this.fornecedores.set(data);
        });
    }

    buscar(event: Event): void {
        const target = event.target as HTMLInputElement;
        this.termoBusca.set(target.value);
    }

    excluir(id: number | undefined): void {
        if (id === undefined) return;
        if (confirm('Deseja realmente excluir este fornecedor?')) {
            this.fornecedorService.delete(id).subscribe(() => {
                this.fornecedores.update(atuais => atuais.filter(f => f.fornecedorId !== id));
            });
        }
    }
}
