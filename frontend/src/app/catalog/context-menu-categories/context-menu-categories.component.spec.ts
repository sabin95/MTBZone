import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContextMenuCategoriesComponent } from './context-menu-categories.component';

describe('ContextMenuCategoriesComponent', () => {
  let component: ContextMenuCategoriesComponent;
  let fixture: ComponentFixture<ContextMenuCategoriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContextMenuCategoriesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContextMenuCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
