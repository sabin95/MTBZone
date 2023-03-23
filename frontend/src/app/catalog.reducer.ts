import { createReducer, on } from '@ngrx/store';
import { addProduct, getProductById, getAllProducts, updateProductById, deleteById, increaseStockPerProduct, addProductFailure, addProductSuccess, getAllProductsSuccess } from './catalog.actions';
import { Category } from './models/category.model';
import { Product } from './models/product.model';
import { ProductResponse } from './models/productResponse.model';

export interface CatalogState{
    AvailableCategories: Category[];
    AllProducts: ProductResponse[];
    AvailableProducts: ProductResponse[];
    OutOfStockProducts: ProductResponse[];
}

export const initialState: CatalogState = {
    AvailableCategories: [],
    AllProducts: [],
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
  })
);