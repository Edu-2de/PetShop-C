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
  private agendamentos = signal<AgendamentoCompleto[]>([]);
  termoBusca = signal<string>('');

  public authService = inject(AuthService);
  private router = inject(Router);

  agendamentosFiltrados = computed(() => {
    const agendamentos = this.agendamentos();
    const termo = this.termoBusca().toLowerCase();

    if (!termo) {
      return agendamentos;
    }

    return agendamentos.filter(ag =>
      (ag.pet && ag.pet.nome.toLowerCase().includes(termo)) ||
      (ag.servico && ag.servico.nome.toLowerCase().includes(termo))
    );
  });

  constructor(
    private agendaService: AgendaService,
    private petService: PetService,
    private servicoPetService: ServicoPetService
  ) { }

  ngOnInit(): void {
    // Lógica de Redirecionamento:
    // Se NÃO for Admin, vai direto para "Agendar Novo" em vez de ver a lista
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/agenda/novo']);
      return;
    }

    this.carregarAgendamentos();
  }

  carregarAgendamentos(): void {
    // ... (código existente)
    forkJoin({
      agendamentos: this.agendaService.listar(),
      pets: this.petService.listar(),
      servicos: this.servicoPetService.listar()
    }).subscribe(({ agendamentos, pets, servicos }) => {
      const petsMap = new Map(pets.map((p: Pet) => [p.id, p]));
      const servicosMap = new Map(servicos.map((s: ServicoPet) => [s.id, s]));

      const agendamentosCompletos: AgendamentoCompleto[] = agendamentos.map((agenda: Agenda) => ({
        ...agenda,
        pet: petsMap.get(agenda.animalId),
        servico: servicosMap.get(agenda.servicoId)
      }));

      this.agendamentos.set(agendamentosCompletos);
    });
  }

  // ... (restante dos métodos: buscar, excluir)
  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  excluir(id: number | undefined): void {
    if (id === undefined) return;

    if (confirm('Deseja realmente excluir este agendamento?')) {
      this.agendaService.deletar(id).subscribe(() => {
        this.agendamentos.update(agendamentosAtuais => agendamentosAtuais.filter(a => a.id !== id));
      });
    }
  }
}
