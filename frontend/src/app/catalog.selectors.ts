import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CatalogState } from './catalog.reducer';

export const selectCatalogState = createFeatureSelector<CatalogState>('catalogState');

export const selectAllProducts = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.AllProducts
);

export const selectProductById = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.ActualProduct
); 