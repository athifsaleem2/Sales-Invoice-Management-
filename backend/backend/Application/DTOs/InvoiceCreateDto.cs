using System.ComponentModel.DataAnnotations;

namespace backend.Application.DTOs;

public class InvoiceCreateDto
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "At least one item is required.")]
    public List<InvoiceItemCreateDto> Items { get; set; } = new();
}

public class InvoiceItemCreateDto
{
    [Required]
    public string ProductName { get; set; } = string.Empty;
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public decimal Quantity { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }
}
