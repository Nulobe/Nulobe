import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SourceJournalComponent } from './source-journal.component';

describe('SourceJournalComponent', () => {
  let component: SourceJournalComponent;
  let fixture: ComponentFixture<SourceJournalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SourceJournalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SourceJournalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
