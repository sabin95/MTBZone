import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Actions } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { Subject, filter, take, takeUntil } from 'rxjs';
import { addCategory, addCategoryFailure, addCategorySuccess, updateCategoryById, updateCategoryByIdFailure, updateCategoryByIdSuccess } from 'src/app/catalog.actions';
import { Category } from 'src/app/models/category.model';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';

@Component({
  selector: 'app-category-dialog-box',
  templateUrl: './category-dialog-box.component.html',
  styleUrls: ['./category-dialog-box.component.less']
})
export class CategoryDialogBoxComponent {
  dataCategory: Partial<CategoryResponse>;
  editCategory: CategoryResponse;
  private destroy$: Subject<void> = new Subject();


  constructor(
    public dialogRef: MatDialogRef<CategoryDialogBoxComponent>,
    private store: Store<{ categories: CategoryResponse[], catalogError: any }>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private actions$: Actions
  ) {
    this.editCategory = data.editCategory || null;
    this.dataCategory = this.editCategory ? { ...this.editCategory } : {
      name: ''
    };
  } 

  onNoClick(): void {
    this.dialogRef.close();
  }

  async onSaveClick(): Promise<void> {
    const data = {
      name: this.dataCategory.name
    };
    const category: Category = data as Category;
    if (this.editCategory) {
      const updatedCategory: Category = {
        ...this.editCategory,
        ...category
      };
      this.store.dispatch(updateCategoryById({ id: this.editCategory.id, category: updatedCategory }));
    } else {
      this.store.dispatch(addCategory({ category }));
    }
  
    this.actions$.pipe(
      filter(action =>
        action.type === (this.editCategory ? updateCategoryByIdSuccess.type : addCategorySuccess.type) ||
        action.type === (this.editCategory ? updateCategoryByIdFailure.type : addCategoryFailure.type)
      ),
      take(1),
      takeUntil(this.destroy$) 
    ).subscribe(action => {
      if (action.type === (this.editCategory ? updateCategoryByIdSuccess.type : addCategorySuccess.type)) {
        this.dialogRef.close(data);
      }
    });
  }

  get isFormValid(): boolean {
    return (
      !!this.dataCategory.name
    );
  } 

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
}
