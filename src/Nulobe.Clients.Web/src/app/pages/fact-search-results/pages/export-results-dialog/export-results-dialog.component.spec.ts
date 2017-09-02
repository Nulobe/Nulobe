import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportResultsDialogComponent } from './export-results-dialog.component';

describe('ExportResultsDialogComponent', () => {
  let component: ExportResultsDialogComponent;
  let fixture: ComponentFixture<ExportResultsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportResultsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportResultsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
