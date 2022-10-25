namespace InvoiceProcessing.Core.Models;

public class Invoice
{
    public int TransactionId { get; set; }
    public string? LicensePlate { get; set; }
    public string? CheckInDate { get; set; }
    public string? CheckOutDate { get; set; }
    public int Price { get; set; }
    public int BusinessUserId { get; set; }
    public int Type { get; set; }
}
