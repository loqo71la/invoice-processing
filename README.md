# Invoice Processing
![Unit Test](https://github.com/loqo71la/invoice-processing/actions/workflows/coverage-analysis.yml/badge.svg)
[![codecov](https://codecov.io/gh/loqo71la/invoice-processing/branch/main/graph/badge.svg?token=23X64UH813)](https://codecov.io/gh/loqo71la/invoice-processing)

It is an demo library that will allow you to obtain pending invoices to be evaluated and processed on a required server.

# For Developers
## Prerequisites
- [Net Core](https://dotnet.microsoft.com/download)



## Web Client
Move to **/InvoiceProcessing.Web** directory:
```
cd InvoiceProcessing.Web
```
Restore dependencies:
```
dotnet restore
```
Add the following variables to your file **appsettings.json**
```
"FetchUrl": "https://** Your site **",
"SendUrl": "https://** Your site **"
```
To run the Web client
```
dotnet run
```
At this time, you have a service running at https://localhost:7154/

![InvoiceProcessing](InvoiceProcessing.png)

## Running Test
Move to **/InvoiceProcessing.Test** directory:
```
cd InvoiceProcessing.Test
```
To run the tests
```
dotnet test
```

## Add it as custom Package
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
