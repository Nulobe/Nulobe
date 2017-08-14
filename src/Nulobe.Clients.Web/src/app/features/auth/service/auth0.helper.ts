import * as auth0 from 'auth0-js';

import { NULOBE_ENV_SETTINGS } from '../../../../environments/environment';

import { AUTH_CONSTANTS } from '../auth.constants';
import { AuthConfig } from './auth-config';

export const Auth0Helper = {
  getAuth0: (config: AuthConfig, authorityName?: string, state?: string) => {
    let redirectUri = `${NULOBE_ENV_SETTINGS.baseUrl}/LOBE/callback`;
    if (authorityName && authorityName !== AUTH_CONSTANTS.AUTHORITY_NAMES.AUTH0) {
        redirectUri += `/${authorityName}`;
    }

    return new auth0.WebAuth({
      clientID: config.clientId,
      domain: config.domain,
      responseType: config.responseType,
      audience: config.audience,
      scope: config.scope,
      redirectUri,
      state
    })
  }
}