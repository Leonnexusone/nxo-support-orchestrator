# NXO Support Orchestrator

AI-powered support case orchestration system built on Azure AI Foundry, Microsoft Agent Framework, and Dynamics 365.

## What is this?
Automatically processes incoming support emails, classifies them using AI, and creates cases in Dynamics 365.

## Current Status: Sprint 2 Complete ✅

**Working Features:**
- ✅ Email polling from Microsoft 365 (every 2 minutes)
- ✅ AI-powered email classification (GPT-4.1-mini)
- ✅ Category detection (D365CRM, BusinessCentral, Technical, Billing, etc.)
- ✅ Priority, sentiment, and complexity assessment
- ✅ Automated AI summaries of support emails

**Next:** Sprint 3 - Dynamics 365 case creation

## Tech Stack
| Layer | Technology | Status |
|---|---|---|
| Trigger | Azure Functions (C#) + Microsoft Graph API | ✅ Working |
| AI | Azure OpenAI (GPT-4.1-mini) | ✅ Working |
| Cases | Dynamics 365 / Dataverse | ⬜ Next sprint |
| Secrets | Azure Key Vault | ⬜ Planned |
| Logging | Azure Monitor | 🟡 Local only |

## Quick Start

See [START-SYSTEM.md](START-SYSTEM.md) for commands to run the system locally.

## Environment
> All development uses Sandbox environment only.
> No production NXO data is used until explicitly switched.

## Sprint Progress
| Sprint | Focus | Status |
|---|---|---|
| Sprint 0 | Foundation | ✅ Done |
| Sprint 1 | Email trigger | ✅ Done |
| Sprint 2 | AI classification | ✅ Done |
| Sprint 3 | Case creation | ⬜ Next |
| Sprint 4 | Human review | ⬜ Planned |
| Sprint 5 | Demo | ⬜ Planned |

## Architecture

```
📧 Email → Azure Functions → 🤖 AI Classification → 📊 Dynamics 365
         (Microsoft Graph)   (GPT-4.1-mini)        (Coming Sprint 3)
```

**Mailbox:** support@personale309.onmicrosoft.com  
**Polling:** Every 2 minutes (configurable)  
**Classification:** Real-time AI analysis with category, priority, sentiment, complexity