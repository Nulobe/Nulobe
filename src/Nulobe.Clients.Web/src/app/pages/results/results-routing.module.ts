import { NgModule } from '@angular/core';
import { Routes, RouterModule, Route, UrlMatcher, UrlMatchResult, UrlSegment, UrlSegmentGroup } from '@angular/router';

import { ResultsComponent } from './results.component';

const routes: Routes = [
  {
    component: ResultsComponent,
    matcher: (segments: UrlSegment[], group: UrlSegmentGroup, route: Route) => {
      let consumed = [];
      
      if (segments.length && segments[0].path !== 'LOBE') {
        consumed = segments;
      }

      return { consumed }
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResultsRoutingModule { }
