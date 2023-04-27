import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable, Subject, filter, take, takeUntil } from 'rxjs';
import { addProduct, addProductFailure, addProductSuccess, getAllCategories, updateProductById, updateProductByIdFailure, updateProductByIdSuccess } from 'src/app/catalog.actions';
import { selectAllCategories } from 'src/app/catalog.selectors';
import { Product } from 'src/app/models/product.model';
import { ProductResponse } from 'src/app/models/productResponse.model';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';
import { Actions } from '@ngrx/effects';


@Component({
  selector: 'app-product-dialog-box',
  templateUrl: './product-dialog-box.component.html',
  styleUrls: ['./product-dialog-box.component.less']
})
export class ProductDialogBoxComponent {
  dataProduct: Partial<ProductResponse>;
  editProduct: ProductResponse;
  categories$: Observable<CategoryResponse[]>;
  private destroy$: Subject<void> = new Subject();


  constructor(
    public dialogRef: MatDialogRef<ProductDialogBoxComponent>,
    private store: Store<{ products: ProductResponse[], catalogError: any }>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private actions$: Actions
  ) {
    this.editProduct = data.editProduct || null;
    this.dataProduct = this.editProduct ? { ...this.editProduct } : {
      title: '',
      price: 0,
      description: '',
      categoryId: ''
    };
  }
  

  ngOnInit() {
    this.store.dispatch(getAllCategories());
    this.categories$ = this.store.select(selectAllCategories);    
  }
  

  onNoClick(): void {
    this.dialogRef.close();
  }

  async onSaveClick(): Promise<void> {
    const data = {
      title: this.dataProduct.title,
      price: this.dataProduct.price,
      description: this.dataProduct.description,
      categoryId: this.dataProduct.categoryId
    };
    const product: Product = data as Product;
  
    if (this.editProduct) {
      const updatedProduct: Product = {
        ...this.editProduct,
        ...product
      };
      this.store.dispatch(updateProductById({ id: this.editProduct.id, product: updatedProduct }));
    } else {
      this.store.dispatch(addProduct({ product }));
    }
  
    this.actions$.pipe(
      filter(action =>
        action.type === (this.editProduct ? updateProductByIdSuccess.type : addProductSuccess.type) ||
        action.type === (this.editProduct ? updateProductByIdFailure.type : addProductFailure.type)
      ),
      take(1),
      takeUntil(this.destroy$) 
    ).subscribe(action => {
      if (action.type === (this.editProduct ? updateProductByIdSuccess.type : addProductSuccess.type)) {
        this.dialogRef.close(data);
      }
    });
  }

  get isFormValid(): boolean {
    return (
      !!this.dataProduct.title &&
      !!this.dataProduct.price &&
      !!this.dataProduct.description &&
      !!this.dataProduct.categoryId
    );
  } 

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
}
