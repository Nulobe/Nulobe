import { NgModule } from '@angular/core';
import { Routes, RouterModule, Route } from '@angular/router';

import { FactSearchResultsComponent } from './fact-search-results.component';

export const routes: Routes = [
  {
    redirectTo: '',
    path: 'q'
  },
  {
    component: FactSearchResultsComponent,
    path: 'q/:tags'
  },
  {
    // Route to allow force refresh of same query
    redirectTo: 'q/:tags',
    path: 'q/:tags/force'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FactSearchResultsRoutingModule { }
