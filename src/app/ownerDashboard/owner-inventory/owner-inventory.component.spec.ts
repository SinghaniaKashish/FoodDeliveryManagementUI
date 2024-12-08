import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnerInventoryComponent } from './owner-inventory.component';

describe('OwnerInventoryComponent', () => {
  let component: OwnerInventoryComponent;
  let fixture: ComponentFixture<OwnerInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OwnerInventoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OwnerInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
