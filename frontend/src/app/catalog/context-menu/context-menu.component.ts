import { Component, Input, Output, EventEmitter } from '@angular/core';
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

  stockValues = [1, 2, 5, 10, 20, 50, 100];
  customStockValue: number;

  constructor(private store: Store) {}

  deleteProduct() {
    this.store.dispatch(deleteProductById({ id: this.productId }));
  }

  editProduct() {
    
  }

  increaseStock(quantity: number) {
    this.store.dispatch(increaseStockPerProduct({ id: this.productId, quantity }));
  }
}
