import { NgModule, ModuleWithProviders, Provider, ValueProvider } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FactApiClient, TagApiClient, QuizletApiClient, API_BASE_URL } from './api.swagger';

import { NULOBE_ENV } from '../../../environments/environment';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    FactApiClient,
    TagApiClient,
    QuizletApiClient,
    
    <ValueProvider>{ provide: API_BASE_URL, useValue: NULOBE_ENV.API_BASE_URL }
  ]
})
export class ApiModule {}
