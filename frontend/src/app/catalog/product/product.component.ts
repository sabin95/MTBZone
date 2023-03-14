import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  productForm: FormGroup;

  constructor(
    private store: Store<{ count: number }>,
    private formBuilder: FormBuilder
  ) {
    this.count$ = store.select('count');

    this.productForm = this.formBuilder.group({
      Title: ['', Validators.required],
      Price: [0, Validators.required],
      Description: ['', Validators.required],
      CategoryId: ['', Validators.required]
    });
  }


  onAddProduct() {
    debugger;
    const product: Product = this.productForm.value as Product;
    this.store.dispatch(addProduct({ product }));
    this.productForm.reset();
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
