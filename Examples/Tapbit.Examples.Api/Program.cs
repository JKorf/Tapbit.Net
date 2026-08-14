using Tapbit.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the Tapbit services
builder.Services.AddTapbit();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddTapbit(options =>
{
    options.ApiCredentials = new ApiCredentials("<APIKEY>", "<APISECRET>");
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoints and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] ITapbitRestClient client, string symbol) =>
{
    var result = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);
    return result.Success
        ? Results.Ok(result.Data.LastPrice)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.MapGet("/Balances", async ([FromServices] ITapbitRestClient client) =>
{
    var result = await client.SpotApi.Account.GetAccountInfoAsync();
    return result.Success
        ? Results.Ok(result.Data.Balances)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.Run();