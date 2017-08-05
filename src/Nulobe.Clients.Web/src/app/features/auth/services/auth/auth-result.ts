export interface AuthResult {
  accessToken: string;
  userId?: string;
  idToken?: string;
  expiresIn: number;
}