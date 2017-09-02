import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactSearchResultsComponent } from './fact-search-results.component';

describe('ResultsComponent', () => {
  let component: FactSearchResultsComponent;
  let fixture: ComponentFixture<FactSearchResultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactSearchResultsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactSearchResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
