import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ServicoPet } from '../../model/servico-pet.model';
import { ServicoPetService } from '../../service/servico-pet/servico-pet';
import { AuthService } from '../../service/auth/auth.service';

@Component({
  selector: 'app-servico-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, CurrencyPipe],
  templateUrl: './servicos-pet.html',
  styleUrls: ['./servicos-pet.scss']
})
export class ServicoPetListComponent implements OnInit {
  private servicos = signal<ServicoPet[]>([]);
  termoBusca = signal<string>('');

  public authService = inject(AuthService);
  private router = inject(Router);

  servicosFiltrados = computed(() => {
    const servicos = this.servicos();
    const termo = this.termoBusca().toLowerCase();

    if (!termo) {
      return servicos;
    }

    return servicos.filter(servico =>
      servico.nome.toLowerCase().includes(termo)
    );
  });

  constructor(private servicoPetService: ServicoPetService) { }

  ngOnInit(): void {
    this.carregarServicos();
  }

  carregarServicos(): void {
    this.servicoPetService.listar().subscribe(data => {
      // Se for usuário, mostra apenas ativos. Se for admin, mostra todos.
      const lista = this.authService.isAdmin() ? data : data.filter(s => s.ativo);
      this.servicos.set(lista);
    });
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  excluir(id: number | undefined): void {
    if (id === undefined) return;
    if (confirm('Deseja realmente excluir este serviço?')) {
      this.servicoPetService.deletar(id).subscribe(() => {
        this.servicos.update(servicosAtuais => servicosAtuais.filter(s => s.id !== id));
      });
    }
  }

  agendar(servico: ServicoPet): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    // Redireciona para o formulário de agendamento (você pode passar o ID do serviço via queryParams se quiser pré-selecionar)
    this.router.navigate(['/agenda/novo'], { queryParams: { servicoId: servico.id } });
  }
}
