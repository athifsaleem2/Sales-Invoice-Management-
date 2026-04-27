using backend.Core.Entities;

namespace backend.Core.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Invoice>> GetReportAsync(DateTime? date, string invoiceNumber);
    Task<Invoice?> GetByIdAsync(int id);
    Task<Invoice> AddAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
    Task<string> GetLastInvoiceNumberAsync();
}
