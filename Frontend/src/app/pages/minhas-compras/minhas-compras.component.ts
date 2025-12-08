import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { VendaService } from '../../service/vendas/venda.service';

@Component({
  selector: 'app-minhas-compras',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="container mt-4 mb-5">
      <div class="row">
        <div class="col-12">
          <div class="card shadow-sm border-0">
            
            <div class="card-header bg-white py-3">
              <h2 class="h4 mb-0 fw-bold text-primary">
                <i class="bi bi-bag-check me-2"></i>Minhas Compras
              </h2>
              <p class="text-muted small mb-0">
                Histórico de todas as suas compras e serviços
              </p>
            </div>

            <div class="card-body p-4">
              
              <!-- Mensagens -->
              <div class="alert alert-danger" *ngIf="erroMsg">
                <i class="bi bi-exclamation-triangle me-2"></i>{{ erroMsg }}
                <button class="btn btn-outline-danger btn-sm ms-3" (click)="carregarCompras()">
                  <i class="bi bi-arrow-clockwise me-1"></i>Tentar Novamente
                </button>
              </div>

              <!-- Info para usuários sem compras -->
              <div class="alert alert-info" *ngIf="!carregando && compras.length === 0 && !erroMsg">
                <i class="bi bi-info-circle me-2"></i>
                <strong>Informação:</strong> 
                <span *ngIf="authService.isAdmin()">Como administrador, você pode ver vendas gerais no painel admin.</span>
                <span *ngIf="authService.isFuncionario()">Como funcionário, você pode acessar vendas pelo painel de funcionários.</span>
                <span *ngIf="!authService.isAdmin() && !authService.isFuncionario()">Suas compras e serviços adquiridos aparecerão aqui.</span>
              </div>

              <!-- Loading -->
              <div *ngIf="carregando" class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Carregando...</span>
                </div>
                <p class="mt-2 text-muted">Carregando suas compras...</p>
              </div>

              <!-- Lista de Compras -->
              <div *ngIf="!carregando">
                
                <!-- Se tem compras -->
                <div *ngIf="compras.length > 0">
                  <div class="row g-3">
                    <div class="col-12" *ngFor="let compra of compras">
                      <div class="card border">
                        <div class="card-body">
                          <div class="row align-items-center">
                            
                            <!-- Informações da Compra -->
                            <div class="col-md-6">
                              <h6 class="mb-1 fw-bold">
                                Compra #{{ compra.vendaId }}
                                <span class="badge bg-success ms-2">{{ compra.status || 'Concluído' }}</span>
                              </h6>
                              <small class="text-muted">
                                <i class="bi bi-calendar me-1"></i>
                                {{ formatarData(compra.dataVenda) }}
                              </small>
                            </div>

                            <!-- Valor Total -->
                            <div class="col-md-3 text-md-center">
                              <div class="fw-bold text-primary fs-5">
                                {{ compra.valorTotal | currency:'BRL' }}
                              </div>
                              <small class="text-muted">{{ compra.formaPagamento }}</small>
                            </div>

                            <!-- Ações -->
                            <div class="col-md-3 text-md-end">
                              <button class="btn btn-outline-primary btn-sm me-2"
                                      (click)="toggleDetalhes(compra.vendaId)">
                                <i class="bi" [class.bi-chevron-down]="!compra.mostrarDetalhes" 
                                                [class.bi-chevron-up]="compra.mostrarDetalhes"></i>
                                {{ compra.mostrarDetalhes ? 'Ocultar' : 'Ver Detalhes' }}
                              </button>
                            </div>
                          </div>

                          <!-- Detalhes expandidos -->
                          <div *ngIf="compra.mostrarDetalhes" class="mt-3 pt-3 border-top">
                            <h6 class="mb-2">Itens da Compra:</h6>
                            <div class="table-responsive">
                              <table class="table table-sm">
                                <thead>
                                  <tr>
                                    <th>Item</th>
                                    <th>Tipo</th>
                                    <th>Qtd</th>
                                    <th>Valor Unit.</th>
                                    <th>Total</th>
                                  </tr>
                                </thead>
                                <tbody>
                                  <tr *ngFor="let item of compra.itens">
                                    <td>
                                      <strong>{{ item.produtoNome || item.servicoNome }}</strong>
                                    </td>
                                    <td>
                                      <span class="badge" 
                                            [class.bg-info]="item.produtoNome"
                                            [class.bg-success]="item.servicoNome">
                                        {{ item.produtoNome ? 'Produto' : 'Serviço' }}
                                      </span>
                                    </td>
                                    <td>{{ item.quantidade }}</td>
                                    <td>{{ item.precoUnitario | currency:'BRL' }}</td>
                                    <td class="fw-bold">{{ (item.quantidade * item.precoUnitario) | currency:'BRL' }}</td>
                                  </tr>
                                </tbody>
                              </table>
                            </div>
                            
                            <!-- Observações se houver -->
                            <div *ngIf="compra.observacoes" class="mt-2">
                              <small class="text-muted">
                                <strong>Observações:</strong> {{ compra.observacoes }}
                              </small>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Se não tem compras -->
                <div *ngIf="compras.length === 0 && !erroMsg" class="text-center py-5">
                  <i class="bi bi-bag text-muted" style="font-size: 4rem;"></i>
                  <h4 class="mt-3 text-muted">Nenhuma compra encontrada</h4>
                  <p class="text-muted">
                    Você ainda não fez nenhuma compra conosco.
                  </p>
                  <a routerLink="/produtos" class="btn btn-primary">
                    <i class="bi bi-bag-plus me-2"></i>Ver Produtos
                  </a>
                </div>

              </div>

            </div>
          </div>

          <!-- Card de Ações Rápidas -->
          <div class="card mt-4 shadow-sm border-0">
            <div class="card-header bg-white py-3">
              <h5 class="mb-0 fw-bold">
                <i class="bi bi-lightning me-2 text-warning"></i>Ações Rápidas
              </h5>
            </div>
            <div class="card-body">
              <div class="row g-3">
                <div class="col-md-3">
                  <a routerLink="/produtos" class="btn btn-outline-primary w-100">
                    <i class="bi bi-bag-plus me-2"></i>Nova Compra
                  </a>
                </div>
                <div class="col-md-3" *ngIf="authService.isTutor()">
                  <a routerLink="/agenda/novo" class="btn btn-outline-success w-100">
                    <i class="bi bi-calendar-plus me-2"></i>Agendar Serviço
                  </a>
                </div>
                <div class="col-md-3" *ngIf="authService.isTutor()">
                  <a routerLink="/pets" class="btn btn-outline-info w-100">
                    <i class="bi bi-heart me-2"></i>Meus Pets
                  </a>
                </div>
                <div class="col-md-3">
                  <a routerLink="/perfil" class="btn btn-outline-secondary w-100">
                    <i class="bi bi-person me-2"></i>Meu Perfil
                  </a>
                </div>
                <div class="col-md-3" *ngIf="authService.isAdmin()">
                  <a routerLink="/admin" class="btn btn-outline-warning w-100">
                    <i class="bi bi-gear me-2"></i>Painel Admin
                  </a>
                </div>
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
    
    .table th {
      border-top: none;
      font-weight: 600;
      color: #495057;
    }
    
    .badge {
      font-size: 0.75em;
    }
  `]
})
export class MinhasComprasComponent implements OnInit {
  public authService = inject(AuthService);
  private vendaService = inject(VendaService);

  compras: any[] = [];
  erroMsg: string = '';
  carregando: boolean = true;

  ngOnInit(): void {
    this.carregarCompras();
  }

  carregarCompras(): void {
    const user = this.authService.getCurrentUser()();
    
    console.log('🔍 DEBUG: Carregando compras para USUÁRIO:', user);

    if (!user || !user.usuarioId) {
      this.erroMsg = 'Usuário não está logado ou não possui ID.';
      this.carregando = false;
      return;
    }

    this.erroMsg = '';
    this.carregando = true;

    console.log('🛒 Buscando compras por usuarioId:', user.usuarioId);
    this.vendaService.buscarPorUsuario(user.usuarioId).subscribe({
      next: (vendas: any[]) => {
        this.compras = vendas.map((venda: any) => ({
          ...venda,
          mostrarDetalhes: false
        }));
        this.carregando = false;
        console.log('✅ Compras carregadas por usuário:', this.compras);
      },
      error: (err: any) => {
        console.error('❌ Erro ao carregar compras por usuário:', err);
        this.erroMsg = 'Erro ao carregar histórico de compras. Tente novamente mais tarde.';
        this.carregando = false;
      }
    });
  }

  toggleDetalhes(vendaId: number): void {
    const compra = this.compras.find(c => c.vendaId === vendaId);
    if (compra) {
      compra.mostrarDetalhes = !compra.mostrarDetalhes;
    }
  }

  formatarData(data: Date | string): string {
    if (!data) return '';
    const date = new Date(data);
    return date.toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
