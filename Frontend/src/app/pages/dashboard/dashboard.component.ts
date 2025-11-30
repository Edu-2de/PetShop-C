import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { ProdutoService } from '../../service/produtos/produto.service';
import { Produto } from '../../model/produto.model';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    public authService = inject(AuthService);
    private produtoService = inject(ProdutoService);

    produtosDestaque = signal<Produto[]>([]);
    carregandoProdutos = signal<boolean>(true);

    // LISTA EXPANDIDA COM TEXTOS MAIORES E MAIS ITEMS
    features = [
        {
            title: 'Corpo Clínico Especializado',
            desc: 'Nossa equipe é composta por veterinários pós-graduados em diversas áreas como cardiologia, dermatologia e ortopedia, garantindo diagnósticos precisos e tratamentos eficazes.',
            icon: 'bi-person-badge-fill'
        },
        {
            title: 'Tecnologia Diagnóstica',
            desc: 'Contamos com laboratório próprio e equipamentos de imagem digital de última geração, proporcionando resultados rápidos para que o tratamento do seu pet comece imediatamente.',
            icon: 'bi-activity'
        },
        {
            title: 'Centro Cirúrgico Seguro',
            desc: 'Bloco cirúrgico equipado com monitoramento anestésico avançado e rigorosos protocolos de esterilização, priorizando sempre a segurança total durante os procedimentos.',
            icon: 'bi-hospital-fill'
        },
        {
            title: 'Estética e Bem-estar',
            desc: 'Utilizamos produtos hipoalergênicos premium e técnicas de "Fear Free" no banho e tosa, transformando a higiene em um momento relaxante e positivo para o animal.',
            icon: 'bi-stars'
        },
        {
            title: 'Farmácia Completa',
            desc: 'Encontre todos os medicamentos prescritos, antipulgas e suplementos essenciais sem sair da clínica, com a garantia de procedência e armazenamento correto.',
            icon: 'bi-capsule'
        },
        {
            title: 'Atendimento 24 Horas',
            desc: 'Sabemos que emergências não têm hora. Estamos prontos para acolher seu pet com equipe veterinária de plantão todos os dias, inclusive feriados.',
            icon: 'bi-clock-history'
        },
        {
            title: 'Monitoramento Online',
            desc: 'Para pets internados, oferecemos boletins digitais e fotos para que você acompanhe a recuperação do seu amigo com transparência e tranquilidade.',
            icon: 'bi-phone-fill'
        },
        {
            title: 'Pet Táxi Conforto',
            desc: 'Serviço de busca e leva com veículos climatizados e caixas de transporte seguras, ideal para quem tem rotina corrida mas não abre mão do cuidado.',
            icon: 'bi-car-front-fill'
        }
    ];

    ngOnInit(): void {
        this.carregarDestaques();
    }

    carregarDestaques() {
        this.produtoService.listar().subscribe({
            next: (produtos) => {
                const recentes = produtos.slice(-4).reverse();
                this.produtosDestaque.set(recentes);
                this.carregandoProdutos.set(false);
            },
            error: (e) => {
                this.carregandoProdutos.set(false);
            }
        });
    }
}
