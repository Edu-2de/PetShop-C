import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ProdutoService } from '../../service/produtos/produto.service';
import { Produto } from '../../model/produto.model';
import { AuthService } from '../../service/auth/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
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

  banners: string[] = [];
  currentBanner = 0;
  rotationInterval: any;
  rotationDelay = 8000;
  paused = false;
  bannersLoaded = false;

  produtos: Produto[] = [];
  produtosCarregando = true;
  produtosErro = false;

  constructor(
    private router: Router,
    private produtoService: ProdutoService,
    public authService: AuthService
  ) {
    this.loadAvailableBanners();
    this.loadProdutos();
  }

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  ngAfterViewInit(): void {
    this.initScrollAnimations();
    this.handleHeaderShadowOnScroll();
  }

  loadAvailableBanners(): void {
    // Carregar banners dinamicamente (Angular usa /assets/ no caminho)
    const maxBanners = 10;
    let checkedCount = 0;

    console.log('🔍 Iniciando busca por banners...');

    for (let i = 1; i <= maxBanners; i++) {
      const src = `/assets/images/carousel/banner${i}.jpg`;
      const img = new Image();
      
      img.onload = () => {
        if (!this.banners.includes(src)) {
          this.banners.push(src);
          console.log(`✅ Banner ${i} encontrado:`, src);
          
          // Se for o primeiro banner carregado, iniciar rotação
          if (this.banners.length === 1 && !this.bannersLoaded) {
            this.bannersLoaded = true;
            setTimeout(() => this.initBannerRotation(), 100);
          }
        }
        checkedCount++;
        if (checkedCount === maxBanners) {
          this.bannersLoaded = true;
          console.log(`📊 Busca finalizada. Total de banners encontrados: ${this.banners.length}`);
        }
      };
      
      img.onerror = () => {
        // Banner não existe, continua verificando
        console.log(`❌ Banner ${i} não encontrado:`, src);
        checkedCount++;
        if (checkedCount === maxBanners) {
          this.bannersLoaded = true;
          console.log(`📊 Busca finalizada. Total de banners encontrados: ${this.banners.length}`);
        }
      };
      
      img.src = src;
    }
  }

  loadProdutos(): void {
    this.produtoService.findAll().subscribe({
      next: (produtos) => {
        // Filtrar apenas produtos ativos e pegar os primeiros 8
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
    // Se a rota requer autenticação e o usuário não está logado, redireciona para login
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
    // Placeholder SVG inline para produtos sem imagem
    return 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 400 400"%3E%3Crect fill="%23f0f0f0" width="400" height="400"/%3E%3Ctext fill="%23999" font-family="sans-serif" font-size="40" x="50%25" y="50%25" text-anchor="middle" dy=".3em"%3ESem imagem%3C/text%3E%3C/svg%3E';
  }

  formatPreco(preco: number): string {
    return preco.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  verProduto(produto: Produto): void {
    this.router.navigate(['/produtos', produto.produtoId]);
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
