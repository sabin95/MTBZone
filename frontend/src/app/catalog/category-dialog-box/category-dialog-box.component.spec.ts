import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryDialogBoxComponent } from './category-dialog-box.component';

describe('CategoryDialogBoxComponent', () => {
  let component: CategoryDialogBoxComponent;
  let fixture: ComponentFixture<CategoryDialogBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryDialogBoxComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryDialogBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
