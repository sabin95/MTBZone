import { createReducer, on } from '@ngrx/store';
import { addProduct, getProductById, getAllProducts, updateProductById, deleteById, increaseStockPerProduct } from './catalog.actions';
import { Category } from './models/category.model';
import { Product } from './models/product.model';

export interface State{
    AvailableCategories: Category[];
    AllProducts: Product[];
    AvailableProducts: Product[];
    OutOfStockProducts: Product[];
}

export const initialState: State = {
    AvailableCategories: [],
    AllProducts: [],
    AvailableProducts: [],
    OutOfStockProducts: []
}

export const catalogReducer = createReducer(
  initialState,
  on(addProduct, state => ({
    ...state,
    AllProduct: state.AllProducts
  })),
);