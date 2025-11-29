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
      desc: 'Ofereça o melhor para o seu pet com nossa seleção de rações premium e super premium. Nutrição balanceada para todas as raças, portes e necessidades específicas.', 
      icon: 'bi-egg-fried', 
      link: '/produtos' 
    },
    { 
      title: 'Brinquedos', 
      desc: 'Diversão não pode faltar! Encontre brinquedos educativos, mordedores resistentes e pelúcias que ajudam no desenvolvimento e reduzem o estresse do seu animal.', 
      icon: 'bi-controller', 
      link: '/produtos' 
    },
    { 
      title: 'Acessórios', 
      desc: 'Conforto e estilo andam juntos. Caminhas macias, coleiras seguras, roupinhas da moda e caixas de transporte para garantir o bem-estar em qualquer lugar.', 
      icon: 'bi-gem', 
      link: '/produtos' 
    },
    { 
      title: 'Estética e Saúde', 
      desc: 'Cuidado completo com banho, tosa e tratamentos de higiene. Nossos profissionais são treinados para deixar seu pet limpo, cheiroso e muito feliz.', 
      icon: 'bi-droplet-half', 
      link: '/agenda/novo' 
    }
  ];

  // Lista expandida com 6 itens
  features = [
    { 
      title: 'Entrega Flash', 
      desc: 'Receba seus pedidos em tempo recorde. Nossa logística é otimizada para que seu pet não espere.', 
      icon: 'bi-lightning-charge' 
    },
    { 
      title: 'Qualidade Premium', 
      desc: 'Trabalhamos apenas com as marcas mais conceituadas e produtos certificados pelo mercado.', 
      icon: 'bi-award' 
    },
    { 
      title: 'Suporte 24h', 
      desc: 'Dúvidas? Nossa equipe de especialistas está pronta para te ajudar a qualquer momento.', 
      icon: 'bi-headset' 
    },
    { 
      title: 'Compra Segura', 
      desc: 'Seus dados são protegidos com criptografia de ponta. Compre com total tranquilidade.', 
      icon: 'bi-shield-check' 
    },
    { 
      title: 'Preço Justo', 
      desc: 'Oferecemos os melhores preços e condições de pagamento do mercado para você economizar.', 
      icon: 'bi-cash-coin' 
    },
    { 
      title: 'Clube de Pontos', 
      desc: 'Ganhe pontos em todas as compras e troque por descontos exclusivos na próxima visita.', 
      icon: 'bi-gift' 
    }
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

  getCategoryIcon(icon: string): string {
    // Mapeamento opcional se quiser usar emojis como fallback, 
    // mas estamos usando classes do Bootstrap Icons direto no HTML agora.
    return ''; 
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
