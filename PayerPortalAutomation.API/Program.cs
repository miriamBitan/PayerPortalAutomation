using Microsoft.Playwright;
using PayerPortalAutomation.Application.Interfaces;
using PayerPortalAutomation.Application.Repositories;
using PayerPortalAutomation.Automation.Factories;
using PayerPortalAutomation.Automation.Portals;
using PayerPortalAutomation.Infrastructure.Repositories;
using PayerPortalAutomation.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// --- Swagger Start ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// --- Swagger End ---

builder.Services.AddOpenApi();
builder.Services.AddScoped<IPayerAutomationService, PayerAutomationService>();
builder.Services.AddScoped<IPortalAutomation, DemoPortalAutomation>();
builder.Services.AddScoped<IPayerTargetRepository, PayerTargetCsvRepository>();
builder.Services.AddScoped<IArtifactStorage, FileArtifactStorage>();
builder.Services.AddScoped<PortalAutomationFactory>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payer Portal Automation API V1");
        c.RoutePrefix = ""; // Swagger UI יהיה ב-root
    });

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();