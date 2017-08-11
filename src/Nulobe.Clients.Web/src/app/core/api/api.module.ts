import { NgModule, ModuleWithProviders, Provider, ValueProvider } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FactApiClient, TagApiClient, FlagApiClient, VoteApiClient, QuizletApiClient, API_BASE_URL } from './api.swagger';

import { NULOBE_ENV_SETTINGS } from '../../../environments/environment';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    FactApiClient,
    TagApiClient,
    FlagApiClient,
    VoteApiClient,
    QuizletApiClient,
    
    <ValueProvider>{ provide: API_BASE_URL, useValue: NULOBE_ENV_SETTINGS.apiBaseUrl }
  ]
})
export class ApiModule {}
