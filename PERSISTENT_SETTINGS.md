# Persistent Settings Implementation

This document describes the persistent settings implementation for OfficeRadio.

## Overview

The application now supports persistent settings that are saved between sessions. The settings are stored in JSON format in the user's application data directory.

## Settings Location

Settings are saved to: `%APPDATA%\OfficeRadio\settings.json`

## Supported Settings

1. **MinimizeToTray** (bool): When enabled, minimizing the window hides it to the system tray instead of showing on the taskbar.

2. **ExitToTray** (bool): When enabled, clicking the close button (X) minimizes the application to the tray instead of exiting.

3. **FavoriteDeviceName** (string): Stores the name of the preferred Chromecast device. When devices are discovered, this device will be prioritized and selected automatically if found.

## User Interface

### Settings Button
A "Settings" button has been added to the main form that opens the settings configuration dialog.

### System Tray Integration
- The application shows an icon in the system tray
- Right-click the tray icon to access: Show, Settings, and Exit options
- Double-click the tray icon to restore the window

### Settings Dialog
The settings dialog allows users to configure:
- Minimize to tray checkbox
- Exit to tray checkbox  
- Favorite device name text field

## Automatic Behavior

### Settings Persistence
- Settings are automatically loaded when the application starts
- Settings are saved immediately when changed through the UI
- Device selection automatically updates the favorite device setting

### Device Discovery
When devices are discovered:
1. All available devices are found
2. If a favorite device is set and found, it's moved to the top of the list
3. The favorite device is automatically selected
4. If no favorite device is set or found, the first discovered device is selected

### Tray Functionality
- **Minimize to Tray**: When enabled, minimizing the window hides it completely
- **Exit to Tray**: When enabled, the close button minimizes instead of closing
- The application can always be fully exited through the tray context menu

## Technical Implementation

### Settings Class
```csharp
public class Settings
{
    public bool MinimizeToTray { get; set; } = false;
    public bool ExitToTray { get; set; } = false;
    public string? FavoriteDeviceName { get; set; } = null;
    
    public static Settings Load() // Loads from JSON
    public void Save() // Saves to JSON
}
```

### Form Overrides
- `OnResize`: Handles minimize to tray behavior
- `OnFormClosing`: Handles exit to tray behavior
- `SetVisibleCore`: Manages window visibility

## Error Handling

The settings system includes error handling for:
- Missing settings file (creates default settings)
- Corrupted JSON (falls back to default settings)
- File system errors (logs but continues with defaults)

This ensures the application continues to work even if settings cannot be loaded or saved.