import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Produto, ProdutoImagem } from '../../../model/produto.model';
import { ProdutoService } from '../../../service/produtos/produto.service';
import { ProdutoImagemService } from '../../../service/produto-imagem/produto-imagem.service';
import { Fornecedor } from '../../../model/fornecedor.model';
import { FornecedorService } from '../../../service/fornecedor/fornecedor';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="card shadow">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">
                <i class="fas fa-box me-2"></i>
                {{ titulo }}
              </h4>
            </div>
            <div class="card-body">
              <form (ngSubmit)="salvar()" #produtoForm="ngForm">
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="nome" class="form-label">Nome *</label>
                    <input
                      type="text"
                      class="form-control"
                      id="nome"
                      name="nome"
                      [(ngModel)]="produto.nome"
                      required
                      #nome="ngModel"
                    />
                    <div class="invalid-feedback" *ngIf="nome.touched && nome.errors?.['required']">
                      Nome é obrigatório
                    </div>
                  </div>
                  
                  <div class="col-md-6 mb-3">
                    <label for="preco" class="form-label">Preço *</label>
                    <div class="input-group">
                      <span class="input-group-text">R$</span>
                      <input
                        type="number"
                        class="form-control"
                        id="preco"
                        name="preco"
                        [(ngModel)]="produto.preco"
                        step="0.01"
                        min="0"
                        required
                        #preco="ngModel"
                      />
                    </div>
                    <div class="invalid-feedback" *ngIf="preco.touched && preco.errors?.['required']">
                      Preço é obrigatório
                    </div>
                  </div>
                </div>

                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="quantidadeEstoque" class="form-label">Quantidade em Estoque *</label>
                    <input
                      type="number"
                      class="form-control"
                      id="quantidadeEstoque"
                      name="quantidadeEstoque"
                      [(ngModel)]="produto.quantidadeEstoque"
                      min="0"
                      required
                      #quantidade="ngModel"
                    />
                    <div class="invalid-feedback" *ngIf="quantidade.touched && quantidade.errors?.['required']">
                      Quantidade é obrigatória
                    </div>
                  </div>
                  
                  <div class="col-md-6 mb-3">
                    <label for="fornecedor" class="form-label">Fornecedor</label>
                    <select
                      class="form-select"
                      id="fornecedor"
                      name="fornecedor"
                      [(ngModel)]="produto.fornecedorId"
                    >
                      <option value="">Selecione um fornecedor</option>
                      <option *ngFor="let fornecedor of fornecedores" [value]="fornecedor.fornecedorId">
                        {{ fornecedor.nome }}
                      </option>
                    </select>
                  </div>
                </div>

                <div class="mb-3">
                  <label for="descricao" class="form-label">Descrição</label>
                  <textarea
                    class="form-control"
                    id="descricao"
                    name="descricao"
                    rows="3"
                    [(ngModel)]="produto.descricao"
                  ></textarea>
                </div>

                <div class="mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="checkbox"
                      id="ativo"
                      name="ativo"
                      [(ngModel)]="produto.ativo"
                    />
                    <label class="form-check-label" for="ativo">
                      Produto ativo
                    </label>
                  </div>
                </div>

                <!-- Upload de Imagens -->
                <div class="mb-4" *ngIf="isEdit && produto.produtoId">
                  <label class="form-label">Imagens do Produto</label>
                  
                  <!-- Upload Area -->
                  <div class="border rounded p-3 mb-3 text-center bg-light">
                    <i class="fas fa-cloud-upload-alt fa-2x text-muted mb-2"></i>
                    <p class="text-muted mb-2">Arraste imagens aqui ou clique para selecionar</p>
                    <input
                      type="file"
                      class="form-control"
                      accept="image/*"
                      multiple
                      (change)="onFileSelect($event)"
                      #fileInput
                    />
                    <small class="text-muted">Formatos aceitos: JPG, PNG, GIF (máx. 5MB por imagem)</small>
                  </div>

                  <!-- Loading -->
                  <div class="text-center mb-3" *ngIf="uploading">
                    <div class="spinner-border text-primary" role="status">
                      <span class="visually-hidden">Enviando...</span>
                    </div>
                    <p class="text-muted mt-2">Enviando imagens...</p>
                  </div>

                  <!-- Imagens Existentes -->
                  <div class="row" *ngIf="produto.imagens && produto.imagens.length > 0">
                    <div class="col-md-3 mb-3" *ngFor="let imagem of produto.imagens">
                      <div class="card">
                        <img [src]="imagem.url" class="card-img-top" style="height: 150px; object-fit: cover;">
                        <div class="card-body p-2">
                          <button
                            type="button"
                            class="btn btn-danger btn-sm w-100"
                            (click)="deleteImagem(imagem.id)"
                          >
                            <i class="fas fa-trash"></i> Excluir
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>

                  <div class="alert alert-info" *ngIf="!produto.imagens || produto.imagens.length === 0">
                    <i class="fas fa-info-circle me-2"></i>
                    Nenhuma imagem cadastrada. Adicione imagens para melhorar a apresentação do produto.
                  </div>
                </div>

                <div class="alert alert-warning" *ngIf="!isEdit">
                  <i class="fas fa-info-circle me-2"></i>
                  <strong>Dica:</strong> Após salvar o produto, você poderá adicionar imagens editando-o novamente.
                </div>

                <div class="d-flex gap-2">
                  <button
                    type="submit"
                    class="btn btn-success"
                    [disabled]="!produtoForm.form.valid"
                  >
                    <i class="fas fa-save me-2"></i>
                    Salvar
                  </button>
                  <a routerLink="/produtos" class="btn btn-secondary">
                    <i class="fas fa-arrow-left me-2"></i>
                    Voltar
                  </a>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .card {
      border: none;
      border-radius: 15px;
    }

    .card-header {
      border-radius: 15px 15px 0 0 !important;
    }

    .form-control:focus,
    .form-select:focus {
      border-color: #0d6efd;
      box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
    }

    .btn {
      border-radius: 8px;
    }

    .upload-area {
      border: 2px dashed #dee2e6;
      transition: border-color 0.2s ease;
    }

    .upload-area:hover {
      border-color: #0d6efd;
    }

    .image-preview {
      max-height: 150px;
      object-fit: cover;
    }
  `]
})
export class ProdutoFormComponent implements OnInit {
  private readonly produtoService = inject(ProdutoService);
  private readonly produtoImagemService = inject(ProdutoImagemService);
  private readonly fornecedorService = inject(FornecedorService);
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
  isEdit = false;
  titulo = 'Novo Produto';
  uploading = false;

  ngOnInit(): void {
    this.fornecedorService.listar().subscribe((data) => this.fornecedores = data);
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Produto';
      this.produtoService.findById(Number(id)).subscribe((produto) => {
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
      this.produtoService.create(this.produto).subscribe((novoProduto) => {
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
      if (file.size > 5 * 1024 * 1024) { // 5MB
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
            event.target.value = ''; // Limpar input
          }
        },
        error: (error) => {
          console.error('Erro ao fazer upload:', error);
          alert(`Erro ao fazer upload da imagem ${file.name}`);
          uploadedCount++;
          
          if (uploadedCount === files.length) {
            this.uploading = false;
            event.target.value = ''; // Limpar input
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
