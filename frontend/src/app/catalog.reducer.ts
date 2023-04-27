import { createReducer, on } from '@ngrx/store';
import { addProduct, addProductFailure, addProductSuccess, getAllCategories, getAllCategoriesSuccess, getAllProducts, getAllProductsSuccess, getCategoryById, getCategoryByIdSuccess, getProductById, getProductByIdFailure, getProductByIdSuccess, increaseStockPerProductSucess, updateCategoryByIdSuccess, updateProductById, updateProductByIdFailure, updateProductByIdSuccess } from './catalog.actions';
import { CategoryResponse } from './models/categoryResponse.model';
import { ProductResponse } from './models/productResponse.model';

export interface CatalogState{
    AllCategories: CategoryResponse[];
    ActualCategory: CategoryResponse;
    AllProducts: ProductResponse[];
    ActualProduct: ProductResponse;
}

export const initialState: CatalogState = {
    AllCategories: [],
    ActualCategory: {id: '',
                     name: ''},
    AllProducts: [],
    ActualProduct: {id: '',
                    title:'',
                    price: 0,
                    description: '',
                    categoryId: '',
                    stock: 0}
}

export const catalogReducer = createReducer(
  initialState,
  on(addProduct, state => ({
    ...state
  })),
  on(addProductSuccess, (state, { product }) => (
    { ...state,
      products: [...state.AllProducts, product]
    })),
  on(addProductFailure, (state, { error }) => (
    { ...state,
    })),
  on(getAllProducts, state => ({
    ...state
  })),  
  on(getAllProductsSuccess, (state, { products }) => {
    return {
      ...state,
      AllProducts: products
    };
  }),
  on(getProductById, state => ({
    ...state,
  })),
  on(getProductByIdSuccess, (state, { product }) => ({
    ...state,
    ActualProduct: product
  })),
  on(getProductByIdFailure, (state, { error }) => ({
    ...state
  })),
  on(updateProductById, state => ({
    ...state,
  })),
  on(updateProductByIdSuccess, (state, { product }) => ({
    ...state,
    ActualProduct: product
  })),
  on(updateProductByIdFailure, (state, { error }) => ({
    ...state
  })),
  on(increaseStockPerProductSucess,(state, {product}) => {
    return {
      ...state,
      ActualProduct: product,
    };
  }),
  on(getAllCategories, state => ({
    ...state
  })),  
  on(getAllCategoriesSuccess, (state, { categories }) => {
    return {
      ...state,
      AllCategories: categories,
    };
  }),
  on(getCategoryByIdSuccess, (state, { category }) => {
    return {
      ...state,
      ActualCategory: category,
    };
  }),
  on(updateCategoryByIdSuccess, (state, { category }) => {
    return {
      ...state,
      ActualCategory: category,
    };
  }),
);