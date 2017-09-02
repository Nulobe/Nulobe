import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactListComponent } from './fact-list.component';

describe('FactListComponent', () => {
  let component: FactListComponent;
  let fixture: ComponentFixture<FactListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
