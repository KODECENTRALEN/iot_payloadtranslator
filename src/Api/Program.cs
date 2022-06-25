using Bluefragments.Utilities.Extensions;
using Newtonsoft.Json;
using Attributes;
using Entities;
using Handlers;
using Helpers;

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

app.MapPost("/", (PayloadRequest payloadRequest) =>
{
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
    if (time == 0)
    {
        return Results.BadRequest("time is not specified correctly");
    }

    long now = DateTime.Now.ToEpochTimeSeconds();
    if (time > now)
    {
        long timeInSeconds = time / 1000;
        if (timeInSeconds > now)
        {
            return Results.BadRequest("time is not specified correctly");
        }

        time = timeInSeconds;
    }

    IHandler handler = DeviceHelper.FindHandlerForDeviceType(deviceType, attributes);
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

    return Results.Ok(payloadRequest);
})
.WithName("Make request from payload");

app.Run();
