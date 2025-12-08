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
          <form #form="ngForm" (ngSubmit)="salvar()">
            <div class="row g-3">
              <div class="col-md-6">
                <label class="form-label">Nome Completo</label>
                <input type="text" class="form-control" [(ngModel)]="func.nome" name="nome" required minlength="3" #nome="ngModel">
                <div *ngIf="nome.invalid && (nome.dirty || nome.touched)" class="text-danger mt-1 small">
                  <div *ngIf="nome.errors?.['required']">O nome é obrigatório.</div>
                  <div *ngIf="nome.errors?.['minlength']">O nome deve ter no mínimo 3 caracteres.</div>
                </div>
              </div>
              <div class="col-md-6">
                <label class="form-label">Cargo</label>
                <select class="form-select" [(ngModel)]="func.cargo" name="cargo" required #cargo="ngModel">
                  <option value="Atendente">Atendente</option>
                  <option value="Veterinário">Veterinário</option>
                  <option value="Tosador">Tosador</option>
                  <option value="Gerente">Gerente (Admin)</option>
                </select>
                <div *ngIf="cargo.invalid && (cargo.dirty || cargo.touched)" class="text-danger mt-1 small">
                  <div *ngIf="cargo.errors?.['required']">O cargo é obrigatório.</div>
                </div>
                <div class="form-text">Selecionar 'Gerente' concede acesso administrativo.</div>
              </div>
              
              <div class="col-md-6">
                <label class="form-label">E-mail (Login)</label>
                <input type="email" class="form-control" [(ngModel)]="func.email" name="email" required email [disabled]="isEdit" #email="ngModel">
                <div *ngIf="email.invalid && (email.dirty || email.touched)" class="text-danger mt-1 small">
                  <div *ngIf="email.errors?.['required']">O e-mail é obrigatório.</div>
                  <div *ngIf="email.errors?.['email']">Formato de e-mail inválido.</div>
                </div>
              </div>
              
              <div class="col-md-6" *ngIf="!isEdit">
                <label class="form-label">Senha Inicial</label>
                <input type="password" class="form-control" [(ngModel)]="func.senha" name="senha" required minlength="6" #senha="ngModel">
                <div *ngIf="senha.invalid && (senha.dirty || senha.touched)" class="text-danger mt-1 small">
                  <div *ngIf="senha.errors?.['required']">A senha é obrigatória.</div>
                  <div *ngIf="senha.errors?.['minlength']">A senha deve ter no mínimo 6 caracteres.</div>
                </div>
              </div>

              <div class="col-md-6">
                <label class="form-label">Telefone</label>
                <input type="text" class="form-control" [(ngModel)]="func.telefone" name="telefone" pattern="^\\(\\d{2}\\)\\s?\\d{4,5}-?\\d{4}$|^\\d{10,11}$" #telefone="ngModel">
                <div *ngIf="telefone.invalid && (telefone.dirty || telefone.touched)" class="text-danger mt-1 small">
                  <div *ngIf="telefone.errors?.['pattern']">Telefone inválido. Use (XX) XXXXX-XXXX ou 10/11 dígitos.</div>
                </div>
              </div>
            </div>

            <div class="d-flex justify-content-end mt-4 gap-2">
              <a routerLink="/funcionarios" class="btn btn-secondary">Cancelar</a>
              <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Salvar</button>
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
