namespace backend.Core.Entities;

public class InvoiceItem
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }

    public Invoice Invoice { get; set; } = null!;
}
