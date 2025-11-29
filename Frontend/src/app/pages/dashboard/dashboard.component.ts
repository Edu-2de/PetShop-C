import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
// CORREÇÃO: Adicionado RouterLink na lista de imports
import { RouterModule, Router, RouterLink } from '@angular/router';
import { ProdutoService } from '../../service/produtos/produto.service';
import { Produto } from '../../model/produto.model';
import { AuthService } from '../../service/auth/auth.service';
// O import abaixo agora funcionará porque renomeamos a classe no passo 1
import { CardProdutosComponent } from '../card-produtos/card-produtos';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, CardProdutosComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements AfterViewInit, OnDestroy {
  // Informações para compradores
  categories = [
    { title: 'Rações', desc: 'Alimentação balanceada para seu pet', icon: 'nutrition', link: '/produtos' },
    { title: 'Brinquedos', desc: 'Diversão garantida', icon: 'toys', link: '/produtos' },
    { title: 'Acessórios', desc: 'Coleiras, camas e muito mais', icon: 'accessories', link: '/produtos' },
    { title: 'Higiene', desc: 'Banho e tosa profissional', icon: 'hygiene', link: '/agenda/novo' }
  ];

  features = [
    { title: 'Entrega Rápida', desc: 'Receba seus produtos no conforto da sua casa com agilidade.', icon: 'delivery' },
    { title: 'Produtos de Qualidade', desc: 'Trabalhamos apenas com marcas confiáveis e testadas.', icon: 'quality' },
    { title: 'Atendimento Especializado', desc: 'Nossa equipe está pronta para te ajudar a escolher o melhor.', icon: 'support' },
    { title: 'Agendamento Online', desc: 'Agende banho e tosa de forma rápida e prática.', icon: 'schedule' },
    { title: 'Promoções Exclusivas', desc: 'Ofertas especiais para nossos clientes cadastrados.', icon: 'promotion' },
    { title: 'Programa de Fidelidade', desc: 'Acumule pontos e ganhe descontos em suas compras.', icon: 'loyalty' }
  ];

  // Placeholder para banners (evita erro 404 local)
  banners: string[] = [
    'https://placehold.co/1200x400/1abc9c/ffffff?text=Bem-vindo+ao+SIGA-PET',
    'https://placehold.co/1200x400/3498db/ffffff?text=Cuidamos+com+Amor',
    'https://placehold.co/1200x400/9b59b6/ffffff?text=Agende+seu+Banho+e+Tosa'
  ];

  currentBanner = 0;
  rotationInterval: any;
  rotationDelay = 8000;
  paused = false;
  bannersLoaded = true; // Força como true para exibir os placeholders

  produtos: Produto[] = [];
  produtosCarregando = true;
  produtosErro = false;

  constructor(
    private router: Router,
    private produtoService: ProdutoService,
    public authService: AuthService
  ) {
    // this.loadAvailableBanners(); // Comentado para usar placeholders e evitar erro 404
    this.initBannerRotation(); // Inicia rotação dos placeholders
    this.loadProdutos();
  }

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  ngAfterViewInit(): void {
    this.initScrollAnimations();
    this.handleHeaderShadowOnScroll();
  }

  // Método original comentado para evitar erros de 404 no console
  /*
  loadAvailableBanners(): void { ... }
  */

  loadProdutos(): void {
    this.produtoService.listar().subscribe({
      next: (produtos) => {
        this.produtos = produtos.filter(p => p.ativo).slice(0, 8);
        this.produtosCarregando = false;
      },
      error: (err) => {
        console.error('Erro ao carregar produtos:', err);
        this.produtosErro = true;
        this.produtosCarregando = false;
      }
    });
  }

  navigate(link: string): void {
    if ((link.includes('/agenda/novo') || link.includes('/admin')) && !this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    this.router.navigate([link]);
  }

  initBannerRotation(): void {
    if (this.banners.length <= 1) return;
    if (this.rotationInterval) clearInterval(this.rotationInterval);

    this.rotationInterval = setInterval(() => {
      if (!this.paused) {
        this.nextBanner();
      }
    }, this.rotationDelay);
  }

  pauseRotation(): void {
    this.paused = true;
  }

  resumeRotation(): void {
    this.paused = false;
  }

  nextBanner(): void {
    if (this.banners.length === 0) return;
    this.currentBanner = (this.currentBanner + 1) % this.banners.length;
  }

  prevBanner(): void {
    if (this.banners.length === 0) return;
    this.currentBanner = (this.currentBanner - 1 + this.banners.length) % this.banners.length;
  }

  getProdutoImagem(produto: Produto): string {
    if (produto.imagens && produto.imagens.length > 0) {
      return produto.imagens[0].url;
    }
    return 'assets/images/no-image.png';
  }

  formatPreco(preco: number): string {
    return preco.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  verProduto(produto: Produto): void {
    // Redireciona para visualização (ajuste conforme suas rotas)
    this.router.navigate(['/produtos/editar', produto.produtoId]);
  }

  getCategoryIcon(icon: string): string {
    const icons: { [key: string]: string } = {
      'nutrition': '🍖',
      'toys': '⚽',
      'accessories': '🎽',
      'hygiene': '🧼'
    };
    return icons[icon] || '📦';
  }

  getFeatureIcon(icon: string): string {
    const icons: { [key: string]: string } = {
      'delivery': '🚚',
      'quality': '⭐',
      'support': '👥',
      'schedule': '📅',
      'promotion': '🏷️',
      'loyalty': '🎁'
    };
    return icons[icon] || '✓';
  }

  initScrollAnimations(): void {
    const observer = new IntersectionObserver(entries => {
      entries.forEach(e => {
        if (e.isIntersecting) {
          e.target.classList.add('in-view');
          observer.unobserve(e.target);
        }
      });
    }, { threshold: 0.15 });

    setTimeout(() => {
      document.querySelectorAll('.animate-on-scroll, .fade-in, .slide-up').forEach(el => observer.observe(el));
    }, 100);
  }

  handleHeaderShadowOnScroll(): void {
    const nav = document.getElementById('mainNav');
    if (!nav) return;
    window.addEventListener('scroll', () => {
      if (window.scrollY > 10) {
        nav.classList.add('scrolled');
      } else {
        nav.classList.remove('scrolled');
      }
    });
  }

  ngOnDestroy(): void {
    if (this.rotationInterval) {
      clearInterval(this.rotationInterval);
    }
  }
}
