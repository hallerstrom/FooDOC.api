# FooDOC.api

FooDOC är ett enkelt REST API byggt med .NET 9.0 och Entity Framework Core. Det hanterar användare, produkter och data via JWT-skyddade endpoints.

##### Klientrepo: [FooDOC] ([hallerstrom/FooDOC at React+BackEnd](https://github.com/hallerstrom/FooDOC/tree/React%2BBackEnd))

## Installation

1. Klona eller ladda ner projektet.
2. Kör följande kommando för att installera nödvändiga paket:

   ```bash
   dotnet restore
   ```

```bash
dotnet run
```

**Projektet kommer köras på** `http://localhost:5164/`

**För att öppna Swagger-dokumentationen i din webbläsare: ** `http://localhost:5164/swagger/index.html`

## Endpoints

#### AuthController


| Method | Endpoint           | Auth | Beskrivning                    |
| -------- | -------------------- | ------ | -------------------------------- |
| POST   | /api/auth/login    | ❌   | Logga in och få en JWT-token. |
| POST   | /api/auth/register | ❌   | Registrera en ny användare.   |

#### CCPController


| Metod | Endpoint      | Auth | Beskrivning             |
| ------- | --------------- | ------ | ------------------------- |
| POST  | /api/ccp      | ❌   | Skapa en ny CCP.        |
| GET   | /api/ccp      | ✅   | Hämta alla CCPs.       |
| GET   | /api/ccp/{id} | ✅   | Hämta en specifik CCP. |

#### ProductController


| Metod | Endpoint     | Auth | Beskrivning               |
| ------- | -------------- | ------ | --------------------------- |
| GET   | /api/product | ❌   | Hämta alla produkter.    |
| POST  | /api/product | ✅   | Lägg till en ny produkt. |


## JWT-Authentication

För endpoints som kräver autentisering, använd en JWT-token i Authorization-headern:
Authorization: Bearer {token}

## Testa API:et

Du kan använda Swagger UI eller en klient som Postman eller curl för att testa API:et.

## Teknologier som använts:

- **.NET 9.0**
- **ASP.NET Core Web API** – Skapar RESTful endpoints
- **Entity Framework Core (InMemory)** – För datalagring (Databas)
- **JWT (Json Web Token)** – För autentisering och auktorisering
- **Swashbuckle/Swagger** – För automatisk API-dokumentation
