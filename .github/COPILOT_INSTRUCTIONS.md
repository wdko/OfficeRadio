# Copilot Instructions for OfficeRadio Repository

## Overview
This repository contains the source code for the OfficeRadio project, a Windows desktop application for casting radio streams to Chromecast devices. The project is written in C# using Windows Forms and targets .NET 9.0. It leverages libraries such as Sharpcaster for Chromecast integration and Tmds.MDns for multicast DNS.

## Project Structure
- `Caster.sln`: Solution file for Visual Studio.
- `Caster/`: Main project directory.
  - `Caster.csproj`: Project file.
  - `Program.cs`: Application entry point.
  - `Form1.cs`, `Form1.Designer.cs`, `Form1.resx`: Main Windows Form and its designer/resources.
  - `CastingService.cs`: Service logic for casting to devices.
  - `RadioCastingHelper.cs`: Helper functions for radio stream management.
  - `bin/`, `obj/`: Build output and intermediate files.

## Coding Guidelines
- **Language**: C# (.NET 9.0, Windows Forms)
- **Naming Conventions**:
  - Use PascalCase for class names, methods, and properties.
  - Use camelCase for local variables and parameters.
  - Prefix private fields with `_`.
- **File Organization**:
  - UI code in `Form1.*` files.
  - Business logic in `CastingService.cs` and `RadioCastingHelper.cs`.
  - Entry point in `Program.cs`.
- **Comments**:
  - Use XML documentation comments for public classes and methods.
  - Inline comments for complex logic.

## Copilot Usage Instructions
- **Feature Implementation**:
  - When asked to implement a feature, first identify the relevant file(s) based on the feature domain (UI, casting logic, helpers).
  - For UI changes, edit `Form1.cs` and `Form1.Designer.cs` as needed.
  - For casting logic, use or extend `CastingService.cs` and `RadioCastingHelper.cs`.
- **Refactoring**:
  - Ensure changes do not break existing functionality.
  - Run and validate the application after refactoring.
- **Testing**:
  - Manual testing is required; automated tests are not present.
  - Validate casting functionality with a Chromecast device.
- **Dependencies**:
  - Use NuGet for package management.
  - External libraries: Sharpcaster, Tmds.MDns, Google.Protobuf, Microsoft.Extensions.*

## Best Practices
- **Error Handling**:
  - Use try-catch blocks for network and casting operations.
  - Log errors to the console or a log file.
- **UI Responsiveness**:
  - Use async/await for network operations to avoid blocking the UI thread.
- **Resource Management**:
  - Dispose of resources (e.g., streams, device connections) properly.

## How to Use Copilot in This Repository
1. **Ask for code changes or new features**: Specify the feature, desired behavior, and any UI requirements.
2. **Request refactoring or improvements**: Indicate the area of code and the goal (e.g., performance, readability).
3. **Get explanations**: Ask for explanations of code, architecture, or dependencies.
4. **Troubleshoot issues**: Describe the problem and Copilot will help diagnose and suggest fixes.

## Example Prompts
- "Add a button to Form1 that refreshes the list of available Chromecast devices."
- "Implement error logging in CastingService.cs."
- "Explain how RadioCastingHelper.cs manages radio streams."
- "Refactor Form1.cs to use async/await for device discovery."

## Additional Notes
- Always review Copilot's suggestions before committing changes.
- Ensure compatibility with .NET 9.0 and Windows Forms.
- For major changes, update the README.md with usage instructions.

---

*This instructions file is intended to guide Copilot and contributors in maintaining consistency, quality, and clarity throughout the OfficeRadio project.*
