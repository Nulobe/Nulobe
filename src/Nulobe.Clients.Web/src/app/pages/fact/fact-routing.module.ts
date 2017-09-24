import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FactComponent } from './fact.component';

const routes: Routes = [
  {
    component: FactComponent,
    path: 'n/:slugNuance/:slugTitle'
  },
  {
    component: FactComponent,
    path: 'n/:factId'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FactRoutingModule { }
