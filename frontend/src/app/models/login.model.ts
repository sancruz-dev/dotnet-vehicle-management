export interface LoginDTO {
  email: string;
  senha: string;
}

export interface AdminLogado {
  email: string;
  perfil: string;
  token: string;
}