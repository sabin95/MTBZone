import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-product-dialog-box',
  templateUrl: './product-dialog-box.component.html',
  styleUrls: ['./product-dialog-box.component.less']
})
export class ProductDialogBoxComponent {
  title: string;
  price: number;
  description: string;
  categoryId: number;

  constructor(public dialogRef: MatDialogRef<ProductDialogBoxComponent>) { }

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
    this.dialogRef.close(data);
  }
}
