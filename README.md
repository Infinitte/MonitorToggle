# Monitor Toggle Tray

A system tray application for Windows 11 that allows toggling monitors on and off.

## Installation

### Using the Installer (Recommended)

1. Download `MonitorToggleTraySetup.exe` from the [latest release](https://github.com/Infinitte/MonitorToggle/releases).
2. Run the installer and follow the setup wizard.
3. The application will be installed and can be launched from the Start menu or desktop shortcut.
4. Optionally, enable autostart during installation.

### Manual Installation

1. Ensure `uvx` is available in the PATH (for monitorcontrol).
2. Copy the files from `MonitorToggleTray\bin\Release\net48\win-x64\publish\` to a folder.
3. Run `MonitorToggleTray.exe`.

## Features

- Icon in the notification area (system tray) with a custom monitor icon
- Context menu to turn on/off/toggle monitors 1 and 2
- Global keyboard shortcuts: Ctrl+Alt+F1 to toggle monitor 1, Ctrl+Alt+F2 to toggle monitor 2
- Single instance protection
- State persistence between sessions
- Runs in the background without a visible window

## Usage

- **From the tray icon:** Right-click the icon and select options for monitor 1 or 2.
- **Keyboard shortcuts:**
  - Ctrl+Alt+F1: Toggle monitor 1
  - Ctrl+Alt+F2: Toggle monitor 2
- **Exit:** Right-click the icon and select "Exit".

## Requirements

- Windows 11 (with .NET Framework 4.8 pre-installed)
- `uvx` in PATH for monitor control functionality ([installation instructions](https://docs.astral.sh/uv/getting-started/installation/))

## Simple Executables (For Testing)

Before the full application, simple executables were created for testing:

- **MonitorOffSimple.exe**: Turns off monitor 1.
- **MonitorOnSimple.exe**: Turns on monitor 1.
- **MonitorToggleSimple.exe**: Toggles between on and off for monitor 1 (uses a `monitor_state.txt` file to remember the state).

These are available as separate downloads in the [releases](https://github.com/Infinitte/MonitorToggle/releases).

Run them to verify they work. The toggle remembers the state between executions.

### Compilation

If you want to modify the code:

1. Install .NET 8.0 SDK
2. Run `dotnet build` in the project folder
3. To publish: `dotnet publish -c Release -r win-x64 --self-contained`