import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateFactsComponent } from './create-facts.component';

describe('CreateFactsComponent', () => {
  let component: CreateFactsComponent;
  let fixture: ComponentFixture<CreateFactsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateFactsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateFactsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
