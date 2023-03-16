import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from './models/product.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private apiUrl = 'https://localhost:7124/api/';

  constructor(private http: HttpClient) { }

  addProduct(product: Product): Observable<any> {
    const url = `${this.apiUrl}/Products/Add`;
    return this.http.post(url, product);
  }

  getProductById(id: string): Observable<any> {
    const url = `${this.apiUrl}Products/GetProductById?id=${id}`;
    return this.http.get(url);
  }
}
