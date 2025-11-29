import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CategoriaService } from '../../../service/categorias/categoria.service';
import { Categoria } from '../../../model/categoria.model';

@Component({
  selector: 'app-categoria-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="container mt-4">
      <div class="card shadow-sm">
        <div class="card-header bg-light py-3 d-flex justify-content-between align-items-center">
          <h2 class="mb-0">Categorias de Produtos</h2>
          <a routerLink="/categorias/novo" class="btn btn-primary"><i class="bi bi-plus-circle me-1"></i> Nova Categoria</a>
        </div>
        <div class="card-body">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nome</th>
                <th>Descrição</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let cat of categorias()">
                <td>{{ cat.categoriaId || cat.id }}</td>
                <td>{{ cat.nome }}</td>
                <td>{{ cat.descricao || '-' }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `
})
export class CategoriaListComponent implements OnInit {
  categorias = signal<Categoria[]>([]);
  private service = inject(CategoriaService);

  ngOnInit() {
    this.service.listar().subscribe(data => this.categorias.set(data));
  }
}
