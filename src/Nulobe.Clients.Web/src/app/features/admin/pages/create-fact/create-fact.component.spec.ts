import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateFactComponent } from './create-fact.component';

describe('CreateFactsComponent', () => {
  let component: CreateFactComponent;
  let fixture: ComponentFixture<CreateFactComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateFactComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateFactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
