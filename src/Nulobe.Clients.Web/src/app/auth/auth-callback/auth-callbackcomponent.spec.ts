import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthCalbackComponent } from './auth-calback.component';

describe('AuthCalbackComponent', () => {
  let component: AuthCalbackComponent;
  let fixture: ComponentFixture<AuthCalbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthCalbackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthCalbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
