import { NgModule, ModuleWithProviders, Provider, ValueProvider } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FactApiClient, TagApiClient, API_BASE_URL } from './api.swagger';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: []
})
export class ApiModule {
  static forRoot(baseUrl: string): ModuleWithProviders {
    return {
      ngModule: ApiModule,
      providers: <Provider[]>[
        FactApiClient,
        TagApiClient,
        
        <ValueProvider>{ provide: API_BASE_URL, useValue: baseUrl }
      ]
    }
  }
}
