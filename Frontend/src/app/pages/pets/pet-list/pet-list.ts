import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Pet } from '../../../model/pet.model';
import { PetService } from '../../../service/pets/pet.service';
import { Tutor } from '../../../model/tutor.model';
import { TutorService } from '../../../service/tutores/tutor.service';
import { FormsModule } from '@angular/forms';

interface PetComTutor extends Pet {
  tutor?: Tutor;
}

@Component({
  selector: 'app-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, DatePipe],
  templateUrl: './pet-list.html',
  styleUrls: ['./pet-list.scss']
})
export class PetListComponent implements OnInit {
  private pets = signal<PetComTutor[]>([]);
  termoBusca = signal<string>('');

  petsFiltrados = computed(() => {
    const pets = this.pets();
    const termo = this.termoBusca().toLowerCase();
    if (!termo) {
      return pets;
    }
    return pets.filter(pet =>
      pet.nome.toLowerCase().includes(termo) ||
      (pet.tutor && pet.tutor.nome.toLowerCase().includes(termo))
    );
  });

  constructor(
    private petService: PetService,
    private tutorService: TutorService
  ) {}

  ngOnInit(): void {
    this.carregarPetsComTutores();
  }

  carregarPetsComTutores(): void {
    forkJoin({
      pets: this.petService.listar(),
      tutores: this.tutorService.listar()
    }).subscribe(({ pets, tutores }) => {
      const tutoresMap = new Map(tutores.map(t => [t.id, t]));
      const petsComTutor: PetComTutor[] = pets.map(pet => ({
        ...pet,
        tutor: tutoresMap.get(pet.tutorId)
      }));
      this.pets.set(petsComTutor);
    });
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  excluir(id: number | undefined): void {
    if (id === undefined) return;
    if (confirm('Deseja realmente excluir este pet?')) {
      this.petService.deletar(id).subscribe(() => {
        this.pets.update(petsAtuais => petsAtuais.filter(p => p.id !== id));
      });
    }
  }
}
