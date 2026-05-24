# Hero's Journey: Turn-Based RPG

**Hero's Journey: Turn-Based RPG** is a simple turn-based browser game built with **Angular 7**, **ASP.NET Web API**, and **SQL Server**. The player creates a hero, fights randomly selected enemies, progresses through levels, and tries to survive until all levels are cleared.

This repository is a cleaned-up and modernized version of an internship project. The goal of the project is to demonstrate a complete local full-stack game flow with a separate frontend, backend, business layer, data access layer, and SQL Server database setup.

## Table of Contents

- [Features](#features)
- [Screenshots](#screenshots)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Architecture Overview](#architecture-overview)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Backend Setup](#backend-setup)
- [Frontend Setup](#frontend-setup)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Gameplay Flow](#gameplay-flow)
- [Known Limitations](#known-limitations)
- [Author](#author)
- [License](#license)

## Features

- Hero creation with basic input validation.
- Random enemy selection based on the current level.
- Turn-based battle flow with player and enemy turns.
- Three player actions:
  - **Attack:** Deals random damage within the hero's attack range.
  - **Defend:** Reduces incoming damage and can trigger counter damage on a full block.
  - **Potion:** Restores health and can be prepared again by defending.
- Delayed enemy turn feedback after player actions.
- Battle log for hero, enemy, and system messages.
- Level progression after defeating enemies.
- Win and defeat flows with modal feedback.
- Responsive game screen designed to fit desktop and mobile viewports without page scrolling.
- Local SQL setup script with seed data.

## Screenshots

### Login Screen

The login screen allows the player to enter a hero name and start a new game.

<img width="1439" height="899" alt="Hero's Journey login screen" src="https://github.com/user-attachments/assets/9dfdcb99-15ca-4665-98ec-df111b968d9b" />

### Battle Screen

The battle screen displays the hero, the current enemy, health bars, available actions, and the current turn status.

<img width="1439" height="899" alt="Hero's Journey battle screen" src="https://github.com/user-attachments/assets/2795384a-06c1-4187-a674-6aba834f5061" />

### Battle Feedback

The battle log records player actions, enemy attacks, and system messages during the turn-based fight.

<img width="1439" height="899" alt="Hero's Journey battle feedback" src="https://github.com/user-attachments/assets/016ad928-7788-43a6-88eb-a612747d5d44" />

## Tech Stack

### Frontend

- Angular 7
- TypeScript
- Bootstrap / Bootswatch
- Font Awesome
- RxJS

### Backend

- ASP.NET Web API
- .NET Framework 4.7.2
- C#
- Layered structure with Web API, BLL, DAL, and shared model projects

### Database

- SQL Server / SQL Server Express
- ADO.NET
- Raw SQL queries through `SqlConnection` and `SqlCommand`

### Tooling

- Visual Studio
- npm
- Angular CLI
- `sqlcmd` for database script execution

## Project Structure

```text
Hero-s-Journey-Turn-Based-RPG/
|-- GameDatabase.sql              # Local database creation and seed script
|-- Game/                         # Backend solution
|   |-- Game.sln
|   |-- GameWebAPI/               # ASP.NET Web API project
|   |-- BLL/                      # Business logic layer
|   |-- DAL/                      # Data access layer
|   `-- Ortak Katman/             # Shared models
`-- Games/                        # Angular frontend
    |-- src/app/login/            # Login screen
    |-- src/app/game/             # Game screen and battle logic
    |-- src/app/services/         # API service classes
    |-- src/app/models/           # Frontend models
    `-- src/assets/images/        # Background and character assets
```

## Architecture Overview

The application is split into a frontend Angular project and a backend ASP.NET Web API solution.

### Frontend Architecture

The Angular frontend is responsible for:

- Displaying the login screen and battle screen.
- Creating a hero through the backend API.
- Fetching hero and enemy data.
- Managing the client-side battle turn flow.
- Showing status messages, battle logs, and modal feedback.
- Keeping the game screen responsive on desktop and mobile devices.

Main frontend parts:

- `LoginComponent`: handles hero name input and hero creation.
- `GameComponent`: handles battle state, player actions, enemy turns, level progression, and modal states.
- `HeroService`: communicates with hero-related API endpoints.
- `EnemyService`: communicates with enemy-related API endpoints.
- `environment.ts`: stores the backend API base URL.

### Backend Architecture

The backend is organized into multiple projects:

- `GameWebAPI`: exposes HTTP endpoints for the frontend.
- `BLL`: contains business logic for hero creation, hero updates, and enemy selection.
- `DAL`: handles SQL Server access.
- `Ortak Katman`: contains shared model classes such as `Hero`, `Enemy`, `Level`, and `HeroTemplate`.

The backend uses ADO.NET instead of Entity Framework. Database access is handled with SQL queries through repository classes.

### Database Access

The data access layer uses a shared `Database` helper to read the `GameDatabase` connection string from configuration. If no configured value is found, it falls back to a local SQL Server Express connection string.

```text
Data Source=.\SQLEXPRESS;Initial Catalog=GameDatabase;Integrated Security=True
```

## Prerequisites

To run the project locally, the following tools are required:

- Windows development environment
- Visual Studio with ASP.NET and .NET Framework project support
- .NET Framework 4.7.2 targeting/developer pack
- SQL Server or SQL Server Express, preferably with a local `SQLEXPRESS` instance
- `sqlcmd` command-line tool
- Node.js and npm
- Angular CLI, installed globally or used through the local project dependencies

## Installation

Clone the repository and move into the project folder:

```powershell
git clone https://github.com/BerkeBuyukkopru/Hero-s-Journey-Turn-Based-RPG.git
cd Hero-s-Journey-Turn-Based-RPG
```

Create and seed the local SQL Server database:

```powershell
sqlcmd -S .\SQLEXPRESS -E -i GameDatabase.sql
```

Open the backend solution in Visual Studio:

```text
Game/Game.sln
```

Restore NuGet packages if needed, set `GameWebAPI` as the startup project, and run the backend with Visual Studio/IIS Express. The API should be available at:

```text
http://localhost:5000
```

> This backend is a .NET Framework ASP.NET Web API project. It is intended to be run from Visual Studio, not with `dotnet run`.

Install frontend dependencies and start the Angular application:

```powershell
cd Games
npm install
npm start
```

Open the application in the browser:

```text
http://localhost:4200
```

## Database Setup

The project includes a database setup script:

```text
GameDatabase.sql
```

The script:

- Creates the `GameDatabase` database if it does not already exist.
- Creates the required tables if they do not already exist:
  - `Tbl_Hero`
  - `Tbl_HeroTemplate`
  - `Tbl_Enemy`
  - `Tbl_Level`
- Inserts the required seed data if the tables are empty:
  - Default hero template
  - Level multipliers
  - Enemy data for levels 1-5

Run the script with SQL Server Express:

```powershell
sqlcmd -S .\SQLEXPRESS -E -i GameDatabase.sql
```

Expected result:

```text
Changed database context to 'GameDatabase'.
```

If your SQL Server instance name is different, update both the `sqlcmd` server value and the connection string in `Game/GameWebAPI/Web.config`.

## Backend Setup

1. Open the backend solution in Visual Studio:

   ```text
   Game/Game.sln
   ```

2. Restore NuGet packages if Visual Studio does not restore them automatically.

3. Set `GameWebAPI` as the startup project.

4. Make sure the connection string in `Game/GameWebAPI/Web.config` points to your SQL Server instance:

   ```xml
   <add name="GameDatabase"
        connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=GameDatabase;Integrated Security=True"
        providerName="System.Data.SqlClient" />
   ```

5. Run the backend project.

> This project is a classic .NET Framework Web API application. Use Visual Studio/IIS Express for local development instead of `dotnet run`.

The frontend expects the backend API to be available at:

```text
http://localhost:5000
```

You can test the backend with:

```powershell
Invoke-RestMethod "http://localhost:5000/api/enemy/GetRandomEnemyByLevel?level=1"
```

## Frontend Setup

1. Go to the Angular project:

   ```powershell
   cd Games
   ```

2. Install dependencies:

   ```powershell
   npm install
   ```

3. Start the frontend:

   ```powershell
   npm start
   ```

4. Open the application:

   ```text
   http://localhost:4200
   ```

The project uses Angular 7. The `start` and `build` scripts include `NODE_OPTIONS=--openssl-legacy-provider` so the project can run more reliably on newer Node.js versions.

Because this is a legacy Angular 7 project, `npm install` may show deprecation or vulnerability warnings from older dependencies. These warnings do not necessarily prevent the local development server from running.

## Usage

After the backend and frontend are running:

1. Open `http://localhost:4200`.
2. Enter a hero name on the login screen.
3. Click **Oyuna Başla** to create the hero and start the game.
4. Use the battle actions during your turn:
   - **Saldır:** attacks the current enemy.
   - **Defans:** reduces the next incoming enemy damage and may prepare another potion.
   - **Pot:** restores hero health when a potion is available.
5. Follow the battle log to see player actions, enemy actions, level progression, win messages, and defeat messages.

## API Endpoints

Base URL:

```text
http://localhost:5000/api
```

### Hero Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/hero/GetHero?heroId={id}` | Returns a hero by id. |
| `POST` | `/hero/InitializeHeroFromTemplate` | Creates a new hero from a hero template. |
| `PUT` | `/hero/UpdateHeroStats` | Updates hero stats when progressing to a new level. |

#### Create Hero Request

```json
{
  "templateId": 1,
  "heroName": "Hero"
}
```

#### Update Hero Request

```json
{
  "heroId": 1,
  "heroLevel": 2
}
```

### Enemy Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/enemy/GetRandomEnemyByLevel?level={level}` | Returns a random enemy for the requested level. |

## Gameplay Flow

1. The player enters a hero name on the login screen.
2. The frontend sends a request to create a hero from the default hero template.
3. The game screen loads the created hero by `heroId`.
4. A random enemy is loaded for the current level.
5. The player chooses one of three actions:
   - Attack
   - Defend
   - Potion
6. After attack or defend actions, the enemy turn is triggered with a short delay.
7. The battle log records player, enemy, and system events.
8. If the enemy is defeated, the player advances to the next level.
9. If all levels are completed, the player wins.
10. If the hero's health reaches zero, the player loses and returns to the login screen.

## Known Limitations

- This project was originally developed as an internship/learning project.
- The backend uses ADO.NET and raw SQL queries instead of Entity Framework.
- Angular 7 is a legacy Angular version.
- The project is designed primarily for local development and demonstration.
- Authentication, persistent user accounts, and production deployment configuration are not included.

## Author

Developed by **Berke Büyükköprü**.

- GitHub: [BerkeBuyukkopru](https://github.com/BerkeBuyukkopru)
- LinkedIn: [Berke Büyükköprü](https://www.linkedin.com/in/berke-buyukkopru/)

## License

This project is licensed under the MIT License. See the [`LICENSE`](LICENSE) file for details.
