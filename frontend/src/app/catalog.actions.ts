import { createAction, props } from '@ngrx/store';
import { Category } from './models/category.model';
import { CategoryResponse } from './models/categoryResponse.model';
import { Product } from './models/product.model';
import { ProductResponse } from './models/productResponse.model';

export const addProduct = createAction(
  '[Catalog] Add Product',
  props<{ product: Product;}>()
);
export const addProductSuccess = createAction(
  '[Catalog] Add Product Success',
  props<{ product: ProductResponse;}>()
);
export const addProductFailure = createAction(
  '[Catalog] Add Product Failure',
  props<{ error: any;}>()
);
      
export const getProductById = createAction(
  '[Catalog] Get Product By Id',
  props<{ productId: string;}>()
);
export const getProductByIdSuccess = createAction(
  '[Catalog] Get Product By Id Success',
  props<{ product: ProductResponse;}>()
);
export const getProductByIdFailure = createAction(
  '[Catalog] Get Product By Id Failure',
  props<{ error: any;}>()
);
    
export const getAllProducts = createAction(
  '[Catalog] Get All Products'
);
export const getAllProductsSuccess = createAction(
  '[Catalog] Get All Products Success',
  props<{ products: ProductResponse[];}>()
);
export const getAllProductsFailure = createAction(
  '[Catalog] Get All Products Failure',
  props<{ error: any;}>()
);

export const updateProductById = createAction(
  '[Catalog] Update Product',
  props<{ id: string, product: Product;}>()
);
export const updateProductByIdSucess = createAction(
  '[Catalog] Update Product Success',
  props<{ product: ProductResponse;}>()
);
export const updateProductByIdFailure = createAction(
  '[Catalog] Update Product Failure',
  props<{ error: any;}>()
);  

export const deleteProductById = createAction(
  '[Catalog] Delete Product',
  props<{ id: string;}>()
);
export const deleteProductByIdSucess = createAction(
  '[Catalog] Delete Product Success'
);
export const deleteProductByIdFailure = createAction(
  '[Catalog] Delete Product Failure',
  props<{ error: any;}>()
);

export const increaseStockPerProduct = createAction(
  '[Catalog] Increase stock per Product',
  props<{ id: string, quantity: number;}>()
);
export const increaseStockPerProductSucess = createAction(
  '[Catalog] Increase stock per Product Success',
  props<{ product: ProductResponse;}>()
);
export const increaseStockPerProductFailure = createAction(
  '[Catalog] Increase stock per Product Failure',
  props<{ error: any;}>()
);
  
  

export const addCategory = createAction(
  '[Catalog] Add Category',
  props<{category: Category}>()
);
export const addCategorySuccess = createAction(
  '[Catalog] Add Category Success',
  props<{category: CategoryResponse}>()
);
export const addCategoryFailure = createAction(
  '[Catalog] Add Category Failure',
  props<{ error: any;}>()
);   

export const getCategoryById = createAction(
  '[Catalog] Get Category By Id',
  props<{id: string}>()
);
export const getCategoryByIdSuccess = createAction(
  '[Catalog] Get Category By Id Success',
  props<{category: CategoryResponse}>()
);
export const getCategoryByIdFailure = createAction(
  '[Catalog] Get Category By Id Failure',
  props<{error: any}>()
);

export const getAllCategories = createAction(
  '[Catalog] Get All Categories'
);
export const getAllCategoriesSuccess = createAction(
  '[Catalog] Get All Categories Success',
  props<{ categories: CategoryResponse[];}>()
);
export const getAllCategoriesFailure = createAction(
  '[Catalog] Get All Categories Failure',
  props<{error: any}>()
);       
    
export const updateCategoryById = createAction(
  '[Catalog] Update Category',
  props<{id: string, category: Category}>()
);
export const updateCategoryByIdSuccess = createAction(
  '[Catalog] Update Category Success',
  props<{category: CategoryResponse}>()
);
export const updateCategoryByIdFailure = createAction(
  '[Catalog] Update Category Failure',
  props<{error: any}>()
);     

export const deleteCategoryById = createAction(
  '[Catalog] Delete Category',
  props<{id: string}>()
);
export const deleteCategoryByIdSuccess = createAction(
  '[Catalog] Delete Category Success'
);
export const deleteCategoryByIdFailure = createAction(
  '[Catalog] Delete Category Failure',
  props<{error: any}>()
);      
