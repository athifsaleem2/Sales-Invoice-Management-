using backend.Application.DTOs;
using backend.Core.Entities;
using backend.Core.Interfaces;

namespace backend.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;

    public InvoiceService(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<InvoiceDto> Invoices, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        var invoices = await _repository.GetAllAsync(pageNumber, pageSize);
        var totalCount = await _repository.GetTotalCountAsync();
        return (invoices.Select(MapToDto), totalCount);
    }

    public async Task<IEnumerable<InvoiceDto>> GetReportAsync(DateTime? date, string invoiceNumber)
    {
        var invoices = await _repository.GetReportAsync(date, invoiceNumber);
        return invoices.Select(MapToDto);
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice == null) return null;
        return MapToDto(invoice);
    }

    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto createDto)
    {
        var lastInvoiceNumber = await _repository.GetLastInvoiceNumberAsync();
        var nextNumber = GenerateNextInvoiceNumber(lastInvoiceNumber);

        var invoice = new Invoice
        {
            InvoiceNumber = nextNumber,
            CustomerName = createDto.CustomerName,
            Date = createDto.Date,
            Items = createDto.Items.Select(i => new InvoiceItem
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price,
                Total = i.Quantity * i.Price
            }).ToList()
        };

        invoice.TotalAmount = invoice.Items.Sum(i => i.Total);

        var created = await _repository.AddAsync(invoice);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, InvoiceUpdateDto updateDto)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice == null) return false;

        invoice.CustomerName = updateDto.CustomerName;
        invoice.Date = updateDto.Date;

        // Simplify updating items: clear existing and add new
        // For a more robust approach, we would compare by ID and update/delete/insert
        invoice.Items.Clear();
        
        foreach (var item in updateDto.Items)
        {
            invoice.Items.Add(new InvoiceItem
            {
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price,
                Total = item.Quantity * item.Price
            });
        }

        invoice.TotalAmount = invoice.Items.Sum(i => i.Total);

        await _repository.UpdateAsync(invoice);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice == null) return false;

        await _repository.DeleteAsync(invoice);
        return true;
    }

    private string GenerateNextInvoiceNumber(string lastInvoiceNumber)
    {
        string yearPrefix = DateTime.UtcNow.Year.ToString().Substring(2, 2);
        
        if (string.IsNullOrEmpty(lastInvoiceNumber) || !lastInvoiceNumber.StartsWith(yearPrefix + "INV"))
        {
            return $"{yearPrefix}INV0001";
        }

        var sequenceString = lastInvoiceNumber.Substring(5);
        if (int.TryParse(sequenceString, out int sequence))
        {
            return $"{yearPrefix}INV{(sequence + 1).ToString("D4")}";
        }

        return $"{yearPrefix}INV0001";
    }

    private InvoiceDto MapToDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            CustomerName = invoice.CustomerName,
            Date = invoice.Date,
            TotalAmount = invoice.TotalAmount,
            Items = invoice.Items.Select(i => new InvoiceItemDto
            {
                Id = i.Id,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price,
                Total = i.Total
            }).ToList()
        };
    }
}
