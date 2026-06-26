using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NXO.Functions.Services;
using OpenTelemetry;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Registrer services
builder.Services.AddSingleton<EmailClassifier>();

// TODO: Tilføj Azure Monitor connection string for produktion
// builder.Services.AddOpenTelemetry()
//     .UseFunctionsWorkerDefaults()
//     .UseAzureMonitor Exporter();

builder.Build().Run();
