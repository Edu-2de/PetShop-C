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
  
  // KPI Cards (Estatísticas Rápidas) - SEM EMOJIS
  statsCards = [
    { 
      title: 'Agendamentos Hoje', 
      value: '12', 
      icon: 'bi-calendar-check', 
      color: 'primary',
      trend: '+2.5%',
      trendUp: true
    },
    { 
      title: 'Faturamento Mensal', 
      value: 'R$ 14.500', 
      icon: 'bi-currency-dollar', 
      color: 'success',
      trend: '+12%',
      trendUp: true 
    },
    { 
      title: 'Novos Clientes', 
      value: '28', 
      icon: 'bi-people', 
      color: 'info',
      trend: '-1%',
      trendUp: false
    },
    { 
      title: 'Estoque Baixo', 
      value: '5', 
      icon: 'bi-box-seam', 
      color: 'warning',
      trend: 'Atenção',
      trendUp: false
    }
  ];

  // Menu de Navegação Profissional
  menuCards = [
    {
      title: 'Gestão de Tutores',
      icon: 'bi-person-vcard',
      desc: 'Cadastrar, editar e visualizar base de clientes.',
      link: '/tutores',
      color: 'primary'
    },
    {
      title: 'Controle de Pets',
      icon: 'bi-gitlab', 
      desc: 'Prontuários, histórico e fichas dos animais.',
      link: '/pets',
      color: 'danger'
    },
    {
      title: 'Agenda de Serviços',
      icon: 'bi-calendar3',
      desc: 'Controle de horários de banho, tosa e veterinário.',
      link: '/agenda',
      color: 'success'
    },
    {
      title: 'Catálogo de Produtos',
      icon: 'bi-tags',
      desc: 'Gerenciar estoque, preços e categorias.',
      link: '/produtos',
      color: 'warning'
    },
    {
      title: 'Serviços Oferecidos',
      icon: 'bi-scissors',
      desc: 'Configurar tipos de serviços e valores.',
      link: '/servicos',
      color: 'info'
    },
    {
      title: 'Fornecedores',
      icon: 'bi-truck',
      desc: 'Gerenciamento de parceiros e compras.',
      link: '/fornecedores',
      color: 'secondary'
    }
  ];

  constructor(public authService: AuthService) {}

  get user() {
    return this.authService.getCurrentUser();
  }
}
