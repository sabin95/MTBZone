import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { switchMap, map, catchError, mergeMap, } from 'rxjs/operators';
import { of } from 'rxjs';
import { addCategory, addCategoryFailure, addCategorySuccess, addProduct, addProductFailure, addProductSuccess, deleteCategoryById, deleteCategoryByIdFailure, deleteCategoryByIdSuccess, deleteProductById, deleteProductByIdFailure, deleteProductByIdSucess, getAllCategories, getAllCategoriesFailure, getAllCategoriesSuccess, getAllProducts, getAllProductsFailure, getAllProductsSuccess, getCategoryById, getCategoryByIdFailure, getCategoryByIdSuccess, getProductById, getProductByIdFailure, getProductByIdSuccess, increaseStockPerProduct, increaseStockPerProductFailure, increaseStockPerProductSucess, updateCategoryById, updateCategoryByIdFailure, updateCategoryByIdSuccess, updateProductById, updateProductByIdFailure, updateProductByIdSuccess } from './catalog.actions';
import { CatalogService } from './catalog.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class CatalogEffects {
  constructor(
    private catalogActions$: Actions,
    private catalogService: CatalogService,
    private snackBar: MatSnackBar
  ) {}

  addProductEffect$ = createEffect(() =>
  this.catalogActions$.pipe(
    ofType(addProduct),
    switchMap((action) =>
      this.catalogService.addProduct(action.product).pipe(
        map((response) => addProductSuccess({ product: response })),
        catchError((error) => {
          const errorMessage = error?.error;
          this.snackBar.open(errorMessage, 'Close', { duration: 3000 });
          return of(addProductFailure({ error: errorMessage }));
        })
      )
    )
  )
);

  getProductByIdEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(getProductById),
      switchMap((action) =>
        this.catalogService.getProductById(action.productId).pipe(
          map((response) => getProductByIdSuccess({ product: response })),
          catchError((error) => {
            const errorMessage = error?.error;
            return of(getProductByIdFailure({ error: errorMessage }));})
        )
      )
    )
  );

  getAllProductsEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(getAllProducts),
      mergeMap(() =>
        this.catalogService.getAllProducts().pipe(
          map((products:any) => getAllProductsSuccess({products})),
          catchError((error) => of(getAllProductsFailure({error})))
        )
      )
    )
  );

  UpdateProductEffect$ = createEffect(() =>
  this.catalogActions$.pipe(
    ofType(updateProductById),
    mergeMap((action) =>
      this.catalogService.updateProduct(action.id, action.product).pipe(
        map((product: any) => updateProductByIdSuccess({ product })),
        catchError((error) => {
          const errorMessage = error?.error;
          this.snackBar.open(errorMessage, 'Close', { duration: 3000 });
          return of(updateProductByIdFailure({ error: errorMessage }));
        })
      )
    )
  )
);

  deleteProductEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(deleteProductById),
      switchMap((action) =>
        this.catalogService.deleteProductById(action.id).pipe(
          map(() => deleteProductByIdSucess()),
          catchError((error) => of(deleteProductByIdFailure({error})))
        )
      )
    )
  );

  increaseStockPerProductEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(increaseStockPerProduct),
      switchMap((action) =>
        this.catalogService.increaseStockPerProduct(action.id, action.quantity).pipe(
          map((response) => increaseStockPerProductSucess({ product: response })),
          catchError((error) => of(increaseStockPerProductFailure({ error })))
        )
      )
    )
  );

  addCategoryEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(addCategory),
      switchMap((action) =>
        this.catalogService.addCategory(action.category).pipe(
          map((response) => addCategorySuccess({ category: response })),
          catchError((error) => of(addCategoryFailure({ error })))
        )
      )
    )
  );

  getCategoryByIdEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(getCategoryById),
      switchMap((action) =>
        this.catalogService.getCategoryById(action.id).pipe(
          map((response) => getCategoryByIdSuccess({ category: response })),
          catchError((error) => of(getCategoryByIdFailure({ error })))
        )
      )
    )
  );

  getAllCategoriesEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(getAllCategories),
      mergeMap(() =>
        this.catalogService.getAllCategories().pipe(
          map((categories:any) => getAllCategoriesSuccess({categories})),
          catchError((error) => of(getAllCategoriesFailure({error})))
        )
      )
    )
  );

  UpdateCategoryEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(updateCategoryById),
      mergeMap((action) =>
        this.catalogService.updateCategory(action.id, action.category).pipe(
          map((category:any) => updateCategoryByIdSuccess({category})),
          catchError((error) => of(updateCategoryByIdFailure({error})))
        )
      )
    )
  );

  deleteCategoryEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(deleteCategoryById),
      switchMap((action) =>
        this.catalogService.deleteCategoryById(action.id).pipe(
          map(() => deleteCategoryByIdSuccess()),
          catchError((error) => of(deleteCategoryByIdFailure({error})))
        )
      )
    )
  );
  
}
