import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SourceFormListComponent } from './source-form-list.component';

describe('SourceFormListComponent', () => {
  let component: SourceFormListComponent;
  let fixture: ComponentFixture<SourceFormListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SourceFormListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SourceFormListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
