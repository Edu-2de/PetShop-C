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
import { AdminDashboardComponent } from './pages/admin/admin-dashboard.component';
import { authGuard } from './guards/auth.guard';
import { ForncedorListComponent } from './pages/forncedor/forncedor-list/forncedor-list';
import { ForncedorFormComponent } from './pages/forncedor/forncedor-form/forncedor-form';

// Certifique-se de que estes arquivos existem (criados no passo anterior)
import { FuncionarioListComponent } from './pages/funcionarios/funcionario-list/funcionario-list';
import { FuncionarioFormComponent } from './pages/funcionarios/funcionario-form/funcionario-form';
import { CategoriaListComponent } from './pages/categorias/categoria-list/categoria-list';
import { CategoriaFormComponent } from './pages/categorias/categoria-form/categoria-form';
import { ProdutoDetailComponent } from './pages/produtos/produto-detail/produto-detail';
import { RegisterComponent } from './pages/register/register.component'; // Importar

export const routes: Routes = [
  // CORREÇÃO: pathMatch 'full' é obrigatório para rotas vazias
  { path: '', component: DashboardComponent, pathMatch: 'full' },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'cadastrar', component: RegisterComponent }, 

  // Admin
  { path: 'admin', component: AdminDashboardComponent, canActivate: [authGuard] },

  // Cadastros Básicos
  { path: 'produtos', component: ProdutoListComponent },
  { path: 'produtos/novo', component: ProdutoFormComponent, canActivate: [authGuard] },
  { path: 'produtos/editar/:id', component: ProdutoFormComponent, canActivate: [authGuard] },

  // NOVA ROTA: Detalhes do Produto (Pública)
  { path: 'produtos/:id', component: ProdutoDetailComponent },

  { path: 'servicos', component: ServicoPetListComponent },
  { path: 'servicos/novo', component: ServicoPetFormComponent, canActivate: [authGuard] },
  { path: 'servicos/editar/:id', component: ServicoPetFormComponent, canActivate: [authGuard] },

  { path: 'tutores', component: TutorListComponent, canActivate: [authGuard] },
  { path: 'tutores/novo', component: TutorFormComponent, canActivate: [authGuard] },
  { path: 'tutores/editar/:id', component: TutorFormComponent, canActivate: [authGuard] },

  { path: 'pets', component: PetListComponent, canActivate: [authGuard] },
  { path: 'pets/novo', component: PetFormComponent, canActivate: [authGuard] },
  { path: 'pets/editar/:id', component: PetFormComponent, canActivate: [authGuard] },

  { path: 'agenda', component: AgendaListComponent, canActivate: [authGuard] },
  { path: 'agenda/novo', component: AgendaFormComponent },
  { path: 'agenda/editar/:id', component: AgendaFormComponent, canActivate: [authGuard] },

  { path: 'fornecedores', component: ForncedorListComponent, canActivate: [authGuard] },
  { path: 'fornecedores/novo', component: ForncedorFormComponent, canActivate: [authGuard] },
  { path: 'fornecedores/editar/:id', component: ForncedorFormComponent, canActivate: [authGuard] },

  // Rotas Novas
  { path: 'funcionarios', component: FuncionarioListComponent, canActivate: [authGuard] },
  { path: 'funcionarios/novo', component: FuncionarioFormComponent, canActivate: [authGuard] },
  { path: 'funcionarios/editar/:id', component: FuncionarioFormComponent, canActivate: [authGuard] },

  { path: 'categorias', component: CategoriaListComponent, canActivate: [authGuard] },
  { path: 'categorias/novo', component: CategoriaFormComponent, canActivate: [authGuard] },

  { path: '**', redirectTo: '' }
];
