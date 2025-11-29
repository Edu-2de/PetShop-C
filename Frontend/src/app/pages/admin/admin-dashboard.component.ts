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

  // KPIs (Estes poderiam vir de um serviço de relatórios futuro)
  statsCards = [
    {
      title: 'Faturamento',
      value: 'R$ --',
      description: 'Vendas acumuladas.',
      icon: 'bi-currency-dollar'
    },
    {
      title: 'Equipe',
      value: 'Gestão',
      description: 'Controle de acesso e RH.',
      icon: 'bi-people-fill'
    }
    // ... outros stats existentes
  ];

  // Menu Completo
  menuCards = [
    { title: 'Funcionários', icon: 'bi-briefcase', desc: 'Contratações e perfis.', link: '/funcionarios' },
    { title: 'Categorias', icon: 'bi-tag', desc: 'Organização de produtos.', link: '/categorias' },
    { title: 'Produtos', icon: 'bi-box-seam', desc: 'Estoque e preços.', link: '/produtos' },
    { title: 'Fornecedores', icon: 'bi-truck', desc: 'Parceiros de negócio.', link: '/fornecedores' },
    { title: 'Serviços', icon: 'bi-scissors', desc: 'Banho, tosa e veterinário.', link: '/servicos' },
    { title: 'Tutores', icon: 'bi-person-heart', desc: 'Clientes e contatos.', link: '/tutores' },
    { title: 'Pets', icon: 'bi-gitlab', desc: 'Prontuários animais.', link: '/pets' },
    { title: 'Agenda', icon: 'bi-calendar-check', desc: 'Marcação de horários.', link: '/agenda' }
  ];

  constructor(public authService: AuthService) { }

  get user() {
    return this.authService.getCurrentUser();
  }
}
