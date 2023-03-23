import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addProduct, deleteById, getAllProducts, getProductById, increaseStockPerProduct, updateProductById } from 'src/app/catalog.actions';
import { selectAllProducts, selectProductById } from 'src/app/catalog.selectors';
import { Product } from 'src/app/models/product.model';
import { ProductResponse } from 'src/app/models/productResponse.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.less']
})
export class ProductComponent implements OnInit {
  productForm: FormGroup;
  products$: Observable<ProductResponse[]>;
  actualProduct$: Observable<ProductResponse|null>;
  productId: string;
  showProducts = false;

  constructor(
    private store: Store<{ products: ProductResponse[] }>,
    private formBuilder: FormBuilder
  ) {

    this.productForm = this.formBuilder.group({
      Id: ['', Validators.required],
      Title: ['', Validators.required],
      Price: [0, Validators.required],
      Description: ['', Validators.required],
      CategoryId: ['', Validators.required],
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
  

  getProductById(productId:string) {
    this.store.dispatch(getProductById({productId}));
    this.actualProduct$ = this.store.select(selectProductById);
  }

  getAllProducts() {
    this.showProducts = !this.showProducts;
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
