
# Rental Service API

Rental Service API är en RESTful tjänst för att hantera uthyrning av olika bilar. Tjänsten är byggd för att vara flexibel och utökningsbar, och använder designmönster som Strategy Pattern, Factory Method och Dependency Injection för att stödja SOLID-principerna.



## Funktioner
- Hantera Uthyrningsobjekt: Skapa och hantera olika typer av uthyrningsbilar

- Prisberäkning: Dynamisk prisberäkning baserat på objektets typ och hyresperiod.
- Flexibilitet och Utökbarhet: Stöd för att enkelt lägga till nya typer av priser, tack vare Strategy Pattern.

## Använding
Exempel på API-förfrågan

För att registera en uthyrning, skicka en POST-förfrågan till https://localhost:{{portNumber}}/api/rentals/.

Exempel Json-body. 

{
    "personNumber": "1534567890",
    "registrationId": "SGT886",
    "timeOfRent": "2024-08-10T14:30:00"
}

För att registera en retur och få priset för uthyrningen, skicka en POST-förfrågan till https://localhost:{{portNumber}}/api/return/.

Exempel Json-body. 

{
    "bookingNumber": "0896c2f4-d74a-4cc7-999b-6d965f8bd5e5",
    "timeOfReturn": "2024-08-13T15:30:00",
    "endedKm": 1060
}


För att hämta alla uthyrningar skicka en Get-förfrågan till 
https://localhost:{{portNumber}}/api/rentals/
specific uthyrning https://localhost:{{portNumber}}/api/rentals/{bookingNumber}

För att se en lista föra alla bilar i systemet skicka en Get-förfrågan till 
https://localhost:{{portNumber}}/api/rentals/cars

För att hämta alla returer skicka en Get-förfrågan till 
https://localhost:{{portNumber}}/api/return/
specific retur https://localhost:{{portNumber}}/api/return/{bookingNumber}

Jag skickar även med en postman config export i repot.
## Bygga
- [.NET SDK 8.0 eller senare](https://dotnet.microsoft.com/download/dotnet/8.0)
- [AutoMapper](https://automapper.org/) (v13.0.1)
- [Microsoft Exchange Web Services](https://docs.microsoft.com/en-us/exchange/client-developer/web-service-reference/ews-operations-in-exchange) (v2.2.0)
- [Serilog](https://serilog.net/) (v8.0.2)
- [Swagger (Swashbuckle.AspNetCore)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) (v6.4.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) eller [Visual Studio Code](https://code.visualstudio.com/) med C#-tillägg för optimal utvecklingsupplevelse

Med kommandotolken se till att du står i rooten på applikationen
```bash
    dotnet build
    cd /RentalApp
    dotnet run
```

nu kan du köra API förfrågningar till applikationen.
Du bör kunna nå Swagger annars kan det behövas att du ändrar ASPNETCORE_ENVIRONMENT-miljövariabeln.

Kör annars med postman eller i visual studio med RentalApp som start projekt.

## Tester
```bash
    cd /RentalApp.Tests
    dotnet test
```
Går också att köra testerna i visual studio.
