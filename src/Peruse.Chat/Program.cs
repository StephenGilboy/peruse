using Azure.AI.OpenAI;
using Peruse.Chat;
using Peruse.Chat.ChatFeature;
using Peruse.Chat.Components;
using Refit;

var builder = WebApplication.CreateBuilder(args);

var openAIKey = builder.Configuration.GetValue<string>("OPEN_AI_KEY");
if (string.IsNullOrWhiteSpace(openAIKey))
    throw new Exception("OPEN_AI_KEY is not set");

builder.Services.AddSingleton(new OpenAIClient(openAIKey));

builder.Services.AddRefitClient<IPeruseApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7138"));

builder.Services.AddSingleton<ChatContext>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();