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
import { FuncionarioService, Funcionario } from '../../service/funcionarios/funcionario.service';
import { TutorService } from '../../service/tutores/tutor.service';
import { switchMap, of, Observable } from 'rxjs';

@Component({
  selector: 'app-agenda-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './agenda-form.html',
  styleUrls: ['./agenda-form.scss'],
})
export class AgendaFormComponent implements OnInit {
  private agendaService = inject(AgendaService);
  private petService = inject(PetService);
  private servicoPetService = inject(ServicoPetService);
  private funcionarioService = inject(FuncionarioService);
  private tutorService = inject(TutorService);
  public authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  agendamento: Partial<Agenda> = { status: 'Pendente' };
  novoPet: Partial<Pet> = { nome: '', especie: 'Cão', raca: '', sexo: 'Macho' };

  // Campos separados para data e hora
  dataSelecionada: string = '';
  horaSelecionada: string = '';

  pets: Pet[] = [];
  servicos: ServicoPet[] = [];
  funcionarios: Funcionario[] = [];
  funcionariosAptos: any[] = []; // NOVO: Funcionários específicos para o serviço

  isEdit = false;
  titulo = 'Novo Agendamento';
  erroMsg: string = '';

  isCliente = false;
  precisaCadastrarPet = false;
  tutorIdLogado: number = 0;
  servicoSelecionado?: ServicoPet;

  // Horários disponíveis (de 8h às 18h, intervalos de 30min)
  horariosDisponiveis: string[] = [];

  ngOnInit(): void {
    this.gerarHorariosDisponiveis();
    this.carregarServicos();
    this.carregarFuncionarios();

    const userSignal = this.authService.getCurrentUser();
    const user = userSignal();

    console.log('Usuário logado:', user); // DEBUG

    this.isCliente = !this.authService.isAdmin();

    if (this.isCliente && user) {
      // CORREÇÃO: Se usuário não tem tutorId, permitir que ele se torne tutor ao cadastrar pet
      this.tutorIdLogado = user.tutorId || 0;
      console.log('TutorId logado:', this.tutorIdLogado); // DEBUG

      if (this.tutorIdLogado > 0) {
        this.carregarPetsDoTutor(this.tutorIdLogado);
      } else {
        // NOVO: Se não é tutor, permitir cadastro de pet (que criará o tutor automaticamente)
        console.log('Usuário não é tutor ainda. Permitindo cadastro de pet para se tornar tutor.');
        this.pets = [];
        this.precisaCadastrarPet = true; // Força cadastro de pet para virar tutor
      }
      this.agendamento.status = 'Pendente';
    } else {
      this.carregarTodosPets();
    }

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Agendamento';
      this.agendaService.buscarPorId(Number(id)).subscribe((data) => {
        this.agendamento = data;
        if (this.agendamento.dataHora) {
          const dataHora = new Date(this.agendamento.dataHora);
          this.dataSelecionada = dataHora.toISOString().split('T')[0];
          this.horaSelecionada = dataHora.toTimeString().substring(0, 5);
        }
        // Se estiver editando e já tiver serviço, carregar funcionários aptos
        if (this.agendamento.servicoId) {
          this.carregarFuncionariosAptos(this.agendamento.servicoId);
        }
      });
    }

    // Definir data mínima como hoje
    const hoje = new Date();
    this.dataSelecionada = hoje.toISOString().split('T')[0];

