# Changelog

All notable changes to the NXO Support Orchestrator project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.0] - 2026-06-26

### Added - Sprint 1 (Email Trigger) ✅ COMPLETED
- Azure Functions project reinitialized with .NET 10 isolated worker runtime
- EmailTrigger function created using TimerTrigger template
- Timer configured to run every 2 minutes for testing (cron: `0 */2 * * * *`)
- Microsoft Graph API integration:
  - Installed `Microsoft.Graph` (v6.2.0) NuGet package
  - Installed `Azure.Identity` (v1.21.0) NuGet package
  - Implemented `GraphServiceClient` with `ClientSecretCredential` authentication
  - Email fetching logic with filters (only unread emails, top 10)
  - Selected fields optimization (subject, from, body, receivedDateTime)
  - **Successfully tested with Microsoft 365 mailbox (support@personale309.onmicrosoft.com)**
- Configuration management:
  - Environment variables for Azure credentials (Tenant ID, Client ID, Client Secret)
  - Mailbox configuration via `GRAPH_MAILBOX` setting
- Local development setup:
  - Azurite installed globally via npm for local Azure Storage emulation
  - Azure Monitor OpenTelemetry disabled for local development
- Error handling and logging:
  - Try-catch blocks for Graph API calls
  - Structured logging with ILogger
  - Detailed error messages for troubleshooting

### Changed - Sprint 1
- Upgraded from .NET 9 to .NET 10 (matching installed SDK)
- Commented out Azure Monitor exporter in Program.cs for local development
- Timer interval changed from 5 minutes to 2 minutes for faster testing

### Fixed - Sprint 1
- Resolved .NET SDK version mismatch (switched from net9.0 to net10.0)
- Fixed Azure Storage connection string requirement by installing Azurite
- Resolved file locking issues during build by using `dotnet clean`
- Fixed Azure App Registration authentication by using correct Client Secret Value (not Secret ID)
- Successfully configured API permissions (Mail.Read) and admin consent in Azure Portal

## [0.1.0] - 2026-06-24

### Added - Sprint 0 (Foundation)
- Initial project repository created
- Project structure established:
  - `/src/NXO.Functions` - Azure Functions
  - `/src/NXO.Agents` - AI Agent logic
  - `/src/NXO.DataverseClient` - Dynamics 365 integration
  - `/src/NXO.Tests` - Test suite
  - `/docs` - Documentation
  - `/infra/azure` - Infrastructure as Code
- Documentation created:
  - README.md with project overview and tech stack
  - Architecture overview (`docs/architecture/overview.md`)
  - Sprint 0 documentation (`docs/sprints/sprint-0.md`)
  - User stories documentation (`docs/user-stories/`)
- Git branch strategy established:
  - `main` - production ready code
  - `sprint-*` - feature branches per sprint

### Infrastructure
- Sandbox Dynamics 365 environment confirmed
- Test customer created (NexoTech Test ApS)

---

## Sprint Status

| Sprint | Focus | Status |
|---|---|---|
| Sprint 0 | Foundation | ✅ Done |
| Sprint 1 | Email trigger | ✅ Done |
| Sprint 2 | AI classification | ⬜ Planned |
| Sprint 3 | Case creation | ⬜ Planned |
| Sprint 4 | Human review | ⬜ Planned |
| Sprint 5 | Demo | ⬜ Planned |

---

## Categories Explained

- **Added** - New features
- **Changed** - Changes to existing functionality
- **Deprecated** - Features that will be removed soon
- **Removed** - Removed features
- **Fixed** - Bug fixes
- **Security** - Security improvements
