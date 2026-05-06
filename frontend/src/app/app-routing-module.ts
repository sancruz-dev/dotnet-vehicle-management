import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { VeiculosComponent } from './pages/veiculos/veiculos.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'veiculos', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'veiculos', component: VeiculosComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'veiculos' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}