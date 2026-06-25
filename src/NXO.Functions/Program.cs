using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// TODO: Tilføj Azure Monitor connection string for produktion
// builder.Services.AddOpenTelemetry()
//     .UseFunctionsWorkerDefaults()
//     .UseAzureMonitor Exporter();

builder.Build().Run();
