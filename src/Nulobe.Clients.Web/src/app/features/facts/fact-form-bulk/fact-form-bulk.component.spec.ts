import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactFormBulkComponent } from './fact-form-bulk.component';

describe('FactFormBulkComponent', () => {
  let component: FactFormBulkComponent;
  let fixture: ComponentFixture<FactFormBulkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactFormBulkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactFormBulkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
