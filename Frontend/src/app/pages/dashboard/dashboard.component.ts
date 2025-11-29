import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { CardProdutosComponent } from '../card-produtos/card-produtos';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, CardProdutosComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements AfterViewInit {

  categories = [
    {
      title: 'Rações Premium',
      desc: 'Ofereça o melhor para o seu pet com nossa seleção de rações premium e super premium.',
      icon: 'bi-egg-fried',
      link: '/produtos'
    },
    {
      title: 'Brinquedos',
      desc: 'Diversão não pode faltar! Encontre brinquedos educativos e mordedores resistentes.',
      icon: 'bi-controller',
      link: '/produtos'
    },
    {
      title: 'Acessórios',
      desc: 'Conforto e estilo andam juntos. Caminhas, coleiras e roupinhas.',
      icon: 'bi-gem',
      link: '/produtos'
    },
    {
      title: 'Estética e Saúde',
      desc: 'Cuidado completo com banho, tosa e tratamentos de higiene.',
      icon: 'bi-droplet-half',
      link: '/agenda/novo'
    }
  ];

  features = [
    {
      title: 'Entrega Flash',
      desc: 'Receba seus pedidos em tempo recorde.',
      icon: 'bi-lightning-charge'
    },
    {
      title: 'Qualidade Premium',
      desc: 'Trabalhamos apenas com as marcas mais conceituadas.',
      icon: 'bi-award'
    },
    {
      title: 'Suporte 24h',
      desc: 'Nossa equipe está pronta para te ajudar.',
      icon: 'bi-headset'
    },
    {
      title: 'Compra Segura',
      desc: 'Seus dados são protegidos com criptografia.',
      icon: 'bi-shield-check'
    },
    {
      title: 'Preço Justo',
      desc: 'Melhores preços e condições de pagamento.',
      icon: 'bi-cash-coin'
    },
    {
      title: 'Clube de Pontos',
      desc: 'Ganhe pontos em todas as compras.',
      icon: 'bi-gift'
    }
  ];

  constructor(
    private router: Router,
    public authService: AuthService
  ) { }

  navigate(link: string): void {
    if ((link.includes('/agenda/novo') || link.includes('/admin')) && !this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    this.router.navigate([link]);
  }

  ngAfterViewInit(): void {
    // Pequeno delay para garantir renderização
    setTimeout(() => {
      this.initScrollAnimations();
    }, 100);
  }

  initScrollAnimations(): void {
    // Verificação de segurança para o Observer
    if ('IntersectionObserver' in window) {
      const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            entry.target.classList.add('in-view');
            observer.unobserve(entry.target);
          }
        });
      }, {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
      });

      const elements = document.querySelectorAll('.animate-on-scroll, .fade-in, .slide-up');
      if (elements.length > 0) {
        elements.forEach((el) => observer.observe(el));
      }
    } else {
      // Fallback: mostra tudo se o navegador for antigo
      const elements = document.querySelectorAll('.animate-on-scroll, .fade-in, .slide-up');
      elements.forEach((el) => el.classList.add('in-view'));
    }
  }
}
