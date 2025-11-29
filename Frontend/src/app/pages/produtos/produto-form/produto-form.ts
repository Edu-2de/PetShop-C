import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Produto } from '../../../model/produto.model';
import { ProdutoService } from '../../../service/produtos/produto.service';
import { ProdutoImagemService } from '../../../service/produto-imagem/produto-imagem.service';
import { Fornecedor } from '../../../model/fornecedor.model';
// CORREÇÃO: Pasta 'fornecedor', arquivo 'forncedor'
import { FornecedorService } from '../../../service/fornecedor/fornecedor';
import { Categoria } from '../../../model/categoria.model';
import { CategoriaService } from '../../../service/categorias/categoria.service';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './produto-form.html',
  styleUrls: ['./produto-form.scss']
})
export class ProdutoFormComponent implements OnInit {
  private readonly produtoService = inject(ProdutoService);
  private readonly produtoImagemService = inject(ProdutoImagemService);
  private readonly fornecedorService = inject(FornecedorService);
  private readonly categoriaService = inject(CategoriaService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  produto: Partial<Produto> = {
    nome: '',
    preco: 0,
    descricao: '',
    quantidadeEstoque: 0,
    ativo: true,
    imagens: []
  };
  fornecedores: Fornecedor[] = [];
  categorias: Categoria[] = [];
  isEdit = false;
  titulo = 'Novo Produto';
  uploading = false;

  ngOnInit(): void {
    this.fornecedorService.listar().subscribe((data: Fornecedor[]) => this.fornecedores = data);
    this.categoriaService.listar().subscribe((data: Categoria[]) => this.categorias = data);

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Produto';
      this.produtoService.findById(Number(id)).subscribe((produto: Produto) => {
        this.produto = produto;
      });
    }
  }

  salvar(): void {
    if (this.isEdit && this.produto.produtoId) {
      this.produtoService.update(this.produto.produtoId, this.produto).subscribe(() => {
        this.router.navigate(['/produtos']);
      });
    } else {
      this.produtoService.create(this.produto).subscribe(() => {
        this.router.navigate(['/produtos']);
      });
    }
  }

  onFileSelect(event: any): void {
    const files = event.target.files;
    if (!files || files.length === 0 || !this.produto.produtoId) return;

    this.uploading = true;
    let uploadedCount = 0;

    for (let file of files) {
      if (file.size > 5 * 1024 * 1024) {
        alert(`Arquivo ${file.name} é muito grande. Máximo 5MB por imagem.`);
        continue;
      }

      this.produtoImagemService.uploadImagem(this.produto.produtoId!, file).subscribe({
        next: (imagem) => {
          if (!this.produto.imagens) {
            this.produto.imagens = [];
          }
          this.produto.imagens.push(imagem);
          uploadedCount++;
          if (uploadedCount === files.length) {
            this.uploading = false;
            event.target.value = '';
          }
        },
        error: (error) => {
          console.error('Erro ao fazer upload:', error);
          alert(`Erro ao fazer upload da imagem ${file.name}`);
          uploadedCount++;
          if (uploadedCount === files.length) {
            this.uploading = false;
            event.target.value = '';
          }
        }
      });
    }
  }

  deleteImagem(imagemId: number): void {
    if (confirm('Deseja realmente excluir esta imagem?')) {
      this.produtoImagemService.deleteImagem(imagemId).subscribe({
        next: () => {
          if (this.produto.imagens) {
            this.produto.imagens = this.produto.imagens.filter(img => img.id !== imagemId);
          }
        },
        error: (error) => {
          console.error('Erro ao deletar imagem:', error);
          alert('Erro ao excluir imagem');
        }
      });
    }
  }
}
