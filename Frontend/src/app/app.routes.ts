import { Routes } from '@angular/router';
import { TutorListComponent } from './pages/tutores/tutor-list/tutor-list';
import { TutorFormComponent } from './pages/tutores/tutor-form/tutor-form';
import { PetListComponent } from './pages/pets/pet-list/pet-list';
import { PetFormComponent } from './pages/pets/pet-form/pet-form';
import { ProdutoListComponent } from './pages/produtos/produto-list';
import { ProdutoFormComponent } from './pages/produtos/produto-form/produto-form';
import { ServicoPetListComponent } from './pages/servicos-pet/servicos-pet';
import { ServicoPetFormComponent } from './pages/servicos-pet/servicos-pet.form/servicos-pet.form';
import { AgendaListComponent } from './pages/agenda-list/agenda-list';
import { AgendaFormComponent } from './pages/agenda-form/agenda-form';
import { ForncedorListComponent } from './pages/forncedor/forncedor-list/forncedor-list';
import { ForncedorFormComponent } from './pages/forncedor/forncedor-form/forncedor-form';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoginComponent } from './pages/login/login.component';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard.component';
import { authGuard, adminGuard } from './guards/auth.guard';

export const routes: Routes = [
    // Rotas públicas
    { path: '', component: DashboardComponent },
    { path: 'login', component: LoginComponent },
    
    // Rotas que requerem autenticação (qualquer usuário logado)
    { 
        path: 'agenda/novo', 
        component: AgendaFormComponent,
        canActivate: [authGuard]
    },
    
    // Rotas administrativas (apenas admin)
    { 
        path: 'admin', 
        component: AdminDashboardComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'tutores', 
        component: TutorListComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'tutores/novo', 
        component: TutorFormComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'tutores/editar/:id', 
        component: TutorFormComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'pets', 
        component: PetListComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'pets/novo', 
        component: PetFormComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'pets/editar/:id', 
        component: PetFormComponent,
        canActivate: [adminGuard]
    },
    
    // Rotas de produtos e serviços (públicas para visualização)
    { path: 'produtos', component: ProdutoListComponent },
    { 
        path: 'produtos/novo', 
        component: ProdutoFormComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'produtos/editar/:id', 
        component: ProdutoFormComponent,
        canActivate: [adminGuard]
    },
    
    { path: 'servicos', component: ServicoPetListComponent },
    { 
        path: 'servicos/novo', 
        component: ServicoPetFormComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'servicos/editar/:id', 
        component: ServicoPetFormComponent,
        canActivate: [adminGuard]
    },
    
    // Rotas de agenda (admin apenas)
    { 
        path: 'agenda', 
        component: AgendaListComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'agenda/editar/:id', 
        component: AgendaFormComponent,
        canActivate: [adminGuard]
    },
    
    // Rotas de fornecedores (admin apenas)
    { 
        path: 'fornecedores', 
        component: ForncedorListComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'fornecedores/novo', 
        component: ForncedorFormComponent,
        canActivate: [adminGuard]
    },
    { 
        path: 'fornecedores/editar/:id', 
        component: ForncedorFormComponent,
        canActivate: [adminGuard]
    },
];
