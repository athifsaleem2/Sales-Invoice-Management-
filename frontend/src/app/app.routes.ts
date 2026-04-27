import { Routes } from '@angular/router';
import { InvoiceListComponent } from './components/invoice-list/invoice-list.component';
import { InvoiceFormComponent } from './components/invoice-form/invoice-form.component';
import { InvoiceReportComponent } from './components/invoice-report/invoice-report.component';

export const routes: Routes = [
    { path: '', redirectTo: 'invoices', pathMatch: 'full' },
    { path: 'invoices', component: InvoiceListComponent },
    { path: 'invoices/create', component: InvoiceFormComponent },
    { path: 'invoices/edit/:id', component: InvoiceFormComponent },
    { path: 'report', component: InvoiceReportComponent }
];
