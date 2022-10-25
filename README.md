# Invoice Processing

It is an demo library that will allow you to obtain pending invoices to be evaluated and processed on a required server.

# For Developers
## Prerequisites
- [Net Core](https://dotnet.microsoft.com/download)

## Running Test
Restore dependencies:
```
dotnet restore
```
Move to **/InvoiceProcessing.Test** directory:
```
cd InvoiceProcessing.Test
```
To run the tests
```
dotnet test
```

## Add Package
To add the core module to your custom products, move **/InvoiceProcessing.Core** to your preferred directory and run the following command:
```
dotnet sln "Your solution.sln" add "New InvoiceProcessing.Core Directory\InvoiceProcessing.Core.csproj"
```
Add the following variables to your file **appsettings.json**
```
{
    "FetchUrl": "https://** Your site **",
    "SendUrl": "https://** Your site **"
}
```
