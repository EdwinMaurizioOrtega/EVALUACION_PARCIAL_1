using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using System.Globalization;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Registro de servicios en el contenedor de inyección de dependencias.
// Se configura MVC con mensajes de validación en español.
// Fuentes:
// - https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
// - https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.modelbinding.metadata.modelbindingmessageprovider

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        fieldName => $"El campo {fieldName} debe ser un número válido. Ejemplo: 12.25");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        value => $"El valor '{value}' no es válido.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
        fieldName => $"El campo {fieldName} es obligatorio.");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var cultureInfo = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo),
    SupportedCultures = new[] { cultureInfo },
    SupportedUICultures = new[] { cultureInfo }
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
