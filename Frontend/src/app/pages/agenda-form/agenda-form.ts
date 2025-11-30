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
import { switchMap, of } from 'rxjs';

@Component({
  selector: 'app-agenda-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './agenda-form.html',
  styleUrls: ['./agenda-form.scss']
})
export class AgendaFormComponent implements OnInit {
  // Services
  private agendaService = inject(AgendaService);
  private petService = inject(PetService);
  private servicoPetService = inject(ServicoPetService);
  public authService = inject(AuthService); // Public para usar no HTML
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  // Dados do Agendamento
  agendamento: Partial<Agenda> = { dataHora: new Date(), status: 'Agendado' };
  
  // Dados do Novo Pet (Caso precise cadastrar na hora)
  novoPet: Partial<Pet> = { nome: '', especie: '', raca: '', sexo: 'Macho' };
  
  // Listas e Controles
  pets: Pet[] = [];
  servicos: ServicoPet[] = [];
  isEdit = false;
  titulo = 'Novo Agendamento';
  erroMsg: string = '';
  
  // Controle de fluxo do Cliente
  isCliente = false;
  precisaCadastrarPet = false; // Se true, mostra form de pet
  tutorIdLogado: number = 0;

  ngOnInit(): void {
    this.carregarServicos();
    
    const user = this.authService.getCurrentUser();
    this.isCliente = !this.authService.isAdmin();

    if (this.isCliente && user) {
      // === MODO CLIENTE ===
      this.tutorIdLogado = user.id || 0; // O ID no token é o ID do Tutor
      this.carregarPetsDoTutor(this.tutorIdLogado);
      this.agendamento.status = 'Agendado'; // Força status
    } else {
      // === MODO ADMIN ===
      this.carregarTodosPets();
    }

    // Verifica se é edição
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Agendamento';
      this.agendaService.buscarPorId(Number(id)).subscribe(data => {
        this.agendamento = data;
      });
    }
    
    // Verifica se veio um serviço pré-selecionado da loja
    this.route.queryParams.subscribe(params => {
      if (params['servicoId']) {
        this.agendamento.servicoId = Number(params['servicoId']);
      }
    });
  }

  carregarServicos() {
    this.servicoPetService.listar().subscribe(data => 
      this.servicos = data.filter(s => s.ativo) // Só mostra ativos
    );
  }

  carregarTodosPets() {
    this.petService.listar().subscribe(data => this.pets = data);
  }

  carregarPetsDoTutor(tutorId: number) {
    this.petService.buscarPorTutor(tutorId).subscribe({
      next: (data) => {
        this.pets = data;
        // Se não tiver nenhum pet, ativa o modo de cadastro automático
        if (this.pets.length === 0) {
          this.precisaCadastrarPet = true;
        }
      },
      error: () => console.error('Erro ao buscar pets do tutor')
    });
  }

  // Alterna entre selecionar existente ou criar novo
  toggleNovoPet() {
    this.precisaCadastrarPet = !this.precisaCadastrarPet;
    // Limpa seleção se for criar novo
    if (this.precisaCadastrarPet) {
      this.agendamento.petid = undefined;
    }
  }

  salvar(): void {
    this.erroMsg = '';

    // 1. Se for CLIENTE e precisar criar PET primeiro
    if (this.isCliente && this.precisaCadastrarPet) {
      this.salvarPetEAgendar();
    } else {
      // 2. Fluxo normal (já tem pet ou é admin)
      this.salvarAgendamento();
    }
  }

  // Fluxo Combinado: Cria Pet -> Pega ID -> Cria Agendamento
  salvarPetEAgendar() {
    if (!this.novoPet.nome || !this.novoPet.especie) {
      this.erroMsg = 'Preencha os dados do seu Pet para continuar.';
      return;
    }

    // Vincula ao tutor logado
    this.novoPet.tutorId = this.tutorIdLogado;

    this.petService.criar(this.novoPet).pipe(
      switchMap((petCriado) => {
        // Pet criado com sucesso! Agora usamos o ID dele
        this.agendamento.petid = petCriado.animalId || petCriado.id;
        
        // Prossegue para criar o agendamento
        if (this.isEdit && this.agendamento.id) {
             return this.agendaService.atualizar(this.agendamento.id, this.agendamento as Agenda);
        }
        return this.agendaService.criar(this.agendamento as Agenda);
      })
    ).subscribe({
      next: () => {
        alert('Pet cadastrado e consulta agendada com sucesso!');
        this.router.navigate(['/agenda']); // Ou para perfil/meus-agendamentos
      },
      error: (err) => this.tratarErro(err)
    });
  }

  // Fluxo Simples: Apenas Agendamento
  salvarAgendamento() {
    let operation;
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
      error: (err) => this.tratarErro(err)
    });
  }

  tratarErro(err: HttpErrorResponse) {
    console.error('Erro:', err);
    this.erroMsg = typeof err.error === 'string' ? err.error : 'Erro ao salvar. Verifique os dados.';
  }
}
