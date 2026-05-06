import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { VeiculoService } from '../../services/veiculo.service';
import { AuthService } from '../../services/auth.service';
import { Veiculo, VeiculoDTO } from '../../models/veiculo.model';

@Component({
  selector: 'app-veiculos',
  standalone: false,
  templateUrl: './veiculos.component.html',
  styleUrls: ['./veiculos.component.scss']
})
export class VeiculosComponent implements OnInit {
  veiculos: Veiculo[] = [];
  carregando = false;
  erro: string = '';
  sucesso: string = '';
  modoEdicao = false;
  idEditando: number | null = null;
  isAdm = false;

  form: VeiculoDTO = { nome: '', marca: '', ano: new Date().getFullYear() };

  constructor(
    private veiculoService: VeiculoService,
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.isAdm = this.authService.isPerfil('Adm');
    this.carregar();
  }

  carregar(): void {
    this.carregando = true;
    this.veiculoService.listar().subscribe({
      next: (data) => {
        this.veiculos = data;
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.erro = 'Erro ao carregar veículos.';
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }

  salvar(): void {
    if (this.modoEdicao && this.idEditando !== null) {
      this.veiculoService.atualizar(this.idEditando, this.form).subscribe({
        next: () => {
          this.sucesso = 'Veículo atualizado!';
          this.resetForm();
          this.carregar();
        },
        error: () => {
          this.erro = 'Erro ao atualizar.';
          this.cdr.detectChanges();
        }
      });
    } else {
      this.veiculoService.criar(this.form).subscribe({
        next: () => {
          this.sucesso = 'Veículo criado!';
          this.resetForm();
          this.carregar();
        },
        error: () => {
          this.erro = 'Erro ao criar veículo.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  editar(v: Veiculo): void {
    this.modoEdicao = true;
    this.idEditando = v.id;
    this.form = { nome: v.nome, marca: v.marca, ano: v.ano };
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  deletar(id: number): void {
    if (!confirm('Deseja excluir este veículo?')) return;
    this.veiculoService.deletar(id).subscribe({
      next: () => {
        this.sucesso = 'Veículo excluído!';
        this.carregar();
      },
      error: () => {
        this.erro = 'Erro ao excluir.';
        this.cdr.detectChanges();
      }
    });
  }

  resetForm(): void {
    this.form = { nome: '', marca: '', ano: new Date().getFullYear() };
    this.modoEdicao = false;
    this.idEditando = null;
    this.erro = '';
    setTimeout(() => {
      this.sucesso = '';
      this.cdr.detectChanges();
    }, 3000);
  }

  logout(): void {
    this.authService.logout();
  }
}