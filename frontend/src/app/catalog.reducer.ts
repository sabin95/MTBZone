import { createReducer, on } from '@ngrx/store';
import { getAllProducts, getAllProductsSuccess, getProductByIdSuccess, increaseStockPerProductSucess, updateProductByIdSucess } from './catalog.actions';
import { Category } from './models/category.model';
import { ProductResponse } from './models/productResponse.model';

export interface CatalogState{
    AvailableCategories: Category[];
    AllProducts: ProductResponse[];
    ActualProduct: ProductResponse;
    AvailableProducts: ProductResponse[];
    OutOfStockProducts: ProductResponse[];
}

export const initialState: CatalogState = {
    AvailableCategories: [],
    AllProducts: [],
    ActualProduct: {id: '',
                    title:'',
                    price: 0,
                    description: '',
                    categoryId: '',
                    stock: 0},
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
  }),
  on(updateProductByIdSucess, (state, { product }) => {
    return {
      ...state,
      ActualProduct: product,
    };
  }),
  on(increaseStockPerProductSucess,(state, {product}) => {
    return {
      ...state,
      ActualProduct: product,
    };
  }),
);