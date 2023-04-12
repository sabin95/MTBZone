import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { filter, firstValueFrom, take } from 'rxjs';
import { addProduct, updateProductById } from 'src/app/catalog.actions';
import { selectCatalogError, selectCatalogLoading } from 'src/app/catalog.selectors';
import { Product } from 'src/app/models/product.model';
import { ProductResponse } from 'src/app/models/productResponse.model';
import { Input } from '@angular/core';

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


  constructor(
    public dialogRef: MatDialogRef<ProductDialogBoxComponent>,
    private store: Store<{ products: ProductResponse[], catalogError: any}>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ) { 
      this.editProduct = data.editProduct || null;
    }

  ngOnInit() {
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
