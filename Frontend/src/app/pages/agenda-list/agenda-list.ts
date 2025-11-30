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
    // Removemos qualquer redirecionamento forçado aqui
    this.carregarDados();
  }

  carregarDados(): void {
    const user = this.authService.getCurrentUser()();
    let obsAgendamentos;

    // LÓGICA PRINCIPAL:
    if (this.authService.isAdmin()) {
      // Admin vê tudo
      obsAgendamentos = this.agendaService.listar();
    } else {
      // Tutor vê apenas os seus
      obsAgendamentos = this.agendaService.buscarPorTutor(user?.id || 0);
    }

    forkJoin({
      agendamentos: obsAgendamentos,
      pets: this.petService.listar(),
      servicos: this.servicoPetService.listar()
    }).subscribe(({ agendamentos, pets, servicos }) => {
      const petsMap = new Map(pets.map(p => [p.id, p]));
      const servicosMap = new Map(servicos.map(s => [s.id, s]));

      const completos = agendamentos.map(agenda => ({
        ...agenda,
        pet: petsMap.get(agenda.animalId),
        servico: servicosMap.get(agenda.servicoId)
      }));

      this.agendamentos.set(completos);
    });
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  cancelar(agenda: AgendamentoCompleto) {
    if (!confirm('Deseja realmente cancelar este agendamento?')) return;

    // Atualiza status para Cancelado
    const agendaAtualizada = { ...agenda, status: 'Cancelado' };

    // Usa o update para mudar o status (não deleta o registro histórico)
    this.agendaService.atualizar(agenda.id!, agendaAtualizada).subscribe({
      next: () => {
        alert('Agendamento cancelado.');
        this.carregarDados();
      },
      error: () => alert('Erro ao cancelar.')
    });
  }
}
