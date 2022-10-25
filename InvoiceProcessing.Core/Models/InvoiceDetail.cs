namespace InvoiceProcessing.Core.Models;

public class InvoiceDetail
{
    public IEnumerable<InvoiceJson>? TypeZero { get; set; }
    public IEnumerable<InvoiceXml>? TypeOne { get; set; }
    public string? StatusTypeZero { get; set; }
    public string? StatusTypeOne { get; set; }
}
