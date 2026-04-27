import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { InvoiceService } from '../../services/invoice.service';
import { Invoice } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-report',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './invoice-report.component.html',
  styleUrl: './invoice-report.component.css'
})
export class InvoiceReportComponent implements OnInit {
  invoices: Invoice[] = [];
  loading: boolean = false;
  currentDate = new Date();
  
  filterDate: string = '';
  filterInvoiceNumber: string = '';

  constructor(private invoiceService: InvoiceService) {}

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loading = true;
    this.invoiceService.getReport(this.filterDate, this.filterInvoiceNumber).subscribe({
      next: (data) => {
        this.invoices = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading report', err);
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.loadReport();
  }

  clearFilters(): void {
    this.filterDate = '';
    this.filterInvoiceNumber = '';
    this.loadReport();
  }

  get totalReportAmount(): number {
    return this.invoices.reduce((sum, inv) => sum + inv.totalAmount, 0);
  }

  printReport(): void {
    window.print();
  }
}
