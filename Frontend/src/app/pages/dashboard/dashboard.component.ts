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
        { title: 'Rações', desc: 'Nutrição completa', icon: 'nutrition', link: '/produtos' },
        { title: 'Brinquedos', desc: 'Diversão garantida', icon: 'toys', link: '/produtos' },
        { title: 'Acessórios', desc: 'Estilo e conforto', icon: 'accessories', link: '/produtos' },
        { title: 'Higiene', desc: 'Banho e Tosa', icon: 'hygiene', link: '/agenda/novo' }
    ];

    features = [
        { title: 'Entrega Rápida', desc: 'Receba no conforto de casa.', icon: 'delivery' },
        { title: 'Qualidade', desc: 'Melhores marcas do mercado.', icon: 'quality' },
        { title: 'Atendimento', desc: 'Equipe apaixonada por pets.', icon: 'support' }
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

    getCategoryIcon(icon: string): string {
        const icons: { [key: string]: string } = {
            'nutrition': '🍖', 'toys': '⚽', 'accessories': '🎽', 'hygiene': '🧼'
        };
        return icons[icon] || '📦';
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
