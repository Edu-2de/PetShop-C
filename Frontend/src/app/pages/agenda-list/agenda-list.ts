import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Agenda } from '../../model/agenda.model';
import { AgendaService } from '../../service/agenda/agenda';
import { Pet } from '../../model/pet.model';
import { PetService } from '../../service/pets/pet.service';
import { ServicoPet } from '../../model/servico-pet.model';
import { ServicoPetService } from '../../service/servico-pet/servico-pet';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../service/auth/auth.service';

interface AgendamentoCompleto extends Agenda {
  pet?: Pet;
  servico?: ServicoPet;
}

@Component({
  selector: 'app-agenda-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, DatePipe],
  templateUrl: './agenda-list.html',
  styleUrls: ['./agenda-list.scss']
})
export class AgendaListComponent implements OnInit {
  // Tornamos público para o HTML acessar
  agendamentos = signal<AgendamentoCompleto[]>([]);
  termoBusca = signal<string>('');
  carregando = signal<boolean>(true);
  erroMsg = signal<string>('');

  public authService = inject(AuthService);
  private router = inject(Router);
  private agendaService = inject(AgendaService);
  private petService = inject(PetService);
  private servicoPetService = inject(ServicoPetService);

  agendamentosFiltrados = computed(() => {
    const lista = this.agendamentos();
    const termo = this.termoBusca().toLowerCase();
    if (!termo) return lista;

    return lista.filter(ag =>
      (ag.pet && ag.pet.nome.toLowerCase().includes(termo)) ||
      (ag.servico && ag.servico.nome.toLowerCase().includes(termo)) ||
      (ag.status.toLowerCase().includes(termo))
    );
  });

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    const user = this.authService.getCurrentUser()();
    this.carregando.set(true);
    this.erroMsg.set('');

    if (!user || !user.usuarioId) {
      this.erroMsg.set('Usuário não está logado ou não possui ID.');
      this.carregando.set(false);
      return;
    }

    // Se for admin ou funcionário, busca todos. Senão, busca por usuário.
    const agendamentos$ = (this.authService.isAdmin() || this.authService.isFuncionario())
      ? this.agendaService.listar()
      : this.agendaService.buscarPorUsuario(user.usuarioId);

    forkJoin({
      agendamentos: agendamentos$,
      // Carregar todos os pets e serviços para mapeamento, independentemente do usuário.
      // O filtro principal já foi feito no backend.
      pets: this.petService.listar(),
      servicos: this.servicoPetService.listar()
    }).subscribe({
      next: ({ agendamentos, pets, servicos }) => {
        const petsMap = new Map(pets.map(p => [p.animalId, p]));
        const servicosMap = new Map(servicos.map(s => [s.servicoId, s]));

        const agendamentosCompletos = agendamentos.map(agenda => ({
          ...agenda,
          pet: petsMap.get(agenda.animalId),
          servico: servicosMap.get(agenda.servicoId)
        }));

        this.agendamentos.set(agendamentosCompletos);
        this.carregando.set(false);
      },
      error: (error) => {
        console.error('❌ Erro ao carregar agendamentos:', error);
        this.erroMsg.set('Erro ao carregar agendamentos. Tente novamente.');
        this.carregando.set(false);
      }
    });
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  cancelar(agenda: AgendamentoCompleto) {
    if (!confirm('Deseja realmente cancelar este agendamento?')) return;

    const idAgendamento = agenda.agendamentoId || agenda.id;
    if (!idAgendamento) {
      alert('Erro: ID do agendamento não encontrado.');
      return;
    }

    // Atualiza status para Cancelado
    const agendaAtualizada = { ...agenda, status: 'Cancelado' };

    this.agendaService.atualizar(idAgendamento, agenda Atualizada).subscribe({
      next: () => {
        alert('Agendamento cancelado.');
        this.carregarDados();
      },
      error: (err) => {
        console.error('Erro ao cancelar agendamento:', err);
        alert('Erro ao cancelar agendamento. Tente novamente.');
      }
    });
  }

  // Método para recarregar dados
  recarregar(): void {
    this.carregarDados();
  }
}
