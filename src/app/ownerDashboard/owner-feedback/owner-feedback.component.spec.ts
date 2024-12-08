import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnerFeedbackComponent } from './owner-feedback.component';

describe('OwnerFeedbackComponent', () => {
  let component: OwnerFeedbackComponent;
  let fixture: ComponentFixture<OwnerFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OwnerFeedbackComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OwnerFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
