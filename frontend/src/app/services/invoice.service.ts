import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Invoice, InvoiceCreate, InvoiceUpdate, PaginatedResult } from '../models/invoice.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private apiUrl = 'https://localhost:7271/api/invoices';

  constructor(private http: HttpClient) { }

  getAll(pageNumber: number = 1, pageSize: number = 10): Observable<PaginatedResult<Invoice>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
      
    return this.http.get<PaginatedResult<Invoice>>(this.apiUrl, { params });
  }

  getReport(date?: string, invoiceNumber?: string): Observable<Invoice[]> {
    let params = new HttpParams();
    if (date) params = params.set('date', date);
    if (invoiceNumber) params = params.set('invoiceNumber', invoiceNumber);
      
    return this.http.get<Invoice[]>(`${this.apiUrl}/report`, { params });
  }

  getById(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiUrl}/${id}`);
  }

  create(invoice: InvoiceCreate): Observable<Invoice> {
    return this.http.post<Invoice>(this.apiUrl, invoice);
  }

  update(id: number, invoice: InvoiceUpdate): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, invoice);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
