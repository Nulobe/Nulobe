// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false
};

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