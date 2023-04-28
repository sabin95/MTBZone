import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CatalogState } from './catalog.reducer';
import { ProductResponse } from './models/productResponse.model';
import { CategoryResponse } from './models/categoryResponse.model';

export const selectCatalogState = createFeatureSelector<CatalogState>('catalogState');

export const selectAllProducts = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.AllProducts
);

export const selectProductById = (productId: string) =>
  createSelector(
    selectAllProducts,
    (products: ProductResponse[]): ProductResponse | undefined =>
      products.find((product) => product.id === productId)
  );

export const selectAllCategories = createSelector(
  selectCatalogState,
  (state: CatalogState) => state.AllCategories
);

export const selectCategoryById = (categoryId: string) =>
  createSelector(
    selectAllCategories,
    (categories: CategoryResponse[]): CategoryResponse | undefined =>
    categories.find((category) => category.id === categoryId)
  );

