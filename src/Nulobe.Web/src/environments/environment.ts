// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false
};

export interface NulobeEnvironment {
  ENVIRONMENT: string;
  API_BASE_URL: string;
  AUTH_CLIENT_ID: string;
  AUTH_DOMAIN: string;
}

export const NULOBE_ENV: NulobeEnvironment = window["NULOBE_ENV"];