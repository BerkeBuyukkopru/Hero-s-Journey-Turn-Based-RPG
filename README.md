# Hero's Journey
**Hero's Journey** is a simple turn-based strategy game developed using **Angular 7** and **ASP.NET MVC**.

## Local Setup

### Database
Run the database script on your local SQL Server Express instance:

```powershell
sqlcmd -S .\SQLEXPRESS -E -i GameDatabase.sql
```

The script creates `GameDatabase`, creates the required `Tbl_*` tables if they do not exist, and inserts the seed data required to start the game.

### Backend
Open `Game/Game.sln` in Visual Studio and run the `GameWebAPI` project. The frontend expects the API at:

```text
http://localhost:5000
```

### Frontend
Install dependencies and start Angular:

```powershell
cd Games
npm install
npm start
```

Then open:

```text
http://localhost:4200
```

## Features
- Start the game by entering a character name.
- Battle randomly generated enemies.
- Choose between three different actions: **Attack**, **Defend**, or **Heal (Use Potion)**.
- After each action, the enemy takes a turn.
- Level up by defeating enemies.
- When all enemies are defeated or the player is defeated, return to the main screen with an option to restart the game.

![image](https://github.com/user-attachments/assets/9a3f53be-f681-4990-b223-ed2ebc89f9b2)
- To start the game, you are asked to enter your character's name.

![image](https://github.com/user-attachments/assets/35c4322f-efba-48de-a7f8-bd4e9f21846d)
- A random enemy appears, and the turn-based battle begins.

![image](https://github.com/user-attachments/assets/bfff2b01-1426-45a4-b635-afeb0e1e5733)

There are three available actions, and after choosing one, the enemy takes its turn:
- **Attack:** Deal damage to the enemy.
- **Defend:** Reduce incoming damage.
- **Use Potion:** Heal your health.

![image](https://github.com/user-attachments/assets/a23fce68-e5bb-42ba-8956-94e038050481)
- When the enemy's health reaches zero, you move on to the next enemy and level up.

![image](https://github.com/user-attachments/assets/28570086-14d0-46bc-adaf-12be56768616)
- Once all enemies are defeated, you are redirected to the main screen with an option to start a new game.

![image](https://github.com/user-attachments/assets/98c6fd62-9c52-4984-8258-dc385b24e871)
- In case of defeat, you are also redirected to the main screen with an option to begin a new game.
