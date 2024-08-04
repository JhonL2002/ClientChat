using ClientChat.Components;
using ClientChat.Responses;
using ClientChat.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ManageTokenService>();
builder.Services.AddScoped<AuthenticationStateProvider ,CustomAuthenticationStateProvider>();
builder.Services.AddAuthorization();

//HTTP client factory
builder.Services.AddHttpClient("BackendChat", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7210/api/account/");
});

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
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();
