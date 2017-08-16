import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFactComponent } from './edit-fact.component';

describe('EditFactComponent', () => {
  let component: EditFactComponent;
  let fixture: ComponentFixture<EditFactComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditFactComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditFactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
