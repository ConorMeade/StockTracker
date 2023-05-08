# RedArris Code Test

This project was bootstrapped with Visual Studio to give the boilerplate code for building REST API endpoints. The endpoint `/GetStockData` will return historial stock data given the parameters `tickerSymbol`, `startDate`, and `fromDate` which are used to determine the daily returns for the date range specified (inclusive).

To fetch the data needed, I have used iexcloud API endpoints. This data is then formatted and pruned to suit the specifications of the desired output. More specifically, I am gathering the data with the `$"https://cloud.iexapis.com/stable/stock/{symbol}/chart/{rangeSlug}?token={tokenSecret}"; endpoint. With the parameters for that uri being:
- `symbol` -> the ticker symbol for a given stock (aapl, msft, tsla, etc.).
- `rangeSlug` -> the range of data we want to fetch given our date bounds (5 days, 1 month, 3 months, 6 months, 1 year, year to date).
- `token` -> our API token that gives authorization to execute the query and get the JSON data back.

## Dependencies
Most of what is needed will be automatically set up by building a new ASP.NET Core Web API project in Visual Studio.

* Docker Desktop
* Visual Studio
* C# .NET Core

#### NuGet packages
* Microsoft.AspNet.WebApi.Client
* Microsoft.VisualStudio.Azure.Containers.Tools.Targets
* Swashbuckle.AspNetCore

#### Frameworks
* Microsoft.ASPNetCore.App
* Microsoft.NETICore.App

## Running Query

In Visual Studio, using `Ctrl + F5` will boot up a docker container and open the Swagger UI spec where parameters can easily be passed into the endpoint.

if cURL is preferred, a sample cURL command, would look something like:

~~~
curl -X 'GET' \
  'https://localhost:49155/Stock?tickerSymbol=msft&startDate=2023-04-10&endDate=2023-04-21' \
  -H 'accept: text/plain' -k
~~~
This will return json data for Microsoft's daily returns from April 10, 2023 - April 21, 2023

`-k` is used to avoid errors for a missing SSL/TLS certificate when running the command outside of Visual Studio/Docker container.

I have attached a sample data file for what is being returned in this [sample data](RedArris/SampleData.json) file.

