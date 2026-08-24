# USBUnlocker v3.0

Portable Windows IT Diagnostic and Repair Toolkit

## Overview

USBUnlocker is a comprehensive USB mass storage diagnostic and repair tool for authorized administration of Windows 7, 8, 8.1, and 10 systems.

## Features

- USB configuration scanning and repair
- System health scoring
- Driver audit
- Network diagnostics
- Security configuration analysis
- Event log analysis
- Hardware inventory
- Service audit
- JSON/ZIP report generation
- Dynamic ASCII art headers
- Interactive persistent menu

## Requirements

- Windows 7/8/8.1/10
- Administrator privileges (for repair operations)
- .NET Framework 4.5+ or .NET 6.0+

## Building

### Using .NET CLI (Recommended)

```bash
# Install .NET SDK 6.0+ from https://dotnet.microsoft.com/download
dotnet publish USBUnlocker.csproj -c Release -r win-x64 --self-contained true -o dist
```

### Using Visual Studio

1. Open `USBUnlocker.sln`
2. Select Release configuration
3. Build > Publish
4. Select folder publish
5. Publish

### Using Build Script

```bash
build.bat
```

## Project Structure

```
USBUnlocker/
├── src/
│   ├── Program.cs              # Entry point
│   ├── core/
│   │   ├── Application.cs      # Main application loop
│   │   ├── AppConfig.cs        # Configuration
│   │   ├── FeatureRegistry.cs  # Feature registry
│   │   └── Dispatcher.cs       # Feature dispatcher
│   ├── ui/
│   │   └── HeaderEngine.cs     # ASCII art engine
│   ├── usb/
│   │   ├── UsbScanner.cs       # USB scanning
│   │   ├── UsbRepairer.cs      # USB repair
│   │   ├── UsbVerifier.cs      # USB verification
│   │   └── ...
│   ├── diagnostics/
│   │   ├── SystemInfoDisplay.cs
│   │   ├── HardwareInfoDisplay.cs
│   │   ├── DriverAuditor.cs
│   │   ├── ServiceAuditor.cs
│   │   └── ...
│   ├── network/
│   │   └── NetworkDiagnostic.cs
│   ├── security/
│   │   ├── SecurityDiagnostic.cs
│   │   └── EventLogAnalyzer.cs
│   ├── reports/
│   │   ├── ReportGenerator.cs
│   │   └── JsonExporter.cs
│   └── utilities/
│       ├── Logger.cs
│       ├── AdminHelper.cs
│       ├── SystemInfo.cs
│       └── ...
├── assets/
│   └── ascii_arts.txt          # ASCII artwork
├── USBUnlocker.csproj          # Project file
├── USBUnlocker.sln             # Solution file
└── build.bat                   # Build script
```

## Command-Line Options

```
USBUnlocker.bat                    # Interactive mode
USBUnlocker.bat /silent            # Silent batch mode
USBUnlocker.bat /scan              # Scan only
USBUnlocker.bat /scanrepair        # Scan and repair
USBUnlocker.bat /rollback          # Rollback last repair
USBUnlocker.bat /portable          # Portable mode
USBUnlocker.bat /debug             # Debug mode
```

## License

For authorized administration use only.
