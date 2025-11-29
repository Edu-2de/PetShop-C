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

    // KPIs Neutros e Informativos
    statsCards = [
        {
            title: 'Receita Mensal',
            value: 'R$ 14.500',
            description: 'Faturamento total acumulado no mês corrente, incluindo serviços e vendas de produtos.',
            icon: 'bi-wallet2' // Ícone neutro
        },
        {
            title: 'Novos Clientes',
            value: '128',
            description: 'Total de novos tutores cadastrados na base de dados nos últimos 30 dias.',
            icon: 'bi-people'
        },
        {
            title: 'Atendimentos',
            value: '42',
            description: 'Número de serviços veterinários e estéticos agendados para a semana atual.',
            icon: 'bi-calendar-check'
        },
        {
            title: 'Inventário',
            value: '850 un.',
            description: 'Quantidade total de itens em estoque. Verifique alertas de reposição.',
            icon: 'bi-box-seam'
        }
    ];

    // Menu Profissional (Texto Rico)
    menuCards = [
        { title: 'Tutores', icon: 'bi-person-vcard', desc: 'Gerenciar base de clientes e históricos.', link: '/tutores' },
        { title: 'Pets', icon: 'bi-gitlab', desc: 'Prontuários médicos e fichas de animais.', link: '/pets' },
        { title: 'Agenda', icon: 'bi-calendar3', desc: 'Grade de horários e disponibilidade.', link: '/agenda' },
        { title: 'Produtos', icon: 'bi-tags', desc: 'Catálogo de venda e controle de estoque.', link: '/produtos' },
        { title: 'Serviços', icon: 'bi-scissors', desc: 'Tabela de preços de banho e tosa.', link: '/servicos' },
        { title: 'Fornecedores', icon: 'bi-truck', desc: 'Gestão de parceiros e compras.', link: '/fornecedores' }
    ];

    constructor(public authService: AuthService) { }

    get user() {
        return this.authService.getCurrentUser();
    }
}
