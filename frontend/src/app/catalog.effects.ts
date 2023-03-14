import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { HttpClient } from '@angular/common/http';
import { switchMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { addProduct, addProductFailure, addProductSuccess } from './catalog.actions';
import { Product } from './models/product.model';

@Injectable()
export class CatalogEffects {
  constructor(
    private catalogActions$: Actions,
    private http: HttpClient
  ) {}

  addProductEffect$ = createEffect(() =>
  this.catalogActions$.pipe(
    ofType(addProduct),
    switchMap((action) =>
      this.http.post<Product>('https://localhost:7124/api/Products/Add', action.product).pipe(
        map((response) => addProductSuccess({ product: response })),
        catchError((error) => of(addProductFailure({ error })))
      )
    )
  )
);
}
