import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { Agenda } from '../../model/agenda.model';
import { AgendaService } from '../../service/agenda/agenda';
import { Pet } from '../../model/pet.model';
import { PetService } from '../../service/pets/pet.service';
import { ServicoPet } from '../../model/servico-pet.model';
import { ServicoPetService } from '../../service/servico-pet/servico-pet';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-agenda-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './agenda-form.html',
  styleUrls: ['./agenda-form.scss']
})
export class AgendaFormComponent implements OnInit {
  agendamento: Partial<Agenda> = { dataHora: new Date(), status: 'Pendente' };
  pets: Pet[] = [];
  servicos: ServicoPet[] = [];
  isEdit = false;
  titulo = 'Novo Agendamento';
  erroMsg: string = '';

  constructor(
    private agendaService: AgendaService,
    private petService: PetService,
    private servicoPetService: ServicoPetService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.petService.listar().subscribe((data: Pet[]) => this.pets = data);
    this.servicoPetService.listar().subscribe((data: ServicoPet[]) => this.servicos = data);

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Agendamento';
      this.agendaService.buscarPorId(Number(id)).subscribe((data: Agenda) => {
        this.agendamento = data;
      });
    }
  }

  salvar(): void {
    this.erroMsg = '';

    // Usamos Observable<any> para aceitar tanto Agenda quanto void
    let operation: Observable<any>;

    if (this.isEdit && this.agendamento.id) {
      operation = this.agendaService.atualizar(this.agendamento.id, this.agendamento as Agenda);
    } else {
      operation = this.agendaService.criar(this.agendamento as Agenda);
    }

    operation.subscribe({
      next: () => {
        this.router.navigate(['/agenda']);
      },
      error: (err: HttpErrorResponse) => {
        console.error('Erro ao agendar:', err);
        this.erroMsg = typeof err.error === 'string' ? err.error : 'Erro ao salvar agendamento. Verifique os dados.';
      }
    });
  }
}
