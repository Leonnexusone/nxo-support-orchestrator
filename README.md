# NXO Support Orchestrator

AI-powered support case orchestration system built on Azure AI Foundry, Microsoft Agent Framework, and Dynamics 365.

## What is this?
Automatically processes incoming support emails, classifies them using AI, and creates cases in Dynamics 365.

## Tech Stack
| Layer | Technology |
|---|---|
| Trigger | Azure Functions (C#) + Microsoft Graph API |
| AI | Microsoft Agent Framework + Azure AI Foundry |
| Cases | Dynamics 365 / Dataverse |
| Secrets | Azure Key Vault |
| Logging | Azure Monitor |

## Environment
> All development uses Sandbox environment only.
> No production NXO data is used until explicitly switched.

## Status
| Sprint | Focus | Status |
|---|---|---|
| Sprint 0 | Foundation | 🟡 In Progress |
| Sprint 1 | Email trigger | ⬜ Planned |
| Sprint 2 | AI classification | ⬜ Planned |
| Sprint 3 | Case creation | ⬜ Planned |
| Sprint 4 | Human review | ⬜ Planned |
| Sprint 5 | Demo | ⬜ Planned |