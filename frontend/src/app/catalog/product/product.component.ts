import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addProduct, deleteProductById, getAllProducts, getProductById, increaseStockPerProduct, updateProductById } from 'src/app/catalog.actions';
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
  actualProduct$: Observable<ProductResponse>;
  productId: string;
  showProducts = false;
  editProduct: ProductResponse;
  stockQuantityIncrease: number;

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
    this.actualProduct$.subscribe((product) => {
      this.editProduct = { ...product };
    });
  }

  getAllProducts() {
    this.showProducts = !this.showProducts;
    this.store.dispatch(getAllProducts());
  }
  

  updateProductById() {
    const updatedProduct: Product = {
      title: this.editProduct.title,
      price: this.editProduct.price,
      description: this.editProduct.description,
      categoryId: this.editProduct.categoryId
    };
    this.store.dispatch(updateProductById({ id: this.editProduct.id, product: updatedProduct }));
  }
  

  deleteProductById(id:string) {
    this.store.dispatch(deleteProductById({id}));
  }

  increaseStockPerProduct(id:string) {
    this.store.dispatch(increaseStockPerProduct({id:id, quantity:this.stockQuantityIncrease}));
  }
}
