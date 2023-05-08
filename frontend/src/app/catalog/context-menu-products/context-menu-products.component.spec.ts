import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContextMenuProductsComponent } from './context-menu-products.component';

describe('ContextMenuComponent', () => {
  let component: ContextMenuProductsComponent;
  let fixture: ComponentFixture<ContextMenuProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContextMenuProductsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContextMenuProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
