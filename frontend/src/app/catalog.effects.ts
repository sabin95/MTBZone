import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { switchMap, map, catchError, mergeMap, } from 'rxjs/operators';
import { of } from 'rxjs';
import { addProduct, addProductFailure, addProductSuccess, deleteProductById, deleteProductByIdFailure, deleteProductByIdSucess, getAllProducts, getAllProductsFailure, getAllProductsSuccess, getProductById, getProductByIdFailure, getProductByIdSuccess, increaseStockPerProduct, increaseStockPerProductFailure, increaseStockPerProductSucess, updateProductById, updateProductByIdFailure, updateProductByIdSucess } from './catalog.actions';
import { CatalogService } from './catalog.service';

@Injectable()
export class CatalogEffects {
  constructor(
    private catalogActions$: Actions,
    private catalogService: CatalogService
  ) {}

  
  addProductEffect$ = createEffect(() =>
    this.catalogActions$.pipe(
      ofType(addProduct),
      switchMap((action) =>
        this.catalogService.addProduct(action.product).pipe(
          map((response) => addProductSuccess({ product: response })),
          catchError((error) => of(addProductFailure({ error })))
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
          catchError((error) => of(getProductByIdFailure({ error })))
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
          map((product:any) => updateProductByIdSucess({product})),
          catchError((error) => of(updateProductByIdFailure({error})))
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
  )

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
  
}
