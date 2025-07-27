# 🍟 Ashley's Fritje
## .NET MAUI App voor Klanten & Beheerders in een Frituur

Deze app is bedoeld als showcase. De backend draait lokaal en is niet publiek beschikbaar.

### Overzicht
Ashley’s Fritje is een mobiele applicatie ontwikkeld met .NET MAUI voor Android, bedoeld voor frituren die bestellingen digitaal willen beheren. De app maakt een duidelijk onderscheid tussen beheerders (admin) en klanten (client) en biedt functionaliteiten zoals productbeheer, bestelbeheer, gebruikersauthenticatie, en een intuïtieve gebruikersinterface.

### Gebruikte technologieën
- .NET MAUI: Cross-platform UI framework (Android only)
- ASP.NET Core Web API:	Backend voor data-opslag & business logic
- Entity Framework Core: ORM voor database interactie
- SQL Server: Lokale of externe opslag
- JWT: Authenticatie & autorisatie
- xUnit + Moq: Unit testing van services

### Functionaliteiten
#### Beheerder (Admin)
- Productbeheer: Producten toevoegen, wijzigen (naam, prijs, optionele afbeelding), verwijderen
- Categoriebeheer: Nieuwe categorieën toevoegen en verwijderen (indien niet gekoppeld aan producten)
- Bestellingen: Overzicht van alle bestellingen, filteren op status, status wijzigen, gedetailleerde bestelinfo bekijken

#### Klant (Client)
- Productoverzicht: Filteren op categorie, aantal kiezen met +/– of handmatig invoeren
- Winkelmandje: aantal wijzigen, producten verwijderen, bestelling plaatsen(vereist een ingelogde gebruiker)
- Bestellingen: Overzicht van eigen bestellingen, filteren op status, status volgen (bijv. in behandeling, afgehaald), gedetailleerde bestelinfo bekijken
- Authenticatie: Registreren, inloggen en uitloggen

#### Toekomstige uitbreidingen
- Integratie van een betalingssysteem
- Notificaties bij wijziging van bestellingstatus
- Zoekfunctionaliteit voor producten
- iOS-ondersteuning via MAUI

### Screenshots
#### Gebruiker algemeen
![Login](screenshots/login.png)
![Registratie](screenshots/registratie.png)
![Account overzicht (niet ingelogd)](screenshots/account-overview-user.png)

#### Klant
[Home client](screenshots/home-client.png)
![Account overzicht klant](screenshots/account-overview-client.png)
[Menu overzicht](screenshots/menu-overview-client.png)
[Winkelwagen](screenshots/shoppingcart.png)
[Bestelling geplaatst](screenshots/order-placed-client.png)
[Overzicht bestellingen](screenshots/orders-overview-client.png)
[Overzicht bestelling](screenshots/order-overview-client.png)

#### Beheerder
[Home admin](screenshots/home-admin.png)
![Account overzicht admin](screenshots/account-overview-admin.png)
[Menu overzicht admin](screenshots/menu-overview-admin.png)
[Categorie toevoegen](screenshots/categorie-toevoegen.png)
[Categorie verwijderen](screenshots/categorie-verwijderen.png)
[Product toevoegen](screenshots/product-toevoegen.png)
[Product updaten](screenshots/product-wijzigen.png)
[Overzicht bestellingen](screenshots/orders-overview-admin.png)
[Overzicht bestelling](screenshots/order-overview-admin.png)

