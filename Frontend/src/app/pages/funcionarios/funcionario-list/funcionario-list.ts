import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FuncionarioService, Funcionario } from '../../../service/funcionarios/funcionario.service';

@Component({
  selector: 'app-funcionario-list',
  standalone: true,
  imports: [CommonModule, RouterLink, DatePipe],
  template: `
    <div class="container mt-4">
      <div class="card shadow-sm">
        <div class="card-header bg-light py-3 d-flex justify-content-between align-items-center">
          <h2 class="mb-0">Equipe</h2>
          <a routerLink="/funcionarios/novo" class="btn btn-primary"><i class="bi bi-person-plus me-1"></i> Contratar</a>
        </div>
        <div class="card-body">
          <table class="table table-hover align-middle">
            <thead class="table-light">
              <tr>
                <th>Nome</th>
                <th>Cargo</th>
                <th>Email</th>
                <th>Telefone</th>
                <th>Contratação</th>
                <th class="text-end">Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let f of funcionarios()">
                <td class="fw-medium">{{ f.nome }}</td>
                <td><span class="badge bg-secondary">{{ f.cargo }}</span></td>
                <td>{{ f.email }}</td>
                <td>{{ f.telefone }}</td>
                <td>{{ f.dataContratacao | date:'dd/MM/yyyy' }}</td>
                <td class="text-end">
                  <a [routerLink]="['/funcionarios/editar', f.funcionarioId]" class="btn btn-sm btn-outline-info me-2">Editar</a>
                  <button (click)="excluir(f.funcionarioId)" class="btn btn-sm btn-outline-danger">Excluir</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `
})
export class FuncionarioListComponent implements OnInit {
  funcionarios = signal<Funcionario[]>([]);
  private service = inject(FuncionarioService);

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.service.listar().subscribe(data => this.funcionarios.set(data));
  }

  excluir(id: number) {
    if (confirm('Tem certeza? Isso removerá o acesso deste usuário.')) {
      this.service.deletar(id).subscribe(() => this.carregar());
    }
  }
}
