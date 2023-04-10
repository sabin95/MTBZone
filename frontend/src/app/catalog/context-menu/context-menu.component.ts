import { Component, Input, Output, EventEmitter, Renderer2, ElementRef } from '@angular/core';
import { Store } from '@ngrx/store';
import { deleteProductById, increaseStockPerProduct } from 'src/app/catalog.actions';

@Component({
  selector: 'app-context-menu',
  templateUrl: './context-menu.component.html',
  styleUrls: ['./context-menu.component.less']
})
export class ContextMenuComponent {
  @Input() position: { x: string; y: string };
  @Input() productId: string;
  @Output() contextMenuClosed = new EventEmitter<void>();

  stockValues = [1, 2, 5, 10, 20, 50, 100];
  customStockValue: number;
  increaseStockSubmenuVisible = false;

  constructor(private store: Store,private renderer: Renderer2, private el: ElementRef) {}

  ngOnChanges() {
    this.updatePosition();
  }

  updatePosition() {
    this.renderer.setStyle(this.el.nativeElement, 'left', this.position.x);
    this.renderer.setStyle(this.el.nativeElement, 'top', this.position.y);
  }

  deleteProduct() {
    this.store.dispatch(deleteProductById({ id: this.productId }));
  }

  editProduct() {
    
  }

  increaseStock(quantity: number) {
    this.store.dispatch(increaseStockPerProduct({ id: this.productId, quantity }));
  }

  showIncreaseStockSubmenu() {
    this.increaseStockSubmenuVisible = true;
  }

  hideIncreaseStockSubmenu() {
    this.increaseStockSubmenuVisible = false;
  }

  hideContextMenu() {
    this.hideIncreaseStockSubmenu();
    this.contextMenuClosed.emit();
  }
}
