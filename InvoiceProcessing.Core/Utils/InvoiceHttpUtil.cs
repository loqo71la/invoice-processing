using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using InvoiceProcessing.Core.Interfaces;
using InvoiceProcessing.Core.Models;
using Microsoft.Extensions.Configuration;

namespace InvoiceProcessing.Core.Utils;

public class InvoiceHttpUtil : IInvoiceHttpUtil
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private static readonly XmlSerializerNamespaces XMLNamespace = new(new[] { XmlQualifiedName.Empty });
    private static readonly XmlWriterSettings XMLSettings = new() { OmitXmlDeclaration = true };
    private static readonly IConfigurationRoot configuration= new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    private static readonly HttpClient _httpClient = new();

    public async Task<IEnumerable<Invoice>> FetchInvoices(int page)
    {
        var response = await _httpClient.PostAsJsonAsync(configuration["FetchUrl"], new { page });
        var content = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<IEnumerable<Invoice>>(content, JsonOptions) ?? Enumerable.Empty<Invoice>();
    }

    public async Task<string?> SendInvoices(int userId, IEnumerable<InvoiceJson> invoices)
    {
        var invoiceJson = JsonSerializer.Serialize(new { Nonpaids = invoices }, JsonOptions);
        var body = new StringContent(invoiceJson, Encoding.UTF8, "application/json");
        return await SendInvoices(userId, body);
    }

    public async Task<string?> SendInvoices(int userId, IEnumerable<InvoiceXml> invoices)
    {
        var invoiceXml = SerializeXml(invoices);
        var body = new StringContent(invoiceXml, Encoding.UTF8, "application/xml");
        return await SendInvoices(userId, body);
    }

    private async Task<string?> SendInvoices(int userId, StringContent body)
    {
        var response = await _httpClient.PostAsync($"{configuration["SendUrl"]}/{userId}", body);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<string>(content);
    }

    private string SerializeXml(IEnumerable<InvoiceXml> invoices)
    {
        var root = new XmlRootAttribute("Invoices");
        using (var stream = new StringWriter())
        using (var writer = XmlWriter.Create(stream, XMLSettings))
        {
            var serializer = new XmlSerializer(invoices.GetType(), root);
            serializer.Serialize(writer, invoices, XMLNamespace);
            return Regex.Replace(stream.ToString(), "InvoiceXml", "Invoice");
        }
    }
}
