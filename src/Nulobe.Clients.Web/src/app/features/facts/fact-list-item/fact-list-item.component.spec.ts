import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactListItemComponent } from './fact-list-item.component';

describe('FactListItemComponent', () => {
  let component: FactListItemComponent;
  let fixture: ComponentFixture<FactListItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactListItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
