import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { TutorService } from '../../service/tutores/tutor.service';
import { FuncionarioService } from '../../service/funcionarios/funcionario.service';

interface PerfilUsuario {
  usuarioId: number;
  nome: string;
  email: string;
  telefone?: string;
  endereco?: string;
  dataCadastro?: Date;
  cargo: string;
  tipoUsuario: string;
}

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="container mt-4 mb-5">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="card shadow-sm border-0">
            
            <div class="card-header bg-white py-3">
              <h2 class="h4 mb-0 fw-bold text-primary">
                <i class="bi bi-person-circle me-2"></i>Meu Perfil
              </h2>
              <p class="text-muted small mb-0">
                Gerencie suas informações pessoais
              </p>
            </div>

            <div class="card-body p-4">
              
              <!-- Mensagens -->
              <div class="alert alert-success alert-dismissible fade show" *ngIf="sucessoMsg" role="alert">
                <i class="bi bi-check-circle me-2"></i>{{ sucessoMsg }}
                <button type="button" class="btn-close" (click)="sucessoMsg = ''"></button>
              </div>
              
              <div class="alert alert-danger" *ngIf="erroMsg">
                <i class="bi bi-exclamation-triangle me-2"></i>{{ erroMsg }}
              </div>

              <!-- Loading -->
              <div *ngIf="carregando" class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Carregando...</span>
                </div>
                <p class="mt-2 text-muted">Carregando informações do perfil...</p>
              </div>

              <!-- Formulário de Perfil -->
              <form #perfilForm="ngForm" (ngSubmit)="salvarPerfil()" *ngIf="!carregando">
                
                <!-- Seção Informações Pessoais -->
                <div class="mb-4">
                  <h5 class="fw-bold text-secondary mb-3">
                    <i class="bi bi-person me-2"></i>Informações Pessoais
                  </h5>
                  
                  <div class="row g-3">
                    <div class="col-md-6">
                      <label for="nome" class="form-label">Nome Completo *</label>
                      <input type="text" 
                             class="form-control"
                             id="nome"
                             name="nome"
                             [(ngModel)]="perfil.nome"
                             required
                             maxlength="120"
                             #nomeInput="ngModel">
                      <div *ngIf="nomeInput.invalid && (nomeInput.dirty || nomeInput.touched)" class="text-danger mt-1 small">
                        <div *ngIf="nomeInput.errors?.['required']">Nome é obrigatório.</div>
                        <div *ngIf="nomeInput.errors?.['maxlength']">Nome deve ter no máximo 120 caracteres.</div>
                      </div>
                    </div>

                    <div class="col-md-6">
                      <label for="telefone" class="form-label">Telefone</label>
                      <input type="tel" 
                             class="form-control"
                             id="telefone"
                             name="telefone"
                             [(ngModel)]="perfil.telefone"
                             placeholder="(11) 99999-9999"
                             maxlength="20">
                    </div>
                  </div>
                </div>

                <!-- Seção Endereço -->
                <div class="mb-4">
                  <h5 class="fw-bold text-secondary mb-3">
                    <i class="bi bi-geo-alt me-2"></i>Endereço
                  </h5>
                  
                  <div class="row g-3">
                    <div class="col-12">
                      <label for="endereco" class="form-label">Endereço Completo</label>
                      <textarea class="form-control" 
                                id="endereco"
                                name="endereco"
                                [(ngModel)]="perfil.endereco"
                                rows="2"
                                maxlength="250"
                                placeholder="Rua, número, bairro, cidade..."></textarea>
                    </div>
                  </div>
                </div>

                <!-- Seção Conta -->
                <div class="mb-4">
                  <h5 class="fw-bold text-secondary mb-3">
                    <i class="bi bi-key me-2"></i>Informações da Conta
                  </h5>
                  
                  <div class="row g-3">
                    <div class="col-md-6">
                      <label for="email" class="form-label">E-mail</label>
                      <input type="email" 
                             class="form-control"
                             id="email"
                             name="email"
                             [value]="perfil.email"
                             readonly
                             style="background-color: #f8f9fa;">
                      <small class="form-text text-muted">
                        <i class="bi bi-info-circle me-1"></i>O e-mail não pode ser alterado por segurança
                      </small>
                    </div>

                    <div class="col-md-6">
                      <label for="tipoUsuario" class="form-label">Tipo de Usuário</label>
                      <input type="text" 
                             class="form-control"
                             id="tipoUsuario"
                             [value]="perfil.tipoUsuario + (perfil.cargo ? ' (' + perfil.cargo + ')' : '')"
                             readonly
                             style="background-color: #f8f9fa;">
                    </div>
                  </div>
                </div>

                <!-- Botões -->
                <div class="d-flex justify-content-between">
                  <a routerLink="/dashboard" class="btn btn-light border">
                    <i class="bi bi-arrow-left me-2"></i>Voltar
                  </a>
                  
                  <div class="d-flex gap-2">
                    <button type="button" 
                            class="btn btn-outline-primary"
                            (click)="resetarFormulario()">
                      <i class="bi bi-arrow-clockwise me-2"></i>Desfazer Alterações
                    </button>
                    
                    <button type="submit" 
                            class="btn btn-primary px-4"
                            [disabled]="perfilForm.invalid || salvando">
                      <span *ngIf="salvando" class="spinner-border spinner-border-sm me-2"></span>
                      <i *ngIf="!salvando" class="bi bi-check-lg me-2"></i>
                      {{ salvando ? 'Salvando...' : 'Salvar Alterações' }}
                    </button>
                  </div>
                </div>

              </form>
            </div>
          </div>

          <!-- Card de Ações Rápidas - PERSONALIZADO POR TIPO DE USUÁRIO -->
          <div class="card mt-4 shadow-sm border-0">
            <div class="card-header bg-white py-3">
              <h5 class="mb-0 fw-bold">
                <i class="bi bi-lightning me-2 text-warning"></i>Ações Rápidas
              </h5>
            </div>
            <div class="card-body">
              <div class="row g-3">
                
                <!-- Ações para ADMIN -->
                <ng-container *ngIf="authService.isAdmin()">
                  <div class="col-md-3">
                    <a routerLink="/admin" class="btn btn-outline-primary w-100">
                      <i class="bi bi-gear me-2"></i>Painel Admin
                    </a>
                  </div>
                  <div class="col-md-3">
                    <a routerLink="/funcionarios" class="btn btn-outline-success w-100">
                      <i class="bi bi-people me-2"></i>Funcionários
                    </a>
                  </div>
                  <div class="col-md-3">
                    <a routerLink="/tutores" class="btn btn-outline-info w-100">
                      <i class="bi bi-person-heart me-2"></i>Tutores
                    </a>
                  </div>
                  <div class="col-md-3">
                    <a routerLink="/produtos" class="btn btn-outline-warning w-100">
                      <i class="bi bi-box me-2"></i>Produtos
                    </a>
                  </div>
                </ng-container>

                <!-- Ações para FUNCIONÁRIOS -->
                <ng-container *ngIf="authService.isFuncionario()">
                  <div class="col-md-4">
                    <a routerLink="/agenda" class="btn btn-outline-primary w-100">
                      <i class="bi bi-calendar me-2"></i>Agendamentos
                    </a>
                  </div>
                  <div class="col-md-4">
                    <a routerLink="/servicos" class="btn btn-outline-success w-100">
                      <i class="bi bi-tools me-2"></i>Serviços
                    </a>
                  </div>
                  <div class="col-md-4">
                    <a routerLink="/produtos" class="btn btn-outline-info w-100">
                      <i class="bi bi-box me-2"></i>Produtos
                    </a>
                  </div>
                </ng-container>

                <!-- Ações para TUTORES/CLIENTES -->
                <ng-container *ngIf="authService.isTutor()">
                  <div class="col-md-4">
                    <a routerLink="/pets" class="btn btn-outline-primary w-100">
                      <i class="bi bi-heart me-2"></i>Meus Pets
                    </a>
                  </div>
                  <div class="col-md-4">
                    <a routerLink="/agenda" class="btn btn-outline-success w-100">
                      <i class="bi bi-calendar me-2"></i>Agendamentos
                    </a>
                  </div>
                  <div class="col-md-4">
                    <a routerLink="/vendas/minhas" class="btn btn-outline-info w-100">
                      <i class="bi bi-bag me-2"></i>Minhas Compras
                    </a>
                  </div>
                </ng-container>

              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  `,
  styles: [`
    .card {
      transition: all 0.3s ease;
    }
    
    .card:hover {
      box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important;
    }
    
    .form-control:focus {
      border-color: #0d6efd;
      box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
    }
    
    .btn-primary {
      background: linear-gradient(45deg, #0d6efd, #0056b3);
      border: none;
    }
    
    .btn-primary:hover {
      background: linear-gradient(45deg, #0056b3, #004085);
    }
  `]
})
export class PerfilComponent implements OnInit {
  public authService = inject(AuthService);
  private tutorService = inject(TutorService);
  private funcionarioService = inject(FuncionarioService);

  perfil: PerfilUsuario = {
    usuarioId: 0,
    nome: '',
    email: '',
    telefone: '',
    endereco: '',
    dataCadastro: new Date(),
    cargo: '',
    tipoUsuario: ''
  };
  
  perfilOriginal: PerfilUsuario = { ...this.perfil };
  erroMsg: string = '';
  sucessoMsg: string = '';
  salvando: boolean = false;
  carregando: boolean = true;

  ngOnInit(): void {
    this.carregarPerfilUsuario();
  }

  carregarPerfilUsuario(): void {
    const user = this.authService.getCurrentUser()();
    
    console.log('🔍 DEBUG: Dados do usuário logado:', user);
    
    if (!user) {
      this.erroMsg = 'Usuário não está logado.';
      this.carregando = false;
      return;
    }

    // NOVO: Carregar dados baseado no tipo de usuário
    if (user.tutorId && this.authService.isTutor()) {
      // Se é TUTOR, buscar dados do tutor (que inclui telefone e endereço)
      this.carregarDadosTutor(user.tutorId, user);
    } else if (user.funcionarioId && this.authService.isFuncionario()) {
      // Se é FUNCIONÁRIO, buscar dados do funcionário
      this.carregarDadosFuncionario(user.funcionarioId, user);
    } else {
      // Se é ADMIN ou usuário simples, usar apenas dados do Usuario
      this.carregarDadosUsuarioSimples(user);
    }
  }

  private carregarDadosTutor(tutorId: number, user: any): void {
    this.tutorService.buscarPorId(tutorId).subscribe({
      next: (tutor) => {
        console.log('🔍 DEBUG: Dados do tutor recebidos:', tutor);
        
        this.perfil = {
          usuarioId: user.usuarioId,
          nome: tutor.nome || user.nome,
          email: tutor.email || user.email,
          telefone: tutor.telefone || '',
          endereco: tutor.endereco || '',
          dataCadastro: tutor.dataCadastro,
          cargo: user.cargo,
          tipoUsuario: 'Tutor'
        };
        
        this.perfilOriginal = { ...this.perfil };
        this.carregando = false;
      },
      error: (err) => {
        console.error('❌ Erro ao carregar perfil do tutor:', err);
        // Fallback: usar dados do usuário
        this.carregarDadosUsuarioSimples(user);
      }
    });
  }

  private carregarDadosFuncionario(funcionarioId: number, user: any): void {
    this.funcionarioService.buscarPorId(funcionarioId).subscribe({
      next: (funcionario) => {
        console.log('🔍 DEBUG: Dados do funcionário recebidos:', funcionario);
        
        this.perfil = {
          usuarioId: user.usuarioId,
          nome: funcionario.nome || user.nome,
          email: funcionario.email || user.email,
          telefone: funcionario.telefone || '',
          endereco: '', // Funcionários não têm endereço específico
          dataCadastro: funcionario.dataContratacao,
          cargo: funcionario.cargo,
          tipoUsuario: 'Funcionário'
        };
        
        this.perfilOriginal = { ...this.perfil };
        this.carregando = false;
      },
      error: (err) => {
        console.error('❌ Erro ao carregar perfil do funcionário:', err);
        // Fallback: usar dados do usuário
        this.carregarDadosUsuarioSimples(user);
      }
    });
  }

  private carregarDadosUsuarioSimples(user: any): void {
    console.log('🔍 DEBUG: Carregando dados simples do usuário:', user);
    
    this.perfil = {
      usuarioId: user.usuarioId,
      nome: user.nome,
      email: user.email,
      telefone: '', // Admin não tem telefone específico
      endereco: '', // Admin não tem endereço específico
      dataCadastro: new Date(), // Usar data atual como fallback
      cargo: user.cargo,
      tipoUsuario: user.cargo === 'Admin' ? 'Administrador' : user.cargo
    };
    
    this.perfilOriginal = { ...this.perfil };
    this.carregando = false;
  }

  salvarPerfil(): void {
    if (this.salvando) return;
    
    this.erroMsg = '';
    this.sucessoMsg = '';
    this.salvando = true;

    const user = this.authService.getCurrentUser()();
    
    console.log('🔄 DEBUG: Salvando perfil para usuário:', user);

    // Salvar baseado no tipo de usuário
    if (user?.tutorId && this.authService.isTutor()) {
      // Salvar como tutor - dados específicos
      const dadosParaEnvio = {
        nome: this.perfil.nome,
        telefone: this.perfil.telefone,
        endereco: this.perfil.endereco
      };

      console.log('🔄 DEBUG: Salvando dados de tutor:', dadosParaEnvio);

      this.tutorService.atualizar(user.tutorId, dadosParaEnvio).subscribe({
        next: () => this.salvoComSucesso(),
        error: (err) => this.erroAoSalvar(err)
      });
    } else if (user?.funcionarioId && this.authService.isFuncionario()) {
      // Salvar como funcionário - primeiro buscar dados atuais para não perder informações
      this.funcionarioService.buscarPorId(user.funcionarioId).subscribe({
        next: (funcionarioAtual) => {
          // Montar objeto Funcionario completo mantendo dados que não devem ser alterados
          const funcionarioAtualizado = {
            funcionarioId: funcionarioAtual.funcionarioId,
            nome: this.perfil.nome, // Permitir alterar nome
            cargo: funcionarioAtual.cargo, // Manter cargo original
            telefone: this.perfil.telefone || funcionarioAtual.telefone, // Permitir alterar telefone
            email: funcionarioAtual.email, // Manter email original (readonly)
            dataContratacao: funcionarioAtual.dataContratacao // Manter data original
          };

          console.log('🔄 DEBUG: Salvando dados de funcionário:', funcionarioAtualizado);

          // Verificação adicional para TypeScript
          if (user.funcionarioId) {
            this.funcionarioService.atualizar(user.funcionarioId, funcionarioAtualizado).subscribe({
              next: () => this.salvoComSucesso(),
              error: (err) => this.erroAoSalvar(err)
            });
          }
        },
        error: (err) => {
          console.error('❌ Erro ao buscar dados do funcionário:', err);
          this.erroAoSalvar(err);
        }
      });
    } else {
      // Admin não pode alterar dados pelo perfil (apenas pelo painel admin)
      this.erroMsg = 'Administradores devem alterar dados pelo Painel Admin.';
      this.salvando = false;
    }
  }

  private salvoComSucesso(): void {
    this.sucessoMsg = '✅ Perfil atualizado com sucesso!';
    this.perfilOriginal = { ...this.perfil };
    this.salvando = false;
    
    setTimeout(() => {
      this.sucessoMsg = '';
    }, 3000);
  }

  private erroAoSalvar(err: any): void {
    console.error('❌ Erro ao salvar perfil:', err);
    this.erroMsg = err.error?.message || 'Erro ao atualizar perfil. Tente novamente.';
    this.salvando = false;
  }

  resetarFormulario(): void {
    this.perfil = { ...this.perfilOriginal };
    this.erroMsg = '';
    this.sucessoMsg = '';
  }
}
