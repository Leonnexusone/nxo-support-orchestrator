using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using NXO.Functions.Services;

namespace NXO.Functions;

public class EmailTrigger
{
    // Logger bruges til at skrive beskeder til terminalen / Azure Monitor
    private readonly ILogger _logger;

    // GraphServiceClient er vores forbindelse til Microsoft Graph API
    private readonly GraphServiceClient _graphClient;

    // EmailClassifier bruges til AI-baseret email klassificering
    private readonly EmailClassifier _classifier;

    public EmailTrigger(ILoggerFactory loggerFactory, EmailClassifier classifier)
    {
        _logger = loggerFactory.CreateLogger<EmailTrigger>();        _classifier = classifier;
        // Hent credentials fra environment variables (kommer fra Key Vault senere)
        var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");
        var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET");

        // Opret forbindelse til Azure med App Registration credentials
        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        // Opret Graph klient med de nødvendige permissions
        _graphClient = new GraphServiceClient(credential);
    }

    // Denne funktion kører automatisk hvert 2. minut (til test - skift til */5 i produktion)
    [Function("EmailTrigger")]
    public async Task Run([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("EmailTrigger kørt: {executionTime}", DateTime.Now);

        try
        {
            // Hent emails fra indbakken
            // TODO: Skift "din@email.com" til environment variable senere
            var mailboxUser = Environment.GetEnvironmentVariable("GRAPH_MAILBOX");

            var messages = await _graphClient.Users[mailboxUser]
                .Messages
                .GetAsync(config =>
                {
                    // Hent kun ulæste emails
                    config.QueryParameters.Filter = "isRead eq false";
                    // Hent kun de felter vi har brug for
                    config.QueryParameters.Select = new[] { "subject", "from", "body", "receivedDateTime" };
                    // Maks 10 emails ad gangen
                    config.QueryParameters.Top = 10;
                });

            if (messages?.Value == null || messages.Value.Count == 0)
            {
                _logger.LogInformation("Ingen nye emails fundet.");
                return;
            }

            // Log og klassificer hver email vi finder
            foreach (var message in messages.Value)
            {
                _logger.LogInformation("Email fundet: Fra={from}, Emne={subject}, Modtaget={received}",
                    message.From?.EmailAddress?.Address,
                    message.Subject,
                    message.ReceivedDateTime);

                // Sprint 2: AI klassificering
                var classification = await _classifier.ClassifyEmailAsync(
                    message.Subject ?? "(intet emne)",
                    message.Body?.Content ?? ""
                );

                _logger.LogInformation("AI Klassificering: Kategori={category}, Prioritet={priority}, Sentiment={sentiment}, Kompleksitet={complexity}",
                    classification.Category,
                    classification.Priority,
                    classification.Sentiment,
                    classification.Complexity);

                _logger.LogInformation("AI Sammenfatning: {summary}", classification.Summary);

                // TODO Sprint 3: Opret case i Dynamics 365 baseret på klassificering
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Fejl ved hentning af emails: {error}", ex.Message);
        }
    }
}