import { Injectable, FactoryProvider } from "@angular/core";
import { ConnectionBackend, RequestOptions, Request, RequestOptionsArgs, Response, Http, Headers, XHRBackend } from "@angular/http";
import { Observable } from "rxjs/Rx";
import { NULOBE_ENV_SETTINGS } from '../../../environments/environment';
import { Auth0AuthService } from './auth.service';

export function authHttpFactory(xhrBackend: XHRBackend, requestOptions: RequestOptions, authService: Auth0AuthService): Http {
  return new AuthHttp(xhrBackend, requestOptions, authService);
}

export const authHttpProvider = <FactoryProvider>{
    provide: Http,
    useFactory: authHttpFactory,
    deps: [XHRBackend, RequestOptions, Auth0AuthService]
};

@Injectable()
export class AuthHttp extends Http {
    
    constructor(
        private backend: ConnectionBackend,
        private defaultOptions: RequestOptions,
        private authService: Auth0AuthService
    ) {
        super(backend, defaultOptions);
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        let urlString = <string>url;
        if (typeof(url) === 'object') {
            urlString = url.url
        }
        return super.request(url, this.getRequestOptionArgs(urlString, options))
            .map(r => {
              if (r.status === 201) {
                r.status = 200;
              }
              return r;
            });
    }

    get(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return super.get(url, this.getRequestOptionArgs(url, options))
            .map(r => {
                if (r.status === 201) {
                    r.status = 200;
                }
                return r;
                });
    }

    post(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
        return super.post(url, body, this.getRequestOptionArgs(url, options))
            .map(r => {
              if (r.status === 201) {
                r.status = 200;
              }
              return r;
            });
    }

    put(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
        return super.put(url, body, this.getRequestOptionArgs(url, options))
            .map(r => {
              if (r.status === 201) {
                r.status = 200;
              }
              return r;
            });
    }

    delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return super.delete(url, this.getRequestOptionArgs(url, options))
            .map(r => {
              if (r.status === 201) {
                r.status = 200;
              }
              return r;
            });
    }

    private getRequestOptionArgs(url: string, options?: RequestOptionsArgs) : RequestOptionsArgs {
        if (!options) {
            options = {};
        }

        if (!options.headers) {
            options.headers = new Headers();
        }

        if (url.startsWith(NULOBE_ENV_SETTINGS.apiBaseUrl) && this.authService.isAuthenticated()) {
            options.headers.append(
                'Authorization',
                `Bearer ${this.authService.getIdToken()}`)
        }

        return options;
    }
}