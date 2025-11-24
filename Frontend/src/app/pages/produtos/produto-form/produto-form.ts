import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Produto } from '../../../model/produto.model';
import { ProdutoService } from '../../../service/produtos/produto.service';
import { Fornecedor } from '../../../model/fornecedor.model';
import { FornecedorService } from '../../../service/fornecedor/fornecedor';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './produto-form.html',
  styleUrls: ['./produto-form.scss'],
})
export class ProdutoFormComponent implements OnInit {
  private readonly produtoService = inject(ProdutoService);
  private readonly fornecedorService = inject(FornecedorService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  produto: Partial<Produto> = { nome: '', preco: 0, descricao: '', quantidadeEstoque: 0, ativo: true };
  fornecedores: Fornecedor[] = [];
  isEdit = false;
  titulo = 'Novo Produto';

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
    if (this.isEdit && this.produto.id) {
      this.produtoService.update(this.produto.id, this.produto).subscribe(() => {
        this.router.navigate(['/produtos']);
      });
    } else {
      this.produtoService.create(this.produto).subscribe(() => {
        this.router.navigate(['/produtos']);
      });
    }
  }
}
