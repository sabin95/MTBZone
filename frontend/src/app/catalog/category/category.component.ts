import { Component, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addCategory, deleteCategory, getAllCategories, getCategoryById, updateCategoryById } from 'src/app/catalog.actions';
import { Category } from 'src/app/models/category.model';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.less']
})
export class CategoryComponent {
  count$: Observable<number>;

  constructor(private store: Store<{count: number}>)
  {
    this.count$ = store.select('count');
  }

  addCategory(category: Category){
    this.store.dispatch(addCategory({category}));
  }

  getCategoryById(id: string){
    this.store.dispatch(getCategoryById({id}));
  }

  getAllCategories(){
    this.store.dispatch(getAllCategories());
  }

  updateCategoryById(id: string, updatedCategory: Category){
    this.store.dispatch(updateCategoryById({id, category: updatedCategory}));
  }

  deleteCategory(id: string){
    this.store.dispatch(deleteCategory({id}));
  }
}
