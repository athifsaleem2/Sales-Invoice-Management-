import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';
import { Invoice, InvoiceItem } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './invoice-form.component.html',
  styleUrl: './invoice-form.component.css'
})
export class InvoiceFormComponent implements OnInit {
  invoiceForm: FormGroup;
  isEditMode: boolean = false;
  invoiceId: number | null = null;
  loading: boolean = false;
  submitSuccess: boolean = false;
  submitError: string = '';

  constructor(
    private fb: FormBuilder,
    private invoiceService: InvoiceService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.invoiceForm = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(200)]],
      date: [new Date().toISOString().substring(0, 10), [Validators.required]],
      items: this.fb.array([], Validators.minLength(1))
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idStr = params.get('id');
      if (idStr) {
        this.isEditMode = true;
        this.invoiceId = +idStr;
        this.loadInvoice(this.invoiceId);
      } else {
        this.addItem(); // Add one empty item by default
      }
    });

    // Auto-calculation listener
    this.items.valueChanges.subscribe(() => {
      // The getter for totalAmount will naturally reflect changes if we use it in the template,
      // but if we want to store it, we could do it here.
    });
  }

  loadInvoice(id: number): void {
    this.loading = true;
    this.invoiceService.getById(id).subscribe({
      next: (invoice) => {
        this.invoiceForm.patchValue({
          customerName: invoice.customerName,
          date: new Date(invoice.date).toISOString().substring(0, 10)
        });

        this.items.clear();
        invoice.items.forEach(item => {
          this.items.push(this.fb.group({
            id: [item.id],
            productName: [item.productName, [Validators.required]],
            quantity: [item.quantity, [Validators.required, Validators.min(0.01)]],
            price: [item.price, [Validators.required, Validators.min(0.01)]]
          }));
        });
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load invoice', err);
        this.submitError = 'Failed to load invoice details.';
        this.loading = false;
      }
    });
  }

  get items(): FormArray {
    return this.invoiceForm.get('items') as FormArray;
  }

  addItem(): void {
    this.items.push(this.fb.group({
      id: [null],
      productName: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(0.01)]],
      price: [0, [Validators.required, Validators.min(0.01)]]
    }));
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  calculateItemTotal(index: number): number {
    const item = this.items.at(index).value;
    return (item.quantity || 0) * (item.price || 0);
  }

  get grandTotal(): number {
    let total = 0;
    for (let i = 0; i < this.items.length; i++) {
      total += this.calculateItemTotal(i);
    }
    return total;
  }

  onSubmit(): void {
    if (this.invoiceForm.invalid) {
      this.invoiceForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.submitError = '';
    
    const formValue = this.invoiceForm.value;

    if (this.isEditMode && this.invoiceId) {
      this.invoiceService.update(this.invoiceId, formValue).subscribe({
        next: () => {
          this.submitSuccess = true;
          setTimeout(() => this.router.navigate(['/invoices']), 1500);
        },
        error: (err) => {
          this.submitError = 'Failed to update invoice.';
          this.loading = false;
        }
      });
    } else {
      this.invoiceService.create(formValue).subscribe({
        next: () => {
          this.submitSuccess = true;
          setTimeout(() => this.router.navigate(['/invoices']), 1500);
        },
        error: (err) => {
          this.submitError = 'Failed to create invoice.';
          this.loading = false;
        }
      });
    }
  }
}
