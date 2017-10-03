import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SourceDateComponent } from './source-date.component';

describe('SourceDateComponent', () => {
  let component: SourceDateComponent;
  let fixture: ComponentFixture<SourceDateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SourceDateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SourceDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
