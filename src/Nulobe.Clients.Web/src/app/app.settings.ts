export interface AuthSettings {
  clientId: string;
  domain: string;
}

export interface AuthSettingsCollection {
  auth0: AuthSettings;
  quizlet: AuthSettings;
}

export interface CountryData {
  name: string,
  displayName: string
}

export interface EnvironmentSettings {
  baseUrl: string;
  apiBaseUrl: string;
  auth: AuthSettingsCollection;
  countries: CountryData[];
}

export const NULOBE_ENV: string = window["NULOBE_ENV"];
export const NULOBE_ENV_SETTINGS: EnvironmentSettings = window["NULOBE_ENV_SETTINGS"];