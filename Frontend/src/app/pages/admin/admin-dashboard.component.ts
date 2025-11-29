import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent {
  
  dataAtual = new Date();

  // CORREÇÃO: Adicionado a propriedade 'icon' que faltava
  statsCards = [
    { 
      title: 'Receita Acumulada', 
      value: 'R$ 14.500,00', 
      description: 'Valor total faturado com vendas e serviços no mês vigente.',
      icon: 'bi-currency-dollar'
    },
    { 
      title: 'Base de Clientes', 
      value: '1.248', 
      description: 'Total de tutores ativos cadastrados na plataforma.',
      icon: 'bi-people'
    },
    { 
      title: 'Atendimentos', 
      value: '42', 
      description: 'Agendamentos confirmados para a semana atual.',
      icon: 'bi-calendar-check'
    },
    { 
      title: 'Inventário', 
      value: '850 Itens', 
      description: 'Quantidade total de produtos em estoque.',
      icon: 'bi-box-seam'
    }
  ];

  menuCards = [
    {
      title: 'Gestão de Tutores',
      icon: 'bi-people',
      desc: 'Acesse o banco de dados completo de clientes.',
      link: '/tutores'
    },
    {
      title: 'Prontuário Animal',
      icon: 'bi-folder2-open', 
      desc: 'Controle detalhado das fichas dos pets.',
      link: '/pets'
    },
    {
      title: 'Agenda e Horários',
      icon: 'bi-calendar3',
      desc: 'Organização completa dos serviços.',
      link: '/agenda'
    },
    {
      title: 'Catálogo de Produtos',
      icon: 'bi-box-seam',
      desc: 'Gerenciamento de inventário.',
      link: '/produtos'
    },
    {
      title: 'Serviços Oferecidos',
      icon: 'bi-list-check',
      desc: 'Configuração do menu de serviços.',
      link: '/servicos'
    },
    {
      title: 'Rede de Fornecedores',
      icon: 'bi-truck',
      desc: 'Cadastro de parceiros comerciais.',
      link: '/fornecedores'
    }
  ];

  // CORREÇÃO: Sintaxe do construtor arrumada
  constructor(public authService: AuthService) {}

  get user() {
    return this.authService.getCurrentUser();
  }
}
