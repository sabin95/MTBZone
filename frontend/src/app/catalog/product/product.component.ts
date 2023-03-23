import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addProduct, deleteById, getAllProducts, getProductById, increaseStockPerProduct, updateProductById } from 'src/app/catalog.actions';
import { selectAllProducts } from 'src/app/catalog.selectors';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.less']
})
export class ProductComponent implements OnInit {
  productForm: FormGroup;
  products$: Observable<Product[]>;

  constructor(
    private store: Store<{ products: Product[] }>,
    private formBuilder: FormBuilder
  ) {

    this.productForm = this.formBuilder.group({
      Title: ['', Validators.required],
      Price: [0, Validators.required],
      Description: ['', Validators.required],
      CategoryId: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.products$ = this.store.select(selectAllProducts);
  }

  onAddProduct() {
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
