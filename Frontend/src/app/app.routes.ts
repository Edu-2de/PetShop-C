import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoginComponent } from './pages/login/login.component';
import { ProdutoListComponent } from './pages/produtos/produto-list';
import { ProdutoFormComponent } from './pages/produtos/produto-form/produto-form';
import { ServicoPetListComponent } from './pages/servicos-pet/servicos-pet';
import { ServicoPetFormComponent } from './pages/servicos-pet/servicos-pet.form/servicos-pet.form';
import { TutorListComponent } from './pages/tutores/tutor-list/tutor-list';
import { TutorFormComponent } from './pages/tutores/tutor-form/tutor-form';
import { PetListComponent } from './pages/pets/pet-list/pet-list';
import { PetFormComponent } from './pages/pets/pet-form/pet-form';
import { AgendaListComponent } from './pages/agenda-list/agenda-list';
import { AgendaFormComponent } from './pages/agenda-form/agenda-form';
// Correção: Nomes das classes conforme estão nos arquivos (com erro de digitação "Forncedor")
import { ForncedorListComponent } from './pages/forncedor/forncedor-list/forncedor-list';
import { ForncedorFormComponent } from './pages/forncedor/forncedor-form/forncedor-form';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard.component';
// Correção: authGuard (minúsculo) conforme exportado no arquivo
import { authGuard } from './guards/auth.guard';
import { FornecedorListComponent } from './pages/fornecedor/fornecedor-list/fornecedor-list';
import { FornecedorFormComponent } from './pages/fornecedor/fornecedor-form/fornecedor-form';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },

  // Rotas Protegidas (Admin)
  { path: 'admin', component: AdminDashboardComponent, canActivate: [authGuard] },

  // Produtos
  { path: 'produtos', component: ProdutoListComponent },
  { path: 'produtos/novo', component: ProdutoFormComponent, canActivate: [authGuard] },
  { path: 'produtos/editar/:id', component: ProdutoFormComponent, canActivate: [authGuard] },

  // Serviços
  { path: 'servicos', component: ServicoPetListComponent },
  { path: 'servicos/novo', component: ServicoPetFormComponent, canActivate: [authGuard] },
  { path: 'servicos/editar/:id', component: ServicoPetFormComponent, canActivate: [authGuard] },

  // Tutores
  { path: 'tutores', component: TutorListComponent, canActivate: [authGuard] },
  { path: 'tutores/novo', component: TutorFormComponent, canActivate: [authGuard] },
  { path: 'tutores/editar/:id', component: TutorFormComponent, canActivate: [authGuard] },

  // Pets
  { path: 'pets', component: PetListComponent, canActivate: [authGuard] },
  { path: 'pets/novo', component: PetFormComponent, canActivate: [authGuard] },
  { path: 'pets/editar/:id', component: PetFormComponent, canActivate: [authGuard] },

  // Agenda
  { path: 'agenda', component: AgendaListComponent, canActivate: [authGuard] },
  { path: 'agenda/novo', component: AgendaFormComponent },
  { path: 'agenda/editar/:id', component: AgendaFormComponent, canActivate: [authGuard] },

  // Fornecedores (Usando os nomes corrigidos das classes)
  { path: 'fornecedores', component: ForncedorListComponent, canActivate: [authGuard] },
  { path: 'fornecedores/novo', component: ForncedorFormComponent, canActivate: [authGuard] },
  { path: 'fornecedores/editar/:id', component: ForncedorFormComponent, canActivate: [authGuard] },

  { path: 'fornecedores', component: FornecedorListComponent, canActivate: [authGuard] },

  { path: '**', redirectTo: '' }
];
