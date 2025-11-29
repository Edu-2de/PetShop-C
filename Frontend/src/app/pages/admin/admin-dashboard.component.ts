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
  adminCards = [
    {
      title: 'Gerenciar Tutores',
      icon: 'bi-people-fill',
      description: 'Cadastrar e gerenciar tutores de pets',
      link: '/tutores',
      color: 'primary'
    },
    {
      title: 'Gerenciar Pets',
      icon: 'bi-heart-fill',
      description: 'Cadastrar e gerenciar animais',
      link: '/pets',
      color: 'danger'
    },
    {
      title: 'Gerenciar Produtos',
      icon: 'bi-box-seam',
      description: 'Cadastrar e gerenciar produtos',
      link: '/produtos',
      color: 'warning'
    },
    {
      title: 'Gerenciar Serviços',
      icon: 'bi-scissors',
      description: 'Cadastrar e gerenciar serviços',
      link: '/servicos',
      color: 'info'
    },
    {
      title: 'Agenda',
      icon: 'bi-calendar-check',
      description: 'Gerenciar agendamentos',
      link: '/agenda',
      color: 'success'
    },
    {
      title: 'Fornecedores',
      icon: 'bi-truck',
      description: 'Gerenciar fornecedores',
      link: '/fornecedores',
      color: 'secondary'
    }
  ];

  constructor(public authService: AuthService) {}

  get user() {
    return this.authService.getCurrentUser();
  }
}
