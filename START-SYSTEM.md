# 🚀 Start NXO Support Orchestrator

## Start systemet

### Terminal 1 - Start Azurite (Azure Storage emulator)
```powershell
azurite --silent --location C:\Users\LeonLorenzen\nxo-support-orchestrator\.azurite
```

### Terminal 2 - Start Azure Functions
```powershell
cd C:\Users\LeonLorenzen\nxo-support-orchestrator\src\NXO.Functions
dotnet run
```

---

## 🛑 Stop systemet

```powershell
Get-Process -Name dotnet,func,node -ErrorAction SilentlyContinue | Stop-Process -Force
```

---

## 📊 Hvad sker der?

Systemet kører hvert 2. minut og:
1. ✅ Henter ulæste emails fra `support@personale309.onmicrosoft.com`
2. ✅ Sender hver email til AI klassificering (GPT-4.1-mini)
3. ✅ Logger resultatet:
   - Email info (fra, emne, modtaget)
   - AI klassificering (kategori, prioritet, sentiment, kompleksitet)
   - AI sammenfatning

---

## 💡 Test AI klassificering

Send en email til: **support@personale309.onmicrosoft.com**

### Eksempel - Kritisk D365 fejl:
**Emne:** Dynamics 365 fejl - Kan ikke gemme kontakter  
**Body:** Hej, vi har akut behov for hjælp. Vores sælgere kan ikke gemme nye kontakter i D365 CRM. Systemet giver en fejl. Dette er kritisk!

**Forventet klassificering:**
- Kategori: D365CRM
- Prioritet: Critical/High
- Sentiment: Frustrated
- Kompleksitet: Medium/Complex
