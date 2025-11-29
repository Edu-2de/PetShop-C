import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CategoriaService } from '../../../service/categorias/categoria.service';
import { Categoria } from '../../../model/categoria.model';

@Component({
  selector: 'app-categoria-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="container mt-4">
      <div class="card shadow-sm" style="max-width: 600px; margin: 0 auto;">
        <div class="card-header bg-light py-3">
          <h2 class="mb-0">Nova Categoria</h2>
        </div>
        <div class="card-body">
          <form (ngSubmit)="salvar()">
            <div class="mb-3">
              <label class="form-label">Nome da Categoria</label>
              <input type="text" class="form-control" [(ngModel)]="categoria.nome" name="nome" required>
            </div>
            <div class="mb-3">
              <label class="form-label">Descrição</label>
              <textarea class="form-control" rows="3" [(ngModel)]="categoria.descricao" name="descricao"></textarea>
            </div>
            <div class="d-flex justify-content-end gap-2">
              <a routerLink="/categorias" class="btn btn-secondary">Cancelar</a>
              <button type="submit" class="btn btn-primary">Salvar</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `
})
export class CategoriaFormComponent {
  categoria: Partial<Categoria> = { nome: '', descricao: '' };
  private service = inject(CategoriaService);
  private router = inject(Router);

  salvar() {
    this.service.criar(this.categoria).subscribe(() => {
      this.router.navigate(['/categorias']);
    });
  }
}
