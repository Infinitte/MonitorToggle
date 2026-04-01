# Monitor Toggle Tray

A system tray application for Windows 11 that allows toggling monitor 1 on and off using existing scripts.

## Simple Executables (Step by Step)

Before the full application, I created simple executables for testing:

- **MonitorOffSimple.exe**: Turns off monitor 1.
- **MonitorOnSimple.exe**: Turns on monitor 1.
- **MonitorToggleSimple.exe**: Toggles between on and off for monitor 1 (uses a `monitor_state.txt` file to remember the state).

These are in the respective folders `\bin\Release\net8.0\win-x64\publish\`.

Run them to verify they work. The toggle remembers the state between executions.

## Full Application

The tray application is in `MonitorToggleTray\bin\Release\net8.0-windows\win-x64\publish\MonitorToggleTray.exe`.

### Features

- Icon in the notification area (system tray) with an information-type icon
- Context menu to turn on/off/toggle monitors 1 and 2
- Global keyboard shortcuts: Ctrl+Alt+F1 to toggle monitor 1, Ctrl+Alt+F2 to toggle monitor 2
- Runs in the background without a visible window

### Installation

1. Ensure `uvx` is available in the PATH (for monitorcontrol).
2. Run `MonitorToggleTray.exe`.
3. The icon will appear in the system tray.

### Usage

- **From the tray icon:** Right-click the icon and select options for monitor 1 or 2.
- **Keyboard shortcuts:**
  - Ctrl+Alt+F1: Toggle monitor 1
  - Ctrl+Alt+F2: Toggle monitor 2
- **Exit:** Right-click the icon and select "Exit".

### Customization

- To change keyboard shortcuts, modify the RegisterHotKey lines in Form1.cs.
- To change the icon, replace SystemIcons.Information with a custom icon (add a .ico file to the project and use Icon = new Icon("path")).
- To add more monitors, copy the methods and register more hotkeys.

### Requirements

- Windows 11
- .NET 8.0 (included in the self-contained publish)
- uvx and monitorcontrol installed

### Compilation

If you want to modify the code:

1. Install .NET 8.0 SDK
2. Run `dotnet build` in the project folder
3. To publish: `dotnet publish -c Release -r win-x64 --self-contained`