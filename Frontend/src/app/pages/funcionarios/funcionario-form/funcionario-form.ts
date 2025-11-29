import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FuncionarioService, Funcionario } from '../../../service/funcionarios/funcionario.service';

@Component({
  selector: 'app-funcionario-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="container mt-4">
      <div class="card shadow-sm">
        <div class="card-header bg-light py-3">
          <h2 class="mb-0">{{ isEdit ? 'Editar Funcionário' : 'Nova Contratação' }}</h2>
        </div>
        <div class="card-body">
          <form (ngSubmit)="salvar()">
            <div class="row g-3">
              <div class="col-md-6">
                <label class="form-label">Nome Completo</label>
                <input type="text" class="form-control" [(ngModel)]="func.nome" name="nome" required>
              </div>
              <div class="col-md-6">
                <label class="form-label">Cargo</label>
                <select class="form-select" [(ngModel)]="func.cargo" name="cargo" required>
                  <option value="Atendente">Atendente</option>
                  <option value="Veterinário">Veterinário</option>
                  <option value="Tosador">Tosador</option>
                  <option value="Gerente">Gerente (Admin)</option>
                </select>
                <div class="form-text">Selecionar 'Gerente' concede acesso administrativo.</div>
              </div>
              
              <div class="col-md-6">
                <label class="form-label">E-mail (Login)</label>
                <input type="email" class="form-control" [(ngModel)]="func.email" name="email" required [disabled]="isEdit">
              </div>
              
              <div class="col-md-6" *ngIf="!isEdit">
                <label class="form-label">Senha Inicial</label>
                <input type="password" class="form-control" [(ngModel)]="func.senha" name="senha" required minlength="6">
              </div>

              <div class="col-md-6">
                <label class="form-label">Telefone</label>
                <input type="text" class="form-control" [(ngModel)]="func.telefone" name="telefone">
              </div>
            </div>

            <div class="d-flex justify-content-end mt-4 gap-2">
              <a routerLink="/funcionarios" class="btn btn-secondary">Cancelar</a>
              <button type="submit" class="btn btn-primary">Salvar</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `
})
export class FuncionarioFormComponent implements OnInit {
  func: Partial<Funcionario> = { nome: '', cargo: 'Atendente', email: '', senha: '', dataContratacao: new Date() };
  isEdit = false;

  private service = inject(FuncionarioService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.service.buscarPorId(Number(id)).subscribe(data => this.func = data);
    }
  }

  salvar() {
    if (this.isEdit && this.func.funcionarioId) {
      this.service.atualizar(this.func.funcionarioId, this.func as Funcionario).subscribe(() => this.router.navigate(['/funcionarios']));
    } else {
      this.service.criar(this.func as Funcionario).subscribe(() => this.router.navigate(['/funcionarios']));
    }
  }
}
