import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/paged-result.model';
import { Product } from '../models/product.model';

export interface CreateProductRequest {
  name: string;
  sku: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5082/api/products';

  getProducts(
    search?: string,
    active?: boolean | null,
    page: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<Product>> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (search && search.trim()) {
      params = params.set('search', search.trim());
    }

    if (active !== null && active !== undefined) {
      params = params.set('active', active);
    }

    return this.http.get<PagedResult<Product>>(this.apiUrl, { params });
  }

  createProduct(request: CreateProductRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.apiUrl, request);
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  updateProduct(id: string, request: CreateProductRequest & { active: boolean }): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  activateProduct(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/activate`, {});
  }
}
