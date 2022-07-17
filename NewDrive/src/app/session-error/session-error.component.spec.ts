import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionErrorComponent } from './session-error.component';

describe('SessionErrorComponent', () => {
  let component: SessionErrorComponent;
  let fixture: ComponentFixture<SessionErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SessionErrorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SessionErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
