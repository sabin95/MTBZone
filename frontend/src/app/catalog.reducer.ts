import { createReducer, on } from '@ngrx/store';
import { getAllProducts, getAllProductsSuccess, getProductByIdSuccess } from './catalog.actions';
import { Category } from './models/category.model';
import { ProductResponse } from './models/productResponse.model';

export interface CatalogState{
    AvailableCategories: Category[];
    AllProducts: ProductResponse[];
    ActualProduct: ProductResponse | null;
    AvailableProducts: ProductResponse[];
    OutOfStockProducts: ProductResponse[];
}

export const initialState: CatalogState = {
    AvailableCategories: [],
    AllProducts: [],
    ActualProduct: null,
    AvailableProducts: [],
    OutOfStockProducts: []
}

export const catalogReducer = createReducer(
  initialState,
  on(getAllProducts, state => ({
    ...state
  })),  
  on(getAllProductsSuccess, (state, { products }) => {
    return {
      ...state,
      AllProducts: products,
    };
  }),
  on(getProductByIdSuccess, (state, { product }) => {
    return {
      ...state,
      ActualProduct: product,
    };
  })
);