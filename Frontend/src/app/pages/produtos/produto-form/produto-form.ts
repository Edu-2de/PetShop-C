import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Produto } from '../../../model/produto.model';
import { ProdutoService } from '../../../service/produtos/produto.service';
import { ProdutoImagemService } from '../../../service/produto-imagem/produto-imagem.service';
import { Fornecedor } from '../../../model/fornecedor.model';
import { FornecedorService } from '../../../service/fornecedor/forncedor';
import { Categoria } from '../../../model/categoria.model';
import { CategoriaService } from '../../../service/categorias/categoria.service';
// ADICIONADO: import de catchError
import { switchMap, of, catchError } from 'rxjs';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './produto-form.html',
  styleUrls: ['./produto-form.scss']
})
export class ProdutoFormComponent implements OnInit {
  // ... (o restante das injeções e variáveis continua igual)
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
  
  selectedFile: File | null = null;
  previewUrl: string | null = null;
  uploading = false;

  ngOnInit(): void {
    this.carregarCombos();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Produto';
      this.produtoService.findById(Number(id)).subscribe((produto: Produto) => {
        this.produto = produto;
      });
    }
  }

  carregarCombos() {
    this.fornecedorService.listar().subscribe(data => this.fornecedores = data);
    this.categoriaService.listar().subscribe(data => this.categorias = data);
  }

  onFileSelect(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.previewUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  salvar(): void {
    this.uploading = true;

    // LÓGICA MELHORADA: Tratamento de erro individual para a imagem
    if (this.isEdit && this.produto.produtoId) {
      // EDIÇÃO
      this.produtoService.update(this.produto.produtoId, this.produto).pipe(
        switchMap(() => {
          if (this.selectedFile) {
            return this.produtoImagemService.uploadImagem(this.produto.produtoId!, this.selectedFile).pipe(
              catchError(err => {
                console.error('Erro no upload da imagem (Edição):', err);
                alert('Produto atualizado, mas houve um erro ao enviar a nova imagem. O servidor pode estar sem permissão de escrita.');
                return of(null); // Continua o fluxo
              })
            );
          }
          return of(null);
        })
      ).subscribe({
        next: () => this.finalizarSalvamento(),
        error: (err) => this.tratarErroGeral(err)
      });

    } else {
      // CRIAÇÃO
      this.produtoService.create(this.produto).pipe(
        switchMap((produtoCriado) => {
          // O produto JÁ FOI CRIADO aqui. Se a imagem falhar, não podemos dar erro geral.
          if (this.selectedFile && produtoCriado.id) {
            return this.produtoImagemService.uploadImagem(produtoCriado.id, this.selectedFile).pipe(
              catchError(err => {
                console.error('Erro no upload da imagem (Criação):', err);
                alert('Produto criado com sucesso! Porém, a imagem não pôde ser salva no servidor (Erro 500). Verifique a pasta de uploads na API.');
                return of(null); // Continua o fluxo para finalizar
              })
            );
          }
          return of(null);
        })
      ).subscribe({
        next: () => this.finalizarSalvamento(),
        error: (err) => this.tratarErroGeral(err)
      });
    }
  }

  private tratarErroGeral(err: any) {
    console.error(err);
    alert('Erro ao salvar os dados do produto. Verifique se todos os campos obrigatórios estão preenchidos.');
    this.uploading = false;
  }

  private finalizarSalvamento() {
    this.uploading = false;
    this.router.navigate(['/produtos']);
  }
}
