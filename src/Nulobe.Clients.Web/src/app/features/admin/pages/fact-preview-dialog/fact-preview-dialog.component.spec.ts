import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactPreviewDialogComponent } from './fact-preview-dialog.component';

describe('FactPreviewDialogComponent', () => {
  let component: FactPreviewDialogComponent;
  let fixture: ComponentFixture<FactPreviewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactPreviewDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactPreviewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
