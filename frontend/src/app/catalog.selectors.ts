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

export const selectAllCategories = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.AllCategories
);

export const selectCategoryById = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.ActualCategory
); 

export const selectCatalogError = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.catalogError
);

export const selectCatalogLoading = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.loading
);
