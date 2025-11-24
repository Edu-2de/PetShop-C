import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Pet } from '../../../model/pet.model';
import { PetService } from '../../../service/pets/pet.service';
import { Tutor } from '../../../model/tutor.model';
import { TutorService } from '../../../service/tutores/tutor.service';

@Component({
  selector: 'app-pet-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './pet-form.html',
  styleUrls: ['./pet-form.scss']
})
export class PetFormComponent implements OnInit {
  pet: Partial<Pet> = { nome: '', especie: '', raca: '', dataNascimento: new Date(), tutorId: undefined };
  tutores: Tutor[] = [];
  isEdit = false;

  constructor(
    private petService: PetService,
    private tutorService: TutorService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.tutorService.listar().subscribe(tutores => this.tutores = tutores);

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.petService.buscarPorId(Number(id)).subscribe(pet => {
        this.pet = pet;
      });
    }
  }

  salvar(): void {
    if (this.isEdit && this.pet.id) {
      this.petService.atualizar(this.pet.id, this.pet).subscribe(() => {
        this.router.navigate(['/pets']);
      });
    } else {
      this.petService.criar(this.pet).subscribe(() => {
        this.router.navigate(['/pets']);
      });
    }
  }
}
