import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ServicoPetService } from '../../../service/servico-pet/servico-pet';
import { CreateServicoPet, UpdateServicoPet } from '../../../model/servico-pet.model';

@Component({
  selector: 'app-servico-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './servicos-pet.form.html',
  styleUrls: ['../servicos-pet.scss']
})
export class ServicoFormComponent implements OnInit {
  private servicoService = inject(ServicoPetService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  servico: CreateServicoPet = {
    nome: '',
    descricao: '',
    preco: 0,
    duracaoMinutos: 60,
    ativo: true,
    cargosResponsaveis: [],
    funcionariosAptosIds: []
  };

  isEdit = false;
  servicoId = 0;
  erroMsg = '';

  // NOVO: Lista de cargos disponíveis
  cargosDisponiveis: string[] = [];
  cargosComuns = ['Veterinário', 'Atendente', 'Tosador', 'Gerente'];

  ngOnInit(): void {
    this.carregarCargosDisponiveis();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.servicoId = Number(id);
      this.servicoService.buscarPorId(this.servicoId).subscribe({
        next: (data) => {
          this.servico = {
            nome: data.nome,
            descricao: data.descricao,
            preco: data.preco,
            duracaoMinutos: data.duracaoMinutos,
            ativo: data.ativo,
            cargosResponsaveis: data.cargosResponsaveis || [],
            funcionariosAptosIds: []
          };
        },
        error: (err) => {
          console.error('Erro ao carregar serviço:', err);
          this.erroMsg = 'Erro ao carregar dados do serviço.';
        }
      });
    }
  }

  carregarCargosDisponiveis(): void {
    this.servicoService.listarCargosDisponiveis().subscribe({
      next: (cargos) => {
        this.cargosDisponiveis = [...new Set([...this.cargosComuns, ...cargos])].sort();
      },
      error: (err) => {
        console.error('Erro ao carregar cargos:', err);
        this.cargosDisponiveis = this.cargosComuns;
      }
    });
  }

  // NOVO: Toggle cargo na lista
  toggleCargo(cargo: string): void {
    const index = this.servico.cargosResponsaveis.indexOf(cargo);
    if (index > -1) {
      this.servico.cargosResponsaveis.splice(index, 1);
    } else {
      this.servico.cargosResponsaveis.push(cargo);
    }
  }

  // NOVO: Verificar se cargo está selecionado
  isCargoSelecionado(cargo: string): boolean {
    return this.servico.cargosResponsaveis.includes(cargo);
  }

  salvar(): void {
    this.erroMsg = '';

    // Validações
    if (!this.servico.nome.trim()) {
      this.erroMsg = 'Nome do serviço é obrigatório.';
      return;
    }

    if (this.servico.preco <= 0) {
      this.erroMsg = 'Preço deve ser maior que zero.';
      return;
    }

    if (this.servico.duracaoMinutos <= 0) {
      this.erroMsg = 'Duração deve ser maior que zero.';
      return;
    }

    if (this.servico.cargosResponsaveis.length === 0) {
      this.erroMsg = 'Selecione pelo menos um cargo responsável pelo serviço.';
      return;
    }

    console.log('Salvando serviço com cargos:', this.servico.cargosResponsaveis);

    if (this.isEdit) {
      const updateData: UpdateServicoPet = {
        nome: this.servico.nome,
        descricao: this.servico.descricao,
        preco: this.servico.preco,
        duracaoMinutos: this.servico.duracaoMinutos,
        ativo: this.servico.ativo,
        cargosResponsaveis: this.servico.cargosResponsaveis,
        funcionariosAptosIds: []
      };

      this.servicoService.atualizar(this.servicoId, updateData).subscribe({
        next: () => {
          alert('Serviço atualizado com sucesso!');
          this.router.navigate(['/servicos']);
        },
        error: (err) => this.tratarErro(err)
      });
    } else {
      this.servicoService.criar(this.servico).subscribe({
        next: () => {
          alert('Serviço criado com sucesso!');
          this.router.navigate(['/servicos']);
        },
        error: (err) => this.tratarErro(err)
      });
    }
  }

  private tratarErro(err: any): void {
    console.error('Erro:', err);
    if (err.error && typeof err.error === 'string') {
      this.erroMsg = err.error;
    } else if (err.error && err.error.message) {
      this.erroMsg = err.error.message;
    } else {
      this.erroMsg = 'Erro ao salvar serviço. Verifique os dados e tente novamente.';
    }
  }
}
