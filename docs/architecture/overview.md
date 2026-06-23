# Architecture Overview

## System Flow

📧 Email ankommer til support inbox

↓

⚡ Azure Function (C#) — læser email via Graph API

↓

🤖 Microsoft Agent Framework

Classifier Agent → Support / Operations / New Dev?
Customer Lookup Agent → find kunde i Dynamics 365
RAG Agent → find lignende historiske sager

↓

📊 Confidence Score beregnes
Over 90%  → opret case automatisk
70-89%    → opret case + flag til review
Under 70% → send til konsulent for godkendelse

↓

📋 Case oprettes i Dynamics 365

↓

📬 Bekræftelsesmail sendes til kunde



## Environments

| | Sandbox (Test) | Production |
|---|---|---|
| Dynamics 365 | nxpowerpagesdevelopment.crm4.dynamics.com | skiftes senere |
| Email | Developer mailbox | support@nexusone.dk |
| Data | Fake test data | Rigtige NXO kunder |
| Skift | Environment variable | Environment variable |

## Azure Resources

| Resource | Navn | Formål |
|---|---|---|
| Resource Group | rg-nxo-orchestrator | Container for alt |
| Azure Function | func-nxo-orchestrator | Email trigger |
| Key Vault | kv-nxo-orchestrator | Alle secrets |
| Azure OpenAI | aoai-nxo-orchestrator | GPT-4o model |
| Azure AI Search | srch-nxo-orchestrator | RAG / historiske sager |
| App Insights | appi-nxo-orchestrator | Logging |
| App Registration | app-nxo-orchestrator | Adgang til Graph + Dataverse |