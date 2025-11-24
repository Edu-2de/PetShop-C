import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ServicoPet } from '../../../model/servico-pet.model';
import { ServicoPetService } from '../../../service/servico-pet/servico-pet';

@Component({
  selector: 'app-servico-pet-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './servicos-pet.form.html',
  styleUrls: ['./servicos-pet.form.scss']
})
export class ServicoPetFormComponent implements OnInit {
  servico: Partial<ServicoPet> = { nome: '', preco: 0, descricao: '', ativo: true };
  isEdit = false;
  titulo = 'Novo Serviço';

  constructor(
    private servicoPetService: ServicoPetService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.titulo = 'Editar Serviço';
      this.servicoPetService.buscarPorId(Number(id)).subscribe(servico => {
        this.servico = servico;
      });
    }
  }

  salvar(): void {
    if (this.isEdit && this.servico.id) {
      this.servicoPetService.atualizar(this.servico.id, this.servico).subscribe(() => {
        this.router.navigate(['/servicos']);
      });
    } else {
      this.servicoPetService.criar(this.servico).subscribe(() => {
        this.router.navigate(['/servicos']);
      });
    }
  }
}
