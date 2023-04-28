import { Component, Input, Output, EventEmitter, Renderer2, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable, firstValueFrom } from 'rxjs';
import { deleteCategoryById, deleteProductById, increaseStockPerProduct } from 'src/app/catalog.actions';
import { selectCategoryById } from 'src/app/catalog.selectors';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';
import { CategoryDialogBoxComponent } from '../category-dialog-box/category-dialog-box.component';

@Component({
  selector: 'app-context-menu-categories',
  templateUrl: './context-menu-categories.component.html',
  styleUrls: ['./context-menu-categories.component.less']
})
export class ContextMenuCategoriesComponent {
  @Input() position: { x: string; y: string };
  @Input() categoryId: string;
  @Output() contextMenuClosed = new EventEmitter<void>();
  actualCategory$: Observable<CategoryResponse | undefined>;
  updatedCategory: CategoryResponse;

  constructor(private store: Store,private renderer: Renderer2, private el: ElementRef, public dialog: MatDialog) {}

  ngOnChanges() {
    this.updatePosition();
  }

  updatePosition() {
    this.renderer.setStyle(this.el.nativeElement, 'left', this.position.x);
    this.renderer.setStyle(this.el.nativeElement, 'top', this.position.y);
  }

  deleteCategory() {
    this.store.dispatch(deleteCategoryById({ id: this.categoryId }));
  }

  async editCategory() {
    this.actualCategory$ = this.store.select(selectCategoryById(this.categoryId));
    const category = await firstValueFrom(this.actualCategory$);
    if (category) {
      this.updatedCategory = { ...category };
      this.openEditDialog();
    }
  }
  
  


  openEditDialog() {
    if (this.updatedCategory) {
      this.dialog.open(CategoryDialogBoxComponent, {
        width: '400px',
        data: {
          editCategory: {
            ...this.updatedCategory,
            id: this.updatedCategory.id
          }
        }
      });
    }
  }

  hideContextMenu() {
    this.contextMenuClosed.emit();
  }
}
