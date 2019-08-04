import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { AuthGuard } from './Guards/auth.guard';

export const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    // runGuardsAndResolvers: 'always',
    // canActivate: [AuthGuard],
    children: [
      { path: '', component: HomeComponent }
    ]
  },
  { path: '**', redirectTo: '',  }
];
