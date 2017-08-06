export interface AuthResult {
  bearerToken: string;
  expiresIn: number;
  [key: string]: any;
}