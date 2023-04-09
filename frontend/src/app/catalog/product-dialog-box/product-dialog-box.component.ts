import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addProduct } from 'src/app/catalog.actions';
import { selectCatalogError } from 'src/app/catalog.selectors';
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
  error: any;

  constructor(
    public dialogRef: MatDialogRef<ProductDialogBoxComponent>,
    private store: Store<{ products: ProductResponse[]}>
    ) { 
      this.error = this.store.select(selectCatalogError);
    }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    const data = {
      title: this.title,
      price: this.price,
      description: this.description,
      categoryId: this.categoryId
    };
    const product: Product = data as Product;
    this.store.dispatch(addProduct({ product }));
    if (!this.error) {
      this.dialogRef.close(data);
    }
  }
}
