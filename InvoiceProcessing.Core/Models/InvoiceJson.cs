namespace InvoiceProcessing.Core.Models;

public class InvoiceJson
{
    public int Id { get; set; }
    public string? Vrm { get; set; }
    public string? EventStartDate { get; set; }
    public string? EventEndDate { get; set; }
    public int Price { get; set; }
    public int FacilityId { get; set; }
}
