import { Component, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addProduct, deleteById, getAllProducts, getProductById, increaseStockPerProduct, updateProductById } from 'src/app/catalog.actions';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.less']
})
export class ProductComponent {
  count$: Observable<number>;

  constructor(private store: Store<{ count: number }>) {
    this.count$ = store.select('count');
  }

  addProduct(product: Product) {
    this.store.dispatch(addProduct({product}));
  }

  getProductById(id:string) {
    this.store.dispatch(getProductById({id}));
  }

  getAllProducts() {
    this.store.dispatch(getAllProducts());
  }

  updateProductById(id:string, updatedProduct: Product) {
    this.store.dispatch(updateProductById({id, product: updatedProduct}));
  }

  deleteById(id:string) {
    this.store.dispatch(deleteById({id}));
  }

  increaseStockPerProduct(id:string, quantity:number) {
    this.store.dispatch(increaseStockPerProduct({id, quantity}));
  }
}
