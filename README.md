# AlphaProject

## Scopo del progetto

AlphaProject è un progetto nato per esplorare tecnologie come .NET Core e architetture API RESTful.
Come autenticazione si appoggia alla piattaforma Zitadel, mentre il DB è ospitato su Supabase

## Stato attuale

- **API:** Attualmente offre funzionalità per la gestione di:
  - Clienti (`Clients`)
  - Prodotti (`Products`)
  - Ordini (`Orders`)
  - Articoli d’ordine (`OrderItems`)
- Utilizza **Entity Framework Core** per la gestione dei dati e **AutoMapper** per la trasformazione tra DTO e modelli.
- L’API implementa CRUD, validazione, mapping esteso e relazioni tra entità, con architettura pronta per estensioni future.

## Prossimi step

- **Realizzazione di un CRM in Blazor:**  
  Lo sviluppo procede con la creazione di un’interfaccia CRM moderna basata su **Blazor**, che si integra con le API esistenti, richiedendo l'uso dei suoi verbi tramite autorizzazione OpenID.

## Note
  
**Contributi** e feedback sono benvenuti per migliorare le funzionalità e la struttura del progetto.
