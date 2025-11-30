import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { ProdutoService } from '../../service/produtos/produto.service';
import { Produto } from '../../model/produto.model';
import { CategoriaService } from '../../service/categorias/categoria.service'; // Importar
import { Categoria } from '../../model/categoria.model'; // Importar

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
  private categoriaService = inject(CategoriaService); // Injetar

  produtosDestaque = signal<Produto[]>([]);
  categoriasDestaque = signal<Categoria[]>([]); // Signal para categorias
  carregandoProdutos = signal<boolean>(true);

  // LISTA EXPANDIDA COM TEXTOS MAIS COMPLETOS
  features = [
    {
      title: 'Corpo Clínico de Elite',
      desc: 'Nossa equipe é formada por especialistas em diversas áreas, incluindo cardiologia, dermatologia e cirurgia, garantindo que seu pet receba o diagnóstico mais preciso e o tratamento mais eficaz disponível na medicina veterinária.',
      icon: 'bi-person-badge-fill'
    },
    {
      title: 'Tecnologia Diagnóstica',
      desc: 'Investimos nos equipamentos mais modernos de imagem e laboratório. Realizamos raio-x digital, ultrassom e exames de sangue na hora, proporcionando agilidade que pode salvar vidas em situações de emergência.',
      icon: 'bi-cpu-fill'
    },
    {
      title: 'Ambiente Stress-Free',
      desc: 'Sabemos que ir ao veterinário pode dar medo. Nossa clínica foi projetada com acústica suave, feromônios calmantes e áreas separadas para cães e gatos, reduzindo a ansiedade e tornando a visita mais tranquila.',
      icon: 'bi-heart-pulse-fill'
    },
    {
      title: 'Farmácia Completa',
      desc: 'Conveniência total para você. Saia da consulta já com a medicação necessária em mãos. Trabalhamos apenas com os melhores laboratórios e garantimos o armazenamento correto de vacinas e remédios.',
      icon: 'bi-capsule'
    },
    {
      title: 'Estética e Spa',
      desc: 'Muito mais que um banho. Oferecemos hidratação profunda, tosa especializada por raça e tratamentos dermatológicos com produtos hipoalergênicos de primeira linha, tudo com muito carinho.',
      icon: 'bi-scissors'
    },
    {
      title: 'Plantão 24 Horas',
      desc: 'Imprevistos não têm hora marcada. Estamos de portas abertas dia e noite, feriados e finais de semana, com veterinário e cirurgião de plantão para acolher seu melhor amigo quando ele mais precisar.',
      icon: 'bi-clock-history'
    }
  ];

  ngOnInit(): void {
    this.carregarDestaques();
    this.carregarCategorias();
  }

  carregarDestaques() {
    this.produtoService.listar().subscribe({
      next: (produtos) => {
        const recentes = produtos.filter(p => p.ativo).slice(-4).reverse();
        this.produtosDestaque.set(recentes);
        this.carregandoProdutos.set(false);
      },
      error: (e) => {
        this.carregandoProdutos.set(false);
      }
    });
  }

  // Busca as categorias para os Badges
  carregarCategorias() {
    this.categoriaService.listar().subscribe(data => {
      // Pega apenas as 4 primeiras para não poluir a home
      this.categoriasDestaque.set(data.slice(0, 4));
    });
  }

  // --- Helpers ---
  codificarId(id: number | undefined): string {
    return id ? btoa(id.toString()) : '';
  }

  getImagemUrl(produto: Produto): string {
    if (!produto.imagens || produto.imagens.length === 0) return 'assets/images/no-image.png';
    const url = produto.imagens[0].url;
    if (url.startsWith('http') || url.startsWith('assets')) return url;
    return `http://localhost:5000/${url}`;
  }

  handleImageError(event: any) {
    event.target.src = 'assets/images/no-image.png';
  }
}
