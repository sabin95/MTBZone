import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable, filter, first, firstValueFrom, take } from 'rxjs';
import { addProduct } from 'src/app/catalog.actions';
import { selectCatalogError, selectCatalogLoading } from 'src/app/catalog.selectors';
import { Product } from 'src/app/models/product.model';
import { ProductResponse } from 'src/app/models/productResponse.model';

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

  constructor(
    public dialogRef: MatDialogRef<ProductDialogBoxComponent>,
    private store: Store<{ products: ProductResponse[], catalogError: any}>
    ) { }

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
    this.store.dispatch(addProduct({ product }));
  
    const catalogLoading$ = this.store.select(selectCatalogLoading);
    await firstValueFrom(
      catalogLoading$.pipe(filter(loading => !loading))
    );   
    const catalogError$ = this.store.select(selectCatalogError);
    const catalogError = await firstValueFrom(
      catalogError$.pipe(take(1))
    );  
    if (catalogError) {
      this.errorMessage = catalogError;
    } else {
      this.dialogRef.close(data);
    }
  }
}
