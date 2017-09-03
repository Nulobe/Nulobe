export interface AuthSettings {
  clientId: string;
  domain: string;
}

export interface AuthSettingsCollection {
  auth0: AuthSettings;
  quizlet: AuthSettings;
}

export interface EnvironmentSettings {
  baseUrl: string;
  apiBaseUrl: string;
  auth: AuthSettingsCollection;
}

export const NULOBE_ENV: string = window["NULOBE_ENV"];
export const NULOBE_ENV_SETTINGS: EnvironmentSettings = window["NULOBE_ENV_SETTINGS"];