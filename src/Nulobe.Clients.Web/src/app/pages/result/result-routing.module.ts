import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ResultComponent } from './result.component';

const routes: Routes = [
  {
    component: ResultComponent,
    path: 'n/:slugNuance/:slugTitle'  
  }  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResultRoutingModule { }
