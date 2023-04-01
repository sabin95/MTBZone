import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addCategory, deleteCategoryById, getAllCategories, getCategoryById, updateCategoryById } from 'src/app/catalog.actions';
import { selectAllCategories, selectCategoryById } from 'src/app/catalog.selectors';
import { Category } from 'src/app/models/category.model';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.less']
})
export class CategoryComponent {
  categoryForm: FormGroup;
  categories$: Observable<CategoryResponse[]>;
  actualCategory$: Observable<CategoryResponse>;
  categoryId: string;
  showCategories = false;
  editCategory: CategoryResponse;

  constructor(
    private store: Store<{ categories: CategoryResponse[] }>,
    private formBuilder: FormBuilder
  ) {

    this.categoryForm = this.formBuilder.group({
      Id: ['', Validators.required],
      Name: ['', Validators.required],
    });
  }
  ngOnInit(): void {
    this.categories$ = this.store.select(selectAllCategories);
  }

  onAddCategory() {
    const category: Category = this.categoryForm.value as Category;
    this.store.dispatch(addCategory({ category }));
    this.categoryForm.reset();
  }
  

  getCategoryById(id: string) {
    this.store.dispatch(getCategoryById({id}));
    this.actualCategory$ = this.store.select(selectCategoryById);
    this.actualCategory$.subscribe((category) => {
      this.editCategory = { ...category };
    });
  }

  getAllCategories() {
    this.showCategories = !this.showCategories;
    this.store.dispatch(getAllCategories());
  }
  

  updateCategoryById() {
    const updatedCategory: Category = {
      name: this.editCategory.name,
    };
    this.store.dispatch(updateCategoryById({ id: this.editCategory.id, category: updatedCategory }));
  }
  

  deleteCategoryById(id:string) {
    this.store.dispatch(deleteCategoryById({id}));
  }
}
