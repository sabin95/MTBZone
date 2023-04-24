import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable, Subject, filter, take, takeUntil } from 'rxjs';
import { addProduct, addProductFailure, addProductSuccess, getAllCategories, updateProductById, updateProductByIdFailure, updateProductByIdSucess } from 'src/app/catalog.actions';
import { selectAllCategories } from 'src/app/catalog.selectors';
import { Product } from 'src/app/models/product.model';
import { ProductResponse } from 'src/app/models/productResponse.model';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Actions } from '@ngrx/effects';


@Component({
  selector: 'app-product-dialog-box',
  templateUrl: './product-dialog-box.component.html',
  styleUrls: ['./product-dialog-box.component.less']
})
export class ProductDialogBoxComponent {
  title: string;
  price: number;
  description: string;
  categoryId: string;
  errorMessage: string;
  id: string;
  stock: number;
  editProduct: ProductResponse | null = null;
  categories$: Observable<CategoryResponse[]>;
  private destroy$: Subject<void> = new Subject();


  constructor(
    public dialogRef: MatDialogRef<ProductDialogBoxComponent>,
    private store: Store<{ products: ProductResponse[], catalogError: any}>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar,
    private actions$: Actions
    ) { 
      this.editProduct = data.editProduct || null;
    }

  ngOnInit() {
    this.store.dispatch(getAllCategories());
    this.categories$ = this.store.select(selectAllCategories);
    if (this.editProduct) {
      this.title = this.editProduct.title;
      this.price = this.editProduct.price;
      this.description = this.editProduct.description;
      this.categoryId = this.editProduct.categoryId;
    }
  }
  

  onNoClick(): void {
    this.dialogRef.close();
  }

  async onSaveClick(): Promise<void> {
    const data = {
      title: this.title,
      price: this.price,
      description: this.description,
      categoryId: this.categoryId
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
        action.type === (this.editProduct ? updateProductByIdSucess.type : addProductSuccess.type) ||
        action.type === (this.editProduct ? updateProductByIdFailure.type : addProductFailure.type)
      ),
      take(1),
      takeUntil(this.destroy$) 
    ).subscribe(action => {
      if (action.type === (this.editProduct ? updateProductByIdSucess.type : addProductSuccess.type)) {
        this.dialogRef.close(data);
      }
    });
  }
  

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
}
