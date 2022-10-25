using InvoiceProcessing.Core.Models;

namespace InvoiceProcessing.Core.Interfaces;

public interface IInvoiceHttpUtil
{
    Task<IEnumerable<Invoice>> FetchInvoices(int page);
    Task<string?> SendInvoices(int userId, IEnumerable<InvoiceJson> invoices);
    Task<string?> SendInvoices(int userId, IEnumerable<InvoiceXml> invoices);
}
