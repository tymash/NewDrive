import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedDownloadComponent } from './shared-download.component';

describe('SharedDownloadComponent', () => {
  let component: SharedDownloadComponent;
  let fixture: ComponentFixture<SharedDownloadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SharedDownloadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SharedDownloadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
