# Changelog

All notable changes to the NXO Support Orchestrator project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added - Sprint 1 (Email Trigger)
- Azure Functions project initialized with .NET 9 isolated worker runtime
- EmailTrigger function created using TimerTrigger template
- Timer configured to run every 5 minutes (cron: `0 */5 * * * *`)
- Basic logging infrastructure with ILogger
- TODO comments for future sprints (Graph API, AI classification, Dynamics 365)

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
| Sprint 1 | Email trigger | 🟡 In Progress |
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
