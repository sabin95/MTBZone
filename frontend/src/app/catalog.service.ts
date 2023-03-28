import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from './models/product.model';
import { deleteProductById } from './catalog.actions';

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

  updateProduct(id:string, product: Product): Observable<any> {
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
}
