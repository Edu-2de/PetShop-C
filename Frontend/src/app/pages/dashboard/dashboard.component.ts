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

  // Usando ícones Bootstrap Icons em vez de emojis
  categories = [
    { title: 'Rações', desc: 'Nutrição completa', icon: 'bi-egg-fried', link: '/produtos' },
    { title: 'Brinquedos', desc: 'Diversão garantida', icon: 'bi-controller', link: '/produtos' },
    { title: 'Acessórios', desc: 'Estilo e conforto', icon: 'bi-gem', link: '/produtos' },
    { title: 'Higiene', desc: 'Banho e Tosa', icon: 'bi-droplet-half', link: '/agenda/novo' }
  ];

  features = [
    { title: 'Entrega Rápida', desc: 'Receba no conforto de casa.', icon: 'bi-truck' },
    { title: 'Qualidade', desc: 'Melhores marcas do mercado.', icon: 'bi-award' },
    { title: 'Atendimento', desc: 'Equipe apaixonada por pets.', icon: 'bi-heart-pulse' }
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
    this.initScrollAnimations();
  }

  initScrollAnimations(): void {
    const observer = new IntersectionObserver(entries => {
      entries.forEach(e => {
        if (e.isIntersecting) {
          e.target.classList.add('in-view');
          observer.unobserve(e.target);
        }
      });
    }, { threshold: 0.1 });

    setTimeout(() => {
      document.querySelectorAll('.animate-on-scroll, .fade-in, .slide-up').forEach(el => observer.observe(el));
    }, 100);
  }
}
