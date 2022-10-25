namespace InvoiceProcessing.Core.Models;

public class InvoiceXml
{
    public int UniqueId { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? DriveInDate { get; set; }
    public string? DriveOutDate { get; set; }
    public int Price { get; set; }
    public int CarparkId { get; set; }
}
