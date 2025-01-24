global using Microsoft.EntityFrameworkCore;
global using RegistroTecnico.Services;
global using RegistroTecnico.Context;
using Blazored.Toast;
using RegistroTecnico.Components;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);

// Registrar Razor Pages y controladores
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Registrar Razor Components interactivos
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar el contexto de base de datos
builder.Services.AddDbContextFactory<TecnicoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConStr"));
});

// Registrar servicios personalizados
builder.Services.AddScoped<TecnicoServices>();
builder.Services.AddScoped<ClienteServices>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazoredToast();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();