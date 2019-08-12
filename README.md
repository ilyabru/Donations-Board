# Easter Seals Telethon Donations Board

## Operating instructions

## Setup

1.  Clone and build GearVrController4Windows project from here: https://github.com/ilyabru/GearVrController4Windows.
2.  Add a reference to GearVrController4Windows.dll you've just built.
3.  When ready for production replace all Mock services with the actual implementations in ServiceLocator class. For example this:
```csharp
serviceCollection.AddSingleton<ISQLiteService, MockSQLiteService>();
serviceCollection.AddSingleton<ISessionService, MockSessionService>();
serviceCollection.AddSingleton<IDonorService, MockDonorService>();
serviceCollection.AddSingleton<IStatsService, MockStatsService>();
```
becomes this:
```csharp
serviceCollection.AddSingleton<ISQLiteService, SQLiteService>();
serviceCollection.AddSingleton<ISessionService, SessionService>();
serviceCollection.AddSingleton<IDonorService, DonorService>();
serviceCollection.AddSingleton<IStatsService, StatsService>();
```

## Hotkeys
**Ctrl+F** on main screen: enable full screen mode
        
**Ctrl+P** on main screen: Open control panel

**W** on main screen: Open popup and view last undread donor. Clicking again will navigate to next donor.

**Esc** when popup is open: close popup.

