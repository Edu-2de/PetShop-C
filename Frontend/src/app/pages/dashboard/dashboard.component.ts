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
      desc: 'Nutrição balanceada para todas as raças.', 
      icon: 'bi-egg-fried', 
      link: '/produtos' 
    },
    { 
      title: 'Brinquedos', 
      desc: 'Diversão e mordedores resistentes.', 
      icon: 'bi-controller', 
      link: '/produtos' 
    },
    { 
      title: 'Acessórios', 
      desc: 'Conforto, caminhas e coleiras.', 
      icon: 'bi-gem', 
      link: '/produtos' 
    },
    { 
      title: 'Estética e Saúde', 
      desc: 'Banho, tosa e tratamentos.', 
      icon: 'bi-droplet-half', 
      link: '/agenda/novo' 
    }
  ];

  features = [
    { title: 'Entrega Flash', desc: 'Receba em tempo recorde.', icon: 'bi-lightning-charge' },
    { title: 'Qualidade Premium', desc: 'Marcas conceituadas.', icon: 'bi-award' },
    { title: 'Suporte 24h', desc: 'Equipe pronta para ajudar.', icon: 'bi-headset' },
    { title: 'Compra Segura', desc: 'Proteção de dados.', icon: 'bi-shield-check' },
    { title: 'Preço Justo', desc: 'Melhores condições.', icon: 'bi-cash-coin' },
    { title: 'Clube de Pontos', desc: 'Descontos exclusivos.', icon: 'bi-gift' }
  ];

  constructor(
    private router: Router,
    public authService: AuthService
  ) {}

  navigate(link: string): void {
    if ((link.includes('/agenda/novo') || link.includes('/admin')) && !this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    this.router.navigate([link]);
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.initScrollAnimations();
    }, 100);
  }

  initScrollAnimations(): void {
    if ('IntersectionObserver' in window) {
      const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            entry.target.classList.add('in-view');
            observer.unobserve(entry.target);
          }
        });
      }, { threshold: 0.1 });

      const elements = document.querySelectorAll('.animate-on-scroll, .fade-in, .slide-up');
      elements.forEach((el) => observer.observe(el));
    } else {
      // Fallback para navegadores antigos
      document.querySelectorAll('.animate-on-scroll, .fade-in, .slide-up')
        .forEach((el) => el.classList.add('in-view'));
    }
  }
}
