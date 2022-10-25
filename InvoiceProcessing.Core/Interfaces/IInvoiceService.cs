using InvoiceProcessing.Core.Models;

namespace InvoiceProcessing.Core.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceDetail> ProcessInvoices(int userId);
}
