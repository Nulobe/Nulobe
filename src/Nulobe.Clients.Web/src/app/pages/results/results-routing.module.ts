import { NgModule } from '@angular/core';
import { Routes, RouterModule, Route, UrlMatcher, UrlMatchResult, UrlSegment, UrlSegmentGroup } from '@angular/router';

import { ResultsComponent } from './results.component';

export const resultsMatcher: UrlMatcher = (segments: UrlSegment[], group: UrlSegmentGroup, route: Route) => {
  let consumed = [];
      
  if (segments.length && segments[0].path !== 'LOBE') {
    consumed = segments;
  }

  return { consumed }
} 

export const routes: Routes = [
  {
    component: ResultsComponent,
    matcher: resultsMatcher
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResultsRoutingModule { }
