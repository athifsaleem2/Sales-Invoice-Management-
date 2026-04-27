import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';
import { Invoice, PaginatedResult } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './invoice-list.component.html',
  styleUrl: './invoice-list.component.css'
})
export class InvoiceListComponent implements OnInit {
  invoices: Invoice[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;
  loading: boolean = true;
  Math = Math;

  constructor(private invoiceService: InvoiceService) {}

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.loading = true;
    this.invoiceService.getAll(this.pageNumber, this.pageSize).subscribe({
      next: (result: PaginatedResult<Invoice>) => {
        this.invoices = result.data;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading invoices', err);
        this.loading = false;
      }
    });
  }

  deleteInvoice(id: number): void {
    if (confirm('Are you sure you want to delete this invoice?')) {
      this.invoiceService.delete(id).subscribe({
        next: () => {
          this.loadInvoices();
        },
        error: (err) => console.error('Error deleting invoice', err)
      });
    }
  }

  nextPage(): void {
    if (this.pageNumber * this.pageSize < this.totalCount) {
      this.pageNumber++;
      this.loadInvoices();
    }
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadInvoices();
    }
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
