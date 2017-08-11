import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';

import { AuthModule } from '../../features/auth';
import { FactsModule } from '../../features/facts/facts.module';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminStateModule } from './state/admin-state.module';
import { CreateFactsComponent } from './pages/create-facts/create-facts.component';

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    AdminStateModule,
    AuthModule,
    FactsModule
  ],
  declarations: [AdminComponent, CreateFactsComponent]
})
export class AdminModule { }
