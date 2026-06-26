using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Extensions.Logging;
using NXO.Functions.Services;

namespace NXO.Functions.Services;

public class CaseCreator
{
    private readonly ILogger<CaseCreator> _logger;
    private readonly ServiceClient _dataverseClient;

    public CaseCreator(ILogger<CaseCreator> logger)
    {
        _logger = logger;
        var d365Url = Environment.GetEnvironmentVariable("D365_URL")!;
        var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")!;
        var clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET")!;
        var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID")!;

        var connectionString = $"AuthType=ClientSecret;Url={d365Url};ClientId={clientId};ClientSecret={clientSecret};TenantId={tenantId}";
        _dataverseClient = new ServiceClient(connectionString);
    }

    public async Task<Guid> CreateCaseAsync(
        string subject,
        string description,
        string senderEmail,
        ClassificationResult classification)
    {
        // IsReady check — sikrer forbindelsen til D365 er klar
        if (!_dataverseClient.IsReady)
        {
            _logger.LogError("Dataverse client ikke klar: {LastError}", _dataverseClient.LastError);
            return Guid.Empty;
        }

        try
        {
            var incident = new Entity("incident");

            // Standard felter
            incident["title"] = subject;
            incident["description"] = description;
            incident["caseorigincode"] = new OptionSetValue(2); // Email = 2

            // TODO: Tilføj customer reference fra CustomerResolver (Sprint 3)
            // incident["customerid"] = new EntityReference("contact", customerId);

            // AI custom felter
            incident["new_aikategori"]     = classification.Category;
            incident["new_aiprioritet"]    = classification.Priority;
            incident["new_aisentiment"]    = classification.Sentiment;
            incident["new_aikompleksitet"] = classification.Complexity;
            incident["new_aiconfidence"]   = (double)classification.Confidence;
            incident["new_aisammenfatning"] = classification.Summary;
            incident["new_afsenderemail"]  = senderEmail;
            incident["new_kanal"]          = "Email"; // TODO: Ændre til nxo_ prefix hvis din solution bruger det

            // Prioritet mapping til D365 standard prioritet
            incident["prioritycode"] = classification.Priority switch
            {
                "Critical" => new OptionSetValue(1), // High
                "High"     => new OptionSetValue(1), // High
                "Medium"   => new OptionSetValue(2), // Normal
                "Low"      => new OptionSetValue(3), // Low
                _          => new OptionSetValue(2)  // Normal
            };

            // Create er allerede thread-safe, Task.Run ikke nødvendig
            var caseId = _dataverseClient.Create(incident);

            _logger.LogInformation("Case oprettet i D365: {CaseId} — {Subject} / {Category} / {Priority}",
                caseId, subject, classification.Category, classification.Priority);

            return await Task.FromResult(caseId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved oprettelse af Case i D365");
            return Guid.Empty;
        }
    }
}