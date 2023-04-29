import { Component, Input, Output, EventEmitter, Renderer2, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable, filter, firstValueFrom, take, tap } from 'rxjs';
import { deleteProductById, getProductById, increaseStockPerProduct } from 'src/app/catalog.actions';
import { selectProductById } from 'src/app/catalog.selectors';
import { ProductDialogBoxComponent } from '../product-dialog-box/product-dialog-box.component';
import { ProductResponse } from 'src/app/models/productResponse.model';

@Component({
  selector: 'app-context-menu-products',
  templateUrl: './context-menu-products.component.html',
  styleUrls: ['./context-menu-products.component.less']
})
export class ContextMenuProductsComponent {
  @Input() position: { x: string; y: string };
  @Input() productId: string;
  @Output() contextMenuClosed = new EventEmitter<void>();
  actualProduct$: Observable<ProductResponse | undefined>;

  stockValues = [1, 2, 5, 10, 20, 50, 100];
  customStockValue: number;
  increaseStockSubmenuVisible = false;
  updatedProduct: ProductResponse;

  constructor(private store: Store,private renderer: Renderer2, private el: ElementRef, public dialog: MatDialog) {}

  ngOnChanges() {
    this.updatePosition();
  }

  updatePosition() {
    this.renderer.setStyle(this.el.nativeElement, 'left', this.position.x);
    this.renderer.setStyle(this.el.nativeElement, 'top', this.position.y);
  }

  deleteProduct() {
    this.store.dispatch(deleteProductById({ id: this.productId }));
  }

  async editProduct() {
    this.actualProduct$ = this.store.select(selectProductById(this.productId));
    const product = await firstValueFrom(this.actualProduct$);
    if (product) {
      this.updatedProduct = { ...product };
      this.openEditDialog();
    }
  }
  
  


  openEditDialog() {
    if (this.updatedProduct) {
      this.dialog.open(ProductDialogBoxComponent, {
        width: '400px',
        data: {
          editProduct: {
            ...this.updatedProduct,
            id: this.updatedProduct.id,
            stock: this.updatedProduct.stock
          }
        }
      });
    }
  }

  increaseStock(quantity: number) {
    this.store.dispatch(increaseStockPerProduct({ id: this.productId, quantity }));
  }

  showIncreaseStockSubmenu() {
    this.increaseStockSubmenuVisible = true;
  }

  hideIncreaseStockSubmenu() {
    this.increaseStockSubmenuVisible = false;
  }

  hideContextMenu() {
    this.hideIncreaseStockSubmenu();
    this.contextMenuClosed.emit();
  }
}
