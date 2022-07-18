import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModerateFilesComponent } from './moderate-files.component';

describe('ModerateFilesComponent', () => {
  let component: ModerateFilesComponent;
  let fixture: ComponentFixture<ModerateFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModerateFilesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModerateFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
