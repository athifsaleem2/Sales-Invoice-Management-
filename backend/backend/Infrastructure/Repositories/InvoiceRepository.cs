using backend.Core.Entities;
using backend.Core.Interfaces;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Invoices
            .OrderByDescending(i => i.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Invoices.CountAsync();
    }

    public async Task<IEnumerable<Invoice>> GetReportAsync(DateTime? date, string invoiceNumber)
    {
        var query = _context.Invoices.AsQueryable();

        if (date.HasValue)
        {
            query = query.Where(i => i.Date.Date == date.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(invoiceNumber))
        {
            query = query.Where(i => i.InvoiceNumber.Contains(invoiceNumber));
        }

        return await query.OrderByDescending(i => i.Date).ToListAsync();
    }

    public async Task<Invoice?> GetByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Invoice> AddAsync(Invoice invoice)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Invoice invoice)
    {
        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetLastInvoiceNumberAsync()
    {
        var lastInvoice = await _context.Invoices
            .OrderByDescending(i => i.Id)
            .FirstOrDefaultAsync();
            
        return lastInvoice?.InvoiceNumber ?? string.Empty;
    }
}
