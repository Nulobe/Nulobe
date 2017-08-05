export interface IAuthService {
  login(): void;
  onLoginCallback(): void;
  logout(): void;
  isAuthenticated(): boolean;
  getIdToken(): string;
}