    this.route.queryParams.subscribe((params) => {
      if (params['servicoId']) {
        this.agendamento.servicoId = Number(params['servicoId']);
        this.onServicoChange();
      }
    });
  }

  gerarHorariosDisponiveis(): void {
    this.horariosDisponiveis = [];
    for (let hora = 8; hora <= 17; hora++) {
      this.horariosDisponiveis.push(`${hora.toString().padStart(2, '0')}:00`);
      this.horariosDisponiveis.push(`${hora.toString().padStart(2, '0')}:30`);
    }
    // Último horário às 18:00
    this.horariosDisponiveis.push('18:00');
  }

  onServicoChange(): void {
    if (this.agendamento.servicoId) {
      this.servicoSelecionado = this.servicos.find(
        (s) => s.servicoId === this.agendamento.servicoId
      );
      this.carregarFuncionariosAptos(this.agendamento.servicoId);
      // Reset funcionário selecionado quando troca serviço
      this.agendamento.funcionarioId = undefined;
    }
  }

  // NOVO: Carregar apenas funcionários aptos para o serviço
  carregarFuncionariosAptos(servicoId: number): void {
    this.servicoPetService.buscarFuncionariosAptos(servicoId).subscribe({
      next: (funcionarios) => {
        this.funcionariosAptos = funcionarios;

        // Se só há um funcionário apto, auto-selecionar
        if (funcionarios.length === 1) {
          this.agendamento.funcionarioId = funcionarios[0].funcionarioId;
        }
      },
      error: (err) => {
        console.error('Erro ao carregar funcionários aptos:', err);
        this.funcionariosAptos = [];
      },
    });
  }

  atualizarDataHora(): void {
    if (this.dataSelecionada && this.horaSelecionada) {
      // CORREÇÃO: Criar data sem conversão de timezone
      // Separar componentes da data e hora
      const [ano, mes, dia] = this.dataSelecionada.split('-').map(Number);
      const [hora, minutos] = this.horaSelecionada.split(':').map(Number);

      // Criar data local sem conversão UTC
      const dataHora = new Date(ano, mes - 1, dia, hora, minutos, 0, 0);

      // DEBUG: Log para verificar valores
      console.log('🔍 Validação hora:', {
        hora,
        minutos,
        dataHora: dataHora.toISOString(),
        horaLocal: dataHora.getHours(),
        dataLocal: dataHora.toLocaleString('pt-BR'),
      });

      // VALIDAÇÃO: Verificar se a data/hora não é no passado
      const agora = new Date();
      if (dataHora <= agora) {
        this.erroMsg = '❌ Não é possível agendar para uma data/hora que já passou!';
        return;
      }

      // VALIDAÇÃO: Verificar se é domingo
      if (dataHora.getDay() === 0) {
        // 0 = Domingo
        this.erroMsg = '❌ Não atendemos aos domingos. Por favor, escolha outro dia da semana.';
        return;
      }

      // VALIDAÇÃO: Verificar horário de funcionamento (8:00 às 18:00 inclusive)
      if (hora < 8 || hora > 18) {
        this.erroMsg =
          '❌ Atendemos apenas das 8:00 às 18:00. Por favor, escolha um horário dentro deste intervalo.';
        console.error('❌ Horário rejeitado:', hora);
        return;
      }

      console.log('✅ Horário aceito:', hora);

      // Se passou por todas as validações, limpar erro e atualizar
      this.erroMsg = '';
      this.agendamento.dataHora = dataHora;
    }
  }

  // NOVO: Método para validar data selecionada
  onDataChange(): void {
    if (this.dataSelecionada) {
      const dataSelecionada = new Date(this.dataSelecionada);
      const hoje = new Date();
      hoje.setHours(0, 0, 0, 0); // Zerar horas para comparar apenas a data

      if (dataSelecionada < hoje) {
        this.erroMsg = '❌ Não é possível selecionar uma data que já passou!';
        this.dataSelecionada = hoje.toISOString().split('T')[0]; // Reset para hoje
        return;
      }

      if (dataSelecionada.getDay() === 0) {
        // Domingo
        this.erroMsg = '❌ Não atendemos aos domingos. Por favor, escolha outro dia.';
        return;
      }

      // Verificar se não é muito no futuro (6 meses)
      const seiseMesesFuture = new Date();
      seiseMesesFuture.setMonth(seiseMesesFuture.getMonth() + 6);
      if (dataSelecionada > seiseMesesFuture) {
        this.erroMsg = '❌ Não é possível agendar com mais de 6 meses de antecedência.';
        return;
      }

      this.erroMsg = '';
      this.atualizarDataHora();
    }
  }

  // NOVO: Método para validar hora selecionada - CORRIGIDO
  onHoraChange(): void {
    if (this.horaSelecionada) {
      const [horas, minutos] = this.horaSelecionada.split(':').map(Number);

      // Corrige: aceita de 8:00 até 18:00 (inclusive)
      if (horas < 8 || horas > 18) {
        this.erroMsg = '❌ Horário deve estar entre 8:00 e 18:00.';
        this.horaSelecionada = '08:00'; // Reset para 8:00
        return;
      }

      this.erroMsg = '';
      this.atualizarDataHora();
    }
  }

  carregarServicos() {
    this.servicoPetService.listarAtivos().subscribe((data) => (this.servicos = data));
  }

  carregarFuncionarios() {
    this.funcionarioService.listar().subscribe({
      next: (data) => {
        this.funcionarios = data;
      },
      error: (err) => {
        console.error('Erro ao carregar funcionários:', err);
      },
    });
  }

  carregarTodosPets() {
    this.petService.listar().subscribe((data) => (this.pets = data));
  }

  carregarPetsDoTutor(tutorId: number) {
    console.log('🐾 Carregando pets do tutor:', tutorId);
    this.petService.buscarPorTutor(tutorId).subscribe({
      next: (data) => {
        this.pets = data;
        console.log('✅ Pets carregados do tutor:', this.pets.length, this.pets);

        if (this.pets.length === 0) {
          console.log('⚠️ Nenhum pet encontrado, forçando cadastro');
          this.precisaCadastrarPet = true;
        } else {
          console.log('✅ Pets disponíveis para seleção');
          this.precisaCadastrarPet = false;

          // DEBUG: Verificar estrutura dos pets
          this.pets.forEach((pet) => {
            console.log('🔍 Pet:', {
              animalId: pet.animalId,
              nome: pet.nome,
              especie: pet.especie,
              tutorId: pet.tutorId,
            });
          });
        }
      },
      error: (err) => {
        console.error('❌ Erro ao buscar pets do tutor:', err);
        this.pets = [];
        this.precisaCadastrarPet = true;
      },
    });
  }

  toggleNovoPet() {
    this.precisaCadastrarPet = !this.precisaCadastrarPet;
    if (this.precisaCadastrarPet) {
      this.agendamento.animalId = undefined;
      this.agendamento.petid = undefined;
    }
  }

  onPetChange(): void {
    console.log('🐾 Pet selecionado:', this.agendamento.animalId);
    const petSelecionado = this.pets.find((p) => p.animalId === this.agendamento.animalId);
    console.log('🔍 Dados do pet selecionado:', petSelecionado);
  }

  salvar(): void {
    this.erroMsg = '';

    // Executar validações antes de prosseguir
    if (!this.validarAgendamento()) {
      return;
    }

    // Atualizar data/hora antes de salvar
    this.atualizarDataHora();

    // 🆕 NOVO: Se é cliente E precisa cadastrar pet, usar endpoint completo
    if (this.isCliente && this.precisaCadastrarPet) {
      this.salvarAgendamentoCompleto();
    } else {
      // Fluxo tradicional: já tem pet selecionado
      this.salvarAgendamento();
    }
  }

  salvarPetEAgendar() {
    if (!this.novoPet.nome || !this.novoPet.especie) {
      this.erroMsg = 'Preencha os dados do seu Pet para continuar.';
      return;
    }

    const user = this.authService.getCurrentUserValue();
    if (!user) {
      this.erroMsg = '❌ Erro: Usuário não está logado.';
      return;
    }

    console.log('Criando pet para usuário:', user);

    // CORREÇÃO: Se usuário não tem tutorId, vamos criá-lo automaticamente via backend
    if (this.tutorIdLogado <= 0) {
      // Primeiro criar o tutor, depois o pet
      this.criarTutorEPet();
    } else {
      // Usuário já é tutor, só criar o pet
      this.novoPet.tutorId = this.tutorIdLogado;
      this.criarPetEAgendar();
    }
  }

  // NOVO: Criar tutor automaticamente e depois o pet
  criarTutorEPet() {
    const user = this.authService.getCurrentUserValue();
    if (!user) {
      this.erroMsg = '❌ Erro: Usuário não identificado.';
      return;
    }

    // Criar tutor usando dados do usuário
    const novoTutor = {
      nome: user.nome,
      email: user.email,
      telefone: '', // Usuário pode preencher depois
      endereco: '', // Usuário pode preencher depois
      senha: '', // Não precisa, já tem usuário
    };

    console.log('Criando tutor automaticamente:', novoTutor);

    this.tutorService.criar(novoTutor).subscribe({
      next: (tutorCriado) => {
        console.log('Tutor criado:', tutorCriado);
        this.tutorIdLogado = tutorCriado.tutorId;
        this.novoPet.tutorId = tutorCriado.tutorId;

        // Agora criar o pet
        this.criarPetEAgendar();
      },
      error: (err) => {
        console.error('Erro ao criar tutor:', err);
        this.erroMsg = '❌ Erro ao criar perfil de tutor. Tente novamente.';
      },
    });
  }

  // NOVO: Método separado para criar pet e agendar
  criarPetEAgendar() {
    console.log('Criando pet com dados:', this.novoPet);

    this.petService
      .criar(this.novoPet)
      .pipe(
        switchMap((petCriado) => {
          console.log('Pet criado:', petCriado);
          this.agendamento.animalId = petCriado.animalId || petCriado.id;
          this.agendamento.petid = petCriado.animalId || petCriado.id;

          if (this.isEdit && this.agendamento.agendamentoId) {
            return this.agendaService.atualizar(
              this.agendamento.agendamentoId,
              this.agendamento as Agenda
            );
          }
          return this.agendaService.criar(this.agendamento as Agenda);
        })
      )
      .subscribe({
        next: () => {
          alert('Pet cadastrado e consulta agendada com sucesso!');
          this.router.navigate(['/agenda']);
        },
        error: (err: any) => this.tratarErro(err),
      });
  }

  salvarAgendamento() {
    // Formatar dataHora corretamente antes de enviar
    const agendamentoParaEnviar = {
      ...this.agendamento,
      dataHora: this.formatarDataHoraParaBackend(this.agendamento.dataHora!),
    };

    let operation: Observable<any>;

    if (this.isEdit && this.agendamento.agendamentoId) {
      operation = this.agendaService.atualizar(
        this.agendamento.agendamentoId,
        agendamentoParaEnviar as any
      );
    } else {
      operation = this.agendaService.criar(agendamentoParaEnviar as any);
    }

    operation.subscribe({
      next: () => {
        alert(this.isEdit ? 'Atualizado com sucesso!' : 'Agendamento realizado!');
        this.router.navigate(['/agenda']);
      },
      error: (err: any) => this.tratarErro(err),
    });
  }

  // NOVO: Método para criar tutor e animal automaticamente via backend
  salvarAgendamentoCompleto() {
    const user = this.authService.getCurrentUserValue();
    if (!user) {
      this.erroMsg = '❌ Erro: Usuário não está logado.';
      return;
    }

    // Formatar dataHora corretamente para enviar ao backend
    const dataHoraFormatada = this.formatarDataHoraParaBackend(this.agendamento.dataHora!);

    const agendamentoCompleto = {
      servicoId: this.agendamento.servicoId,
      funcionarioId: this.agendamento.funcionarioId,
      dataHora: dataHoraFormatada,
      status: this.agendamento.status || 'Pendente',
      observacoes: this.agendamento.observacoes,

      // Dados do tutor (usar dados do usuário logado)
      nomeTutor: user.nome,
      emailTutor: user.email,
      telefoneTutor: '', // Usuário pode cadastrar depois
      enderecoTutor: 'A definir', // Usuário pode cadastrar depois

      // Dados do pet
      nomeAnimal: this.novoPet.nome,
      especieAnimal: this.novoPet.especie,
      racaAnimal: this.novoPet.raca || 'SRD',
      sexoAnimal: this.novoPet.sexo,
      dataNascimentoAnimal: this.novoPet.dataNascimento,
      pelagemAnimal: this.novoPet.pelagem || 'Curta',
      observacoesAnimal: this.novoPet.observacoes,
    };

    console.log('Criando agendamento completo:', agendamentoCompleto);

    this.agendaService.criarCompleto(agendamentoCompleto).subscribe({
      next: (agendamentoCriado) => {
        console.log('Agendamento completo criado:', agendamentoCriado);

        // RECARREGAR dados do usuário para atualizar tutorId
        this.authService.reloadUserInfo().subscribe({
          next: (userAtualizado: any) => {
            console.log('✅ Usuário atualizado após criar tutor:', userAtualizado);
          },
          error: (err: any) => console.error('Erro ao atualizar usuário:', err),
        });

        alert(
          '✅ Agendamento realizado com sucesso!\n\n🐾 Seu pet foi cadastrado automaticamente.\n👤 Agora você pode fazer novos agendamentos!'
        );
        this.router.navigate(['/agenda']);
      },
      error: (err) => {
        console.error('Erro ao criar agendamento completo:', err);
        this.tratarErro(err);
      },
    });
  }

  tratarErro(err: any) {
    console.error('Erro:', err);
    if (err.error && typeof err.error === 'string') {
      this.erroMsg = err.error;
    } else {
      this.erroMsg = 'Erro ao salvar. Verifique se todos os campos estão preenchidos corretamente.';
    }
  }

  // Métodos auxiliares para validação de datas
  getMinDate(): string {
    const hoje = new Date();
    return hoje.toISOString().split('T')[0];
  }

  getMaxDate(): string {
    const seisMesesFuture = new Date();
    seisMesesFuture.setMonth(seisMesesFuture.getMonth() + 6);
    return seisMesesFuture.toISOString().split('T')[0];
  }

  // NOVO: Formatar data/hora para enviar ao backend sem conversão de timezone
  formatarDataHoraParaBackend(data: Date): string {
    const ano = data.getFullYear();
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const dia = String(data.getDate()).padStart(2, '0');
    const hora = String(data.getHours()).padStart(2, '0');
    const minutos = String(data.getMinutes()).padStart(2, '0');
    const segundos = String(data.getSeconds()).padStart(2, '0');

    // Formato: YYYY-MM-DDTHH:mm:ss (sem timezone)
    return `${ano}-${mes}-${dia}T${hora}:${minutos}:${segundos}`;
  }

  // NOVO: Validação mais robusta no método salvar - CORRIGIDA
  validarAgendamento(): boolean {
    // Validar data/hora
    if (!this.dataSelecionada || !this.horaSelecionada) {
      this.erroMsg = '❌ Por favor, selecione data e horário para o agendamento.';
      return false;
    }

    const dataHora = new Date(`${this.dataSelecionada}T${this.horaSelecionada}:00`);
    const agora = new Date();

    if (dataHora <= agora) {
      this.erroMsg = '❌ Não é possível agendar para uma data/hora que já passou!';
      return false;
    }

    if (dataHora.getDay() === 0) {
      this.erroMsg = '❌ Não atendemos aos domingos. Por favor, escolha outro dia.';
      return false;
    }

    // Corrige validação de horário
    const hora = dataHora.getHours();
    if (hora < 8 || hora > 18) {
      this.erroMsg = '❌ Horário deve estar entre 8:00 e 18:00.';
      return false;
    }

    // Validar serviço
    if (!this.agendamento.servicoId) {
      this.erroMsg = '❌ Por favor, selecione um serviço.';
      return false;
    }

    // Validar pet
    if (!this.agendamento.animalId && !this.precisaCadastrarPet) {
      this.erroMsg = '❌ Por favor, selecione um pet ou cadastre um novo.';
      return false;
    }

    if (this.precisaCadastrarPet && (!this.novoPet.nome || !this.novoPet.especie)) {
      this.erroMsg = '❌ Por favor, preencha os dados do novo pet.';
      return false;
    }

    return true;
  }
}
