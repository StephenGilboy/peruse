using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peruse.Api.Requests;
using Peruse.Api.Wttw.Commands;
using Peruse.Api.Wttw.PlaywrightWindow;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(logBuilder =>
{
    logBuilder.AddConsole();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetEntryAssembly()!);
});

builder.Services.AddSingleton<PlaywrightWindowContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/browser/open", async () =>
{
    var mediator = app.Services.GetRequiredService<IMediator>();
    var result = await mediator.Send(new StartSession());
    return result.IsSuccess
        ? Results.Ok(new { sessionId = result.Value.SessionId })
        : Results.BadRequest(result.Error.Message);
}).Produces<JsonResult>();

app.MapPost("/api/browser/navigate", async (NavigateToPageRequest model) =>
{
    var mediator = app.Services.GetRequiredService<IMediator>();
    var result = await mediator.Send(new NavigateToPage(model.SessionId, model.Url));
    if(result.IsFailure)
        return Results.BadRequest(result.Error.Message);
    
    return Results.Ok(new { sessionId = model.SessionId, content = result.Value.Html, statusCode = result.Value.StatusCode, statusDescription = result.Value.StatusDescription });
}).Produces<JsonResult>();


var playwrightWindowContext = app.Services.GetRequiredService<PlaywrightWindowContext>();
await playwrightWindowContext.Initialize();
app.Run();
