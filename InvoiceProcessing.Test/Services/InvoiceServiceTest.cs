using InvoiceProcessing.Core.Interfaces;
using InvoiceProcessing.Core.Models;
using InvoiceProcessing.Core.Services;
using Moq;

namespace InvoiceProcessing.Test.Services;

public class InvoiceServiceTest
{
    private IInvoiceService _service;
    private Mock<IInvoiceHttpUtil> _mockClient;

    public InvoiceServiceTest()
    {
        _mockClient = new();
        _service = new InvoiceService(_mockClient.Object);
    }

    [Fact]
    public async Task ProcessInvoices_WithNewUser_ReturnsEmptyDetail()
    {
        // Arrange
        _mockClient.Setup(client => client.FetchInvoices(0))
                .ReturnsAsync(new List<Invoice>());

        // Act
        var result = await _service.ProcessInvoices(8);

        // Assert
        Assert.Empty(result.TypeZero);
        Assert.Empty(result.TypeOne);
        Assert.Null(result.StatusTypeZero);
        Assert.Null(result.StatusTypeOne);
        _mockClient.Verify(client => client.FetchInvoices(0), Times.Once);
        _mockClient.Verify(client => client.SendInvoices(8, It.IsAny<IEnumerable<InvoiceJson>>()), Times.Never);
        _mockClient.Verify(client => client.SendInvoices(8, It.IsAny<IEnumerable<InvoiceXml>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessInvoices_WithValidUser_ReturnsFullDetail()
    {
        // Arrange
        _mockClient.Setup(client => client.FetchInvoices(0))
                .ReturnsAsync(new List<Invoice>{
                    new Invoice{ BusinessUserId = 18, TransactionId = 5188, Type = 1 },
                    new Invoice{ BusinessUserId = 34, TransactionId = 3331, Type = 0 },
                    new Invoice{ BusinessUserId = 18, TransactionId = 4332, Type = 1 }
                });
        _mockClient.Setup(client => client.FetchInvoices(1))
                .ReturnsAsync(new List<Invoice>());
        _mockClient.Setup(client => client.SendInvoices(18, It.IsAny<IEnumerable<InvoiceXml>>()))
                .ReturnsAsync("Y6KqXJU");

        // Act
        var result = await _service.ProcessInvoices(18);

        // Assert
        Assert.Empty(result.TypeZero);
        Assert.Null(result.StatusTypeZero);
        Assert.Equal("Y6KqXJU", result.StatusTypeOne);
        Assert.Equal(2, result.TypeOne?.Count());
        _mockClient.Verify(client => client.FetchInvoices(It.IsAny<int>()), Times.Exactly(2));
        _mockClient.Verify(client => client.SendInvoices(18, It.IsAny<IEnumerable<InvoiceJson>>()), Times.Never);
        _mockClient.Verify(client => client.SendInvoices(18, It.IsAny<IEnumerable<InvoiceXml>>()), Times.Once);
    }

    [Fact]
    public async Task ProcessInvoices_WithInvalidUser_ReturnsEmptyDetail()
    {
        // Arrange
        _mockClient.Setup(client => client.FetchInvoices(0))
                .ReturnsAsync(new List<Invoice>());

        // Act
        var result = await _service.ProcessInvoices(-156);

        // Assert
        Assert.Empty(result.TypeZero);
        Assert.Empty(result.TypeOne);
        Assert.Null(result.StatusTypeZero);
        Assert.Null(result.StatusTypeOne);

        _mockClient.Verify(client => client.FetchInvoices(0), Times.Once);
        _mockClient.Verify(client => client.SendInvoices(-156, It.IsAny<IEnumerable<InvoiceJson>>()), Times.Never);
        _mockClient.Verify(client => client.SendInvoices(-156, It.IsAny<IEnumerable<InvoiceXml>>()), Times.Never);
    }
}