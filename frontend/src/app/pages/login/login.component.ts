import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginDTO } from '../../models/login.model';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  credentials: LoginDTO = { email: '', senha: '' };
  erro: string = '';
  carregando = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.erro = '';
    this.carregando = true;
    this.authService.login(this.credentials).subscribe({
      next: () => this.router.navigate(['/veiculos']),
      error: () => {
        this.erro = 'Email ou senha inválidos.';
        this.carregando = false;
      }
    });
  }
}