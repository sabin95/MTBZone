import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from './models/product.model';
import { Category } from './models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private apiUrl = 'https://localhost:7124/api/';

  constructor(private http: HttpClient) { }

  addProduct(product: Product): Observable<any> {
    const url = `${this.apiUrl}Products/Add`;
    return this.http.post(url, product);
  }

  getProductById(id: string): Observable<any> {
    const url = `${this.apiUrl}Products/${id}`;
    return this.http.get(url);
  }

  getAllProducts(): Observable<any> {
    const url = `${this.apiUrl}Products`;
    return this.http.get(url);
  }

  updateProduct(id: string, product: Product): Observable<any> {
    const url = `${this.apiUrl}Products/${id}`;
    return this.http.put(url, product);
  }

  deleteProductById(id: string): Observable<any> {
    const url = `${this.apiUrl}Products/${id}`;
    return this.http.delete(url);
  }

  increaseStockPerProduct(id: string, quantity: number): Observable<any> {
    const url = `${this.apiUrl}Products/${id}/IncreaseStock`;
    return this.http.put(url, quantity);
  }

  addCategory(category: Category): Observable<any> {
    const url = `${this.apiUrl}Categories/Add`;
    return this.http.post(url, category);
  }

  getCategoryById(id: string): Observable<any> {
    const url = `${this.apiUrl}Categories/${id}`;
    return this.http.get(url);
  }

  getAllCategories(): Observable<any> {
    const url = `${this.apiUrl}Categories`;
    return this.http.get(url);
  }

  updateCategory(id: string, category: Category): Observable<any> {
    const url = `${this.apiUrl}Categories/${id}`;
    return this.http.put(url, category);
  }

  deleteCategoryById(id: string): Observable<any> {
    const url = `${this.apiUrl}Categories/${id}`;
    return this.http.delete(url);
  }
}
