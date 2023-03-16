import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { switchMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { addProduct, addProductFailure, addProductSuccess, getProductById, getProductByIdFailure, getProductByIdSuccess } from './catalog.actions';
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
        this.catalogService.getProductById(action.id).pipe(
          map((response) => getProductByIdSuccess({ product: response })),
          catchError((error) => of(getProductByIdFailure({ error })))
        )
      )
    )
  );
}
