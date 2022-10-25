using InvoiceProcessing.Core.Interfaces;
using InvoiceProcessing.Core.Models;
using InvoiceProcessing.Core.Utils;

namespace InvoiceProcessing.Core.Services;

public class InvoiceService : IInvoiceService
{
    private IInvoiceHttpUtil _invoiceUtil;

    public InvoiceService(): this(new InvoiceHttpUtil()) {}
    
    public InvoiceService(IInvoiceHttpUtil invoiceHttpUtil) {
        _invoiceUtil = invoiceHttpUtil;
    }

    public async Task<InvoiceDetail> ProcessInvoices(int userId)
    {
        var invoices = await GetAllInvoices(userId);
        var invoiceDetail = new InvoiceDetail
        {
            TypeZero = invoices.Where(invoice => invoice.Type == 0)
                            .Select(ToJson)
                            .ToList(),
            TypeOne = invoices.Where(invoice => invoice.Type == 1)
                            .Select(ToXml)
                            .ToList()
        };
        if (invoiceDetail.TypeZero.Any())
        {
            invoiceDetail.StatusTypeZero = await _invoiceUtil.SendInvoices(userId, invoiceDetail.TypeZero);
        }
        if (invoiceDetail.TypeOne.Any())
        {
            invoiceDetail.StatusTypeOne = await _invoiceUtil.SendInvoices(userId, invoiceDetail.TypeOne);
        }
        return invoiceDetail;
    }

    private async Task<IEnumerable<Invoice>> GetAllInvoices(int userId)
    {
        var page = 0;
        var invoices = await _invoiceUtil.FetchInvoices(page);
        var totalInvoices = new List<Invoice>();
        while (invoices.Any())
        {
            totalInvoices.AddRange(invoices.Where(invoice => invoice.BusinessUserId == userId));
            invoices = await _invoiceUtil.FetchInvoices(++page);
        }
        return totalInvoices;
    }

    private InvoiceJson ToJson(Invoice invoice)
    {
        return new()
        {
            Id = invoice.TransactionId,
            Vrm = invoice.LicensePlate,
            EventStartDate = invoice.CheckInDate,
            EventEndDate = invoice.CheckOutDate,
            Price = invoice.Price,
            FacilityId = invoice.BusinessUserId
        };
    }

    private InvoiceXml ToXml(Invoice invoice)
    {
        return new()
        {
            UniqueId = invoice.TransactionId,
            RegistrationNumber = invoice.LicensePlate,
            DriveInDate = invoice.CheckInDate,
            DriveOutDate = invoice.CheckOutDate,
            Price = invoice.Price,
            CarparkId = invoice.BusinessUserId
        };
    }
}
