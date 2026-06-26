using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using System.Text.Json;

namespace NXO.Functions.Services;

// ════════════════════════════════════════════════════════════════════════════
// EmailClassifier - AI-powered email classification service
// ════════════════════════════════════════════════════════════════════════════
// Bruger Azure OpenAI (GPT-4.1-mini) til at klassificere support emails
// 
// Klassificerer:
//   - Kategori: TechnicalSupport, D365CRM, BusinessCentral, Billing, Onboarding, Complaint, General
//   - Prioritet: Critical, High, Medium, Low
//   - Sentiment: Positive, Neutral, Negative, Frustrated
//   - Kompleksitet: Simple, Medium, Complex, Expert
//   - Confidence: 0.0 - 1.0 (AI's sikkerhed i klassificeringen)
//   - Summary: AI-genereret sammenfatning på dansk/engelsk
//
// Fallback: Ved fejl returneres General/Medium med fejlbesked i summary
// ════════════════════════════════════════════════════════════════════════════

public class EmailClassifier
{
    private readonly ILogger<EmailClassifier> _logger;
    private readonly AzureOpenAIClient _client;
    private readonly string _model;

    // Constructor: Henter AI konfiguration fra environment variables
    // AI_ENDPOINT: Azure OpenAI endpoint URL (fx https://xxx.openai.azure.com/)
    // AI_API_KEY: Azure OpenAI API key
    // AI_MODEL: Model navn (default: gpt-4.1-mini)
    public EmailClassifier(ILogger<EmailClassifier> logger)
    {
        _logger = logger;
        var endpoint = Environment.GetEnvironmentVariable("AI_ENDPOINT")!;
        var apiKey = Environment.GetEnvironmentVariable("AI_API_KEY")!;
        _model = Environment.GetEnvironmentVariable("AI_MODEL") ?? "gpt-4.1-mini";
        
        _client = new AzureOpenAIClient(new Uri(endpoint), new System.ClientModel.ApiKeyCredential(apiKey));
    }

    // ClassifyEmailAsync: Sender email til AI og returnerer struktureret klassificering
    // - Subject: Email emne
    // - Body: Email indhold (max 4000 chars for at undgå token limits)
    // - Returnerer: ClassificationResult med alle klassificeringsfelter
    public async Task<ClassificationResult> ClassifyEmailAsync(string subject, string body)
    {
        // ═══════════════════════════════════════════════════════════════════════
        // PROMPT ENGINEERING
        // ═══════════════════════════════════════════════════════════════════════
        // Detaljeret prompt der fortæller AI'en:
        // 1. Hvem den er (support email classifier for NXO IT-konsulenthus)
        // 2. Hvilke kategorier der findes
        // 3. Hvilket JSON format den skal returnere
        // 4. At den IKKE må tilføje markdown eller forklaring
        // ═══════════════════════════════════════════════════════════════════════
        var prompt = $$"""
            Du er et AI-system der klassificerer support-emails for NXO, et Microsoft-specialiseret IT-konsulenthus.
            
            Klassificer følgende email og returner KUN et JSON-objekt uden markdown eller forklaring.
            
            Kategorier: TechnicalSupport, D365CRM, BusinessCentral, Billing, Onboarding, Complaint, General
            Prioritet: Critical, High, Medium, Low
            Sentiment: Positive, Neutral, Negative, Frustrated
            Kompleksitet: Simple, Medium, Complex, Expert
            
            Email:
            Emne: {{subject}}
            Indhold: {{body?.Substring(0, Math.Min(body?.Length ?? 0, 4000))}}
            
            Returner præcist dette JSON format:
            {
              "category": "...",
              "priority": "...",
              "sentiment": "...",
              "complexity": "...",
              "confidence": 0.0,
              "summary": "..."
            }
            """;

        try
        {
            // Hent chat client og send beskeder til AI
            var chatClient = _client.GetChatClient(_model);
            
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystemMessage("Du er et klassificerings-system. Returner KUN valid JSON."),
                ChatMessage.CreateUserMessage(prompt)
            };
            
            // Send til Azure OpenAI og vent på svar
            var response = await chatClient.CompleteChatAsync(messages);

            // Parse JSON svar til C# objekt
            var json = response.Value.Content[0].Text;
            var result = JsonSerializer.Deserialize<ClassificationResult>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Tillad "Category" og "category"
            });

            _logger.LogInformation("Email klassificeret: {Category} / {Priority} / {Confidence}",
                result?.Category, result?.Priority, result?.Confidence);

            return result ?? new ClassificationResult { Category = "General", Priority = "Medium" };
        }
        catch (Exception ex)
        {
            // FALLBACK: Ved AI fejl, returner default klassificering
            // Dette sikrer at systemet fortsætter selvom AI servicen fejler
            _logger.LogError(ex, "Fejl ved AI klassificering — bruger fallback");
            return new ClassificationResult
            {
                Category = "General",
                Priority = "Medium",
                Sentiment = "Neutral",
                Complexity = "Simple",
                Confidence = 0.0f,
                Summary = "Klassificering fejlede — manuel behandling påkrævet"
            };
        }
    }
}

// ════════════════════════════════════════════════════════════════════════════
// ClassificationResult - Struktureret resultat fra AI klassificering
// ════════════════════════════════════════════════════════════════════════════
// Dette objekt indeholder AI'ens vurdering af en support email
// Bruges til at beslutte hvordan casen skal oprettes i Dynamics 365 (Sprint 3)
// ════════════════════════════════════════════════════════════════════════════
public class ClassificationResult
{
    public string Category { get; set; } = "General";      // Hvilken type support henvendelse
    public string Priority { get; set; } = "Medium";       // Hvor hurtigt skal det behandles
    public string Sentiment { get; set; } = "Neutral";     // Kundens humør/tone
    public string Complexity { get; set; } = "Simple";     // Hvor svært er problemet
    public float Confidence { get; set; }                  // AI's sikkerhed (0.0 - 1.0)
    public string Summary { get; set; } = string.Empty;    // Kort sammenfatning på dansk/engelsk
}