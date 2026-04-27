export interface Invoice {
    id: number;
    invoiceNumber: string;
    customerName: string;
    date: string | Date;
    totalAmount: number;
    items: InvoiceItem[];
}

export interface InvoiceItem {
    id: number;
    productName: string;
    quantity: number;
    price: number;
    total: number;
}

export interface InvoiceCreate {
    customerName: string;
    date: string | Date;
    items: InvoiceItemCreate[];
}

export interface InvoiceItemCreate {
    productName: string;
    quantity: number;
    price: number;
}

export interface InvoiceUpdate {
    customerName: string;
    date: string | Date;
    items: InvoiceItemUpdate[];
}

export interface InvoiceItemUpdate {
    id?: number | null;
    productName: string;
    quantity: number;
    price: number;
}

export interface PaginatedResult<T> {
    data: T[];
    totalCount: number;
}
