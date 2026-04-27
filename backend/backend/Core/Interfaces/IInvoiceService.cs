using backend.Application.DTOs;

namespace backend.Core.Interfaces;

public interface IInvoiceService
{
    Task<(IEnumerable<InvoiceDto> Invoices, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<InvoiceDto>> GetReportAsync(DateTime? date, string invoiceNumber);
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<InvoiceDto> CreateAsync(InvoiceCreateDto createDto);
    Task<bool> UpdateAsync(int id, InvoiceUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}
