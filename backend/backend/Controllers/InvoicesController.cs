using backend.Application.DTOs;
using backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var (invoices, totalCount) = await _invoiceService.GetAllAsync(pageNumber, pageSize);
        return Ok(new { Data = invoices, TotalCount = totalCount });
    }

    [HttpGet("report")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetReport([FromQuery] DateTime? date, [FromQuery] string? invoiceNumber)
    {
        var invoices = await _invoiceService.GetReportAsync(date, invoiceNumber ?? string.Empty);
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetById(int id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice == null) return NotFound();
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] InvoiceCreateDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var created = await _invoiceService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] InvoiceUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _invoiceService.UpdateAsync(id, updateDto);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _invoiceService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
