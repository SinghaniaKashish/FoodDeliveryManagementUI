import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RestaurantcrudComponent } from './restaurantcrud.component';

describe('RestaurantcrudComponent', () => {
  let component: RestaurantcrudComponent;
  let fixture: ComponentFixture<RestaurantcrudComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RestaurantcrudComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RestaurantcrudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
