using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Handlers;
using PayloadTranslator.Helpers;
using Bluefragments.Utilities.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

IEnumerable<DeviceAttributeContainer> attributes = DeviceHelper.GetTypesWithDeviceAttributes();
string modelId = "dtmi:generic:generic;1";

app.MapPost("/", (PayloadRequest payloadRequest) =>
{
    payloadRequest.ModelId = modelId;

    string deviceType = payloadRequest.DeviceType;
    if (string.IsNullOrEmpty(deviceType))
    {
        return Results.BadRequest("devicetype is not specified");
    }

    string data = payloadRequest.Data;
    if (string.IsNullOrEmpty(data))
    {
        return Results.BadRequest("data is not specified");
    }

    long time = payloadRequest.Time;
    if (time == 0 || time > DateTime.Now.ToEpochTimeSeconds())
    {
        return Results.BadRequest("time is not specified correctly");
    }

    IHandler? handler = DeviceHelper.FindHandlerForDeviceType(deviceType, attributes);
    if (handler == null)
    {
        return Results.BadRequest("it was not possible to find a handler for the devicetype");
    }

    PayloadResponse payloadResponse = handler.HandlePayload(payloadRequest);
    if (payloadResponse == null)
    {
        return Results.BadRequest("payload translator failed to handle the data");
    }

    return Results.Ok(payloadResponse);
})
.WithName("Translate payload from request");

app.MapPost("/request", async (HttpRequest req) =>
{
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    var payload = JsonConvert.DeserializeObject<dynamic>(requestBody);

    PayloadRequest payloadRequest = DeviceHelper.DecodePayloadMessage(payload);
    if (payloadRequest == null)
    {
        return Results.BadRequest("it was not possible to create a request from the data");
    }

    payloadRequest.ModelId = modelId;

    return Results.Ok(payloadRequest);
})
.WithName("Make request from payload");

app.Run();
