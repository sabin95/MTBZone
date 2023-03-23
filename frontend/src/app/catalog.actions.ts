import { createAction, props } from '@ngrx/store';
import { Category } from './models/category.model';
import { Product } from './models/product.model';

export const addProduct = createAction(
  '[Catalog] Add Product',
  props<{ product: Product;}>()
);
export const addProductSuccess = createAction(
  '[Catalog] Add Product Success',
  props<{ product: Product }>()
);
export const addProductFailure = createAction(
  '[Catalog] Add Product Failure',
  props<{ error: any }>()
);
      
export const getProductById = createAction(
  '[Catalog] Get Product By Id',
  props<{ id: string;}>()
);
export const getProductByIdSuccess = createAction(
  '[Catalog] Get Product By Id Success',
  props<{ product: Product }>()
);
export const getProductByIdFailure = createAction(
  '[Catalog] Get Product By Id Failure',
  props<{ error: any }>()
);
    
export const getAllProducts = createAction(
  '[Catalog] Get All Products'
);
export const getAllProductsSuccess = createAction(
  '[Catalog] Get All Products Success',
  props<{ products: Product[] }>()
);
export const getAllProductsFailure = createAction(
  '[Catalog] Get All Products Failure',
  props<{ error: any }>()
);

export const updateProductById = createAction(
    '[Catalog] Update Product',
    props<{ id: string, product: Product;}>()
  );
export const deleteById = createAction(
    '[Catalog] Delete Product',
    props<{ id: string;}>()
  );
export const increaseStockPerProduct = createAction(
    '[Catalog] Increase stock per Product',
    props<{ id: string, quantity: number;}>()
  );

export const addCategory = createAction(
    '[Catalog] Add Category',
    props<{category: Category}>());
export const getCategoryById = createAction(
    '[Catalog] Get Category By Id',
    props<{id: string}>());
export const getAllCategories = createAction(
    '[Catalog] Get All Categories'
    );
export const updateCategoryById = createAction(
    '[Catalog] Update Category',
    props<{id: string, category: Category}>());
export const deleteCategory = createAction(
    '[Catalog] Delete Category',
    props<{id: string}>());
