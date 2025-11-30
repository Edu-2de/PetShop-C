// Frontend/src/app/pages/agenda-form/agenda-form.ts
import { Component, OnInit, inject } from '@angular/core';
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
import { AuthService } from '../../service/auth/auth.service';
import { switchMap, of, Observable } from 'rxjs'; // Importante para corrigir o erro de fluxo

@Component({
  selector: 'app-agenda-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './agenda-form.html',
  styleUrls: ['./agenda-form.scss']
})
export class AgendaFormComponent implements OnInit {
  private agendaService = inject(AgendaService);
  private petService = inject(PetService);
  private servicoPetService = inject(ServicoPetService);
  public authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  agendamento: Partial<Agenda> = { dataHora: new Date(), status: 'Agendado' };
  novoPet: Partial<Pet> = { nome: '', especie: 'Cão', raca: '', sexo: 'Macho' };

  pets: Pet[] = [];
  servicos: ServicoPet[] = [];
  isEdit = false;
  titulo = 'Novo Agendamento';
  erroMsg: string = '';

  isCliente = false;
  precisaCadastrarPet = false;
  tutorIdLogado: number = 0;

  ngOnInit(): void {
    this.carregarServicos();

    // CORREÇÃO: Acessando o valor do Signal corretamente
    const userSignal = this.authService.getCurrentUser();
    const user = userSignal();

    this.isCliente = !this.authService.isAdmin();

    if (this.isCliente && user) {
      this.tutorIdLogado = user.id || 0; // Agora 'id' existe na interface User
      this.carregarPetsDoTutor(this.tutorIdLogado);
      this.agendamento.status = 'Agendado';
    } else {
      this.carregarTodosPets();
    }

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Agendamento';
      this.agendaService.buscarPorId(Number(id)).subscribe(data => {
        this.agendamento = data;
      });
    }

    this.route.queryParams.subscribe(params => {
      if (params['servicoId']) {
        this.agendamento.servicoId = Number(params['servicoId']);
      }
    });
  }

  carregarServicos() {
    this.servicoPetService.listar().subscribe(data =>
      this.servicos = data.filter(s => s.ativo)
    );
  }

  carregarTodosPets() {
    this.petService.listar().subscribe(data => this.pets = data);
  }

  carregarPetsDoTutor(tutorId: number) {
    this.petService.buscarPorTutor(tutorId).subscribe({
      next: (data) => {
        this.pets = data;
        // LÓGICA DO REQUISITO: Se não tem pets, abre a tela de cadastro
        if (this.pets.length === 0) {
          this.precisaCadastrarPet = true;
        }
      },
      error: () => console.error('Erro ao buscar pets do tutor')
    });
  }

  toggleNovoPet() {
    this.precisaCadastrarPet = !this.precisaCadastrarPet;
    if (this.precisaCadastrarPet) {
      this.agendamento.petid = undefined;
    }
  }

  salvar(): void {
    this.erroMsg = '';

    if (this.isCliente && this.precisaCadastrarPet) {
      this.salvarPetEAgendar();
    } else {
      this.salvarAgendamento();
    }
  }

  salvarPetEAgendar() {
    if (!this.novoPet.nome || !this.novoPet.especie) {
      this.erroMsg = 'Preencha os dados do seu Pet para continuar.';
      return;
    }

    this.novoPet.tutorId = this.tutorIdLogado;

    this.petService.criar(this.novoPet).pipe(
      switchMap((petCriado) => {
        // Usa o ID do pet recém-criado
        this.agendamento.petid = petCriado.animalId || petCriado.id;

        if (this.isEdit && this.agendamento.id) {
          return this.agendaService.atualizar(this.agendamento.id, this.agendamento as Agenda);
        }
        return this.agendaService.criar(this.agendamento as Agenda);
      })
    ).subscribe({
      next: () => {
        alert('Pet cadastrado e consulta agendada com sucesso!');
        this.router.navigate(['/agenda']);
      },
      error: (err: any) => this.tratarErro(err)
    });
  }

  salvarAgendamento() {
    // CORREÇÃO: Tipagem explícita para evitar erro de assinatura incompatível
    let operation: Observable<any>;

    if (this.isEdit && this.agendamento.id) {
      operation = this.agendaService.atualizar(this.agendamento.id, this.agendamento as Agenda);
    } else {
      operation = this.agendaService.criar(this.agendamento as Agenda);
    }

    operation.subscribe({
      next: () => {
        alert(this.isEdit ? 'Atualizado com sucesso!' : 'Agendamento realizado!');
        this.router.navigate(['/agenda']);
      },
      error: (err: any) => this.tratarErro(err)
    });
  }

  tratarErro(err: any) {
    console.error('Erro:', err);
    if (err.error && typeof err.error === 'string') {
      this.erroMsg = err.error;
    } else {
      this.erroMsg = 'Erro ao salvar. Verifique os dados.';
    }
  }
}
