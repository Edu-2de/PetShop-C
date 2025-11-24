import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Fornecedor } from '../../../model/fornecedor.model';
import { FornecedorService } from '../../../service/fornecedor/fornecedor';

@Component({
  selector: 'app-fornecedor-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './forncedor-form.html',
  styleUrls: ['./forncedor-form.scss']
})
export class ForncedorFormComponent implements OnInit {
  fornecedor: Partial<Fornecedor> = { nome: '', cnpj: '', telefone: '', email: '', endereco: '' };
  isEdit = false;

  constructor(
    private fornecedorService: FornecedorService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.fornecedorService.findById(Number(id)).subscribe((data: Fornecedor) => {
        this.fornecedor = data;
      });
    }
  }

  salvar(): void {
    if (this.isEdit && this.fornecedor.id) {
      this.fornecedorService.update(this.fornecedor.id, this.fornecedor as Fornecedor).subscribe(() => {
        this.router.navigate(['/fornecedores']);
      });
    } else {
      this.fornecedorService.create(this.fornecedor as Fornecedor).subscribe(() => {
        this.router.navigate(['/fornecedores']);
      });
    }
  }
}
