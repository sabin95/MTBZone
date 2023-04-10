import { createReducer, on } from '@ngrx/store';
import { addProduct, addProductFailure, addProductSuccess, getAllCategories, getAllCategoriesSuccess, getAllProducts, getAllProductsSuccess, getCategoryById, getCategoryByIdSuccess, getProductByIdSuccess, increaseStockPerProductSucess, updateCategoryByIdSuccess, updateProductByIdSucess } from './catalog.actions';
import { CategoryResponse } from './models/categoryResponse.model';
import { ProductResponse } from './models/productResponse.model';

export interface CatalogState{
    AllCategories: CategoryResponse[];
    ActualCategory: CategoryResponse;
    AllProducts: ProductResponse[];
    ActualProduct: ProductResponse;
    loading: boolean;
    catalogError: string;
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
                    stock: 0},
    loading: false,
    catalogError: ''
}

export const catalogReducer = createReducer(
  initialState,
  on(addProduct, (state) => (
    { ...state, 
      loading: true })),
  on(addProductSuccess, (state, { product }) => (
    { ...state,
      products: [...state.AllProducts, product], 
      catalogError: '',
      loading: false })),
  on(addProductFailure, (state, { error }) => (
    { ...state, 
      loading: false, 
      catalogError: error })),
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