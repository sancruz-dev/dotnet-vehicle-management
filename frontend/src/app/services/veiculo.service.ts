import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Veiculo, VeiculoDTO } from '../models/veiculo.model';

@Injectable({ providedIn: 'root' })
export class VeiculoService {
  private readonly url = `${environment.apiUrl}/Veiculos`;

  constructor(private http: HttpClient) {}

  listar(pagina: number = 1): Observable<Veiculo[]> {
    return this.http.get<Veiculo[]>(`${this.url}/veiculos?pagina=${pagina}`);
  }

  buscarPorId(id: number): Observable<Veiculo> {
    return this.http.get<Veiculo>(`${this.url}/veiculo/${id}`);
  }

  criar(veiculo: VeiculoDTO): Observable<Veiculo> {
    return this.http.post<Veiculo>(`${this.url}/veiculo`, veiculo);
  }

  atualizar(id: number, veiculo: VeiculoDTO): Observable<Veiculo> {
    return this.http.put<Veiculo>(`${this.url}/veiculo/${id}`, veiculo);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/veiculo/${id}`);
  }
}