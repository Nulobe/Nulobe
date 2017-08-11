import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactFormComponent } from './fact-form.component';

describe('FactFormComponent', () => {
  let component: FactFormComponent;
  let fixture: ComponentFixture<FactFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
