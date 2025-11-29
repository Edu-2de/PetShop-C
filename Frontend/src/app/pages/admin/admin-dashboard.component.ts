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

    // Cards de Estatísticas (KPIs)
    statsCards = [
        { title: 'Agendamentos Hoje', value: '12', icon: 'bi-calendar-check', color: 'primary' },
        { title: 'Vendas do Mês', value: 'R$ 4.5k', icon: 'bi-currency-dollar', color: 'success' },
        { title: 'Novos Clientes', value: '8', icon: 'bi-people', color: 'info' },
        { title: 'Alertas de Estoque', value: '3', icon: 'bi-exclamation-triangle', color: 'warning' }
    ];

    // Cards de Navegação (Menu Rápido)
    menuCards = [
        {
            title: 'Tutores',
            icon: 'bi-person-vcard',
            desc: 'Gerenciar base de clientes',
            link: '/tutores'
        },
        {
            title: 'Pets',
            icon: 'bi-gitlab', // ou bi-heart-pulse
            desc: 'Prontuários e cadastros',
            link: '/pets'
        },
        {
            title: 'Agenda',
            icon: 'bi-calendar3',
            desc: 'Controle de horários',
            link: '/agenda'
        },
        {
            title: 'Produtos',
            icon: 'bi-box-seam',
            desc: 'Catálogo e estoque',
            link: '/produtos'
        },
        {
            title: 'Serviços',
            icon: 'bi-scissors',
            desc: 'Tabela de preços',
            link: '/servicos'
        },
        {
            title: 'Fornecedores',
            icon: 'bi-truck',
            desc: 'Parceiros e compras',
            link: '/fornecedores'
        }
    ];

    constructor(public authService: AuthService) { }

    get user() {
        return this.authService.getCurrentUser();
    }
}
