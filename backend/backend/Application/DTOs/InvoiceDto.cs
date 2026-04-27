namespace backend.Application.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public List<InvoiceItemDto> Items { get; set; } = new();
}

public class InvoiceItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
}
