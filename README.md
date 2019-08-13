# Easter Seals Telethon Donations Board

A digital reinvention of the Easter Seals Telethon Angel Board. This board shows donors who have donated >= $100. 
The host of the show can display the donors one by one when exclaiming their name. Donors are added in a form located on the control panel window.

First successfully used during the 2019 telethon:
[![Easter Seals Telethon 2019](https://img.youtube.com/vi/4tVy8v-E6h0/0.jpg)](http://www.youtube.com/watch?v=4tVy8v-E6h0)

Notable timestamps of app in action: [3:22:25](https://youtu.be/4tVy8v-E6h0?t=12145), [4:39:00](https://youtu.be/4tVy8v-E6h0?t=16740), [5:52:00](https://youtu.be/4tVy8v-E6h0?t=21120)

### Notable Features

* Contains [Bluetooth remote](https://github.com/ilyabru/GearVrController4Windows) functionality. This allows the show host to easily select and display donors (see video)
* Implements an MVVM architectural pattern
* Uses a SQLite database for persistence with full CRUD functionality
* Has a statistics screen useful for data analysis

<img src="./Docs/MainScreen.png" alt="image" width="300"/>
<img src="./Docs/Popup.png" alt="image" width="300"/>

## Operating instructions

1.  Clone and build GearVrController4Windows project from here: https://github.com/ilyabru/GearVrController4Windows.
2.  Add a reference to GearVrController4Windows.dll you've just built.
3.  When ready for production replace all Mock services with the actual implementations in ServiceLocator class. For example, this:
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

**W** on main screen: Open popup and view last unread donor. Clicking again will navigate to next donor.

**Esc** when popup is open: close popup.

