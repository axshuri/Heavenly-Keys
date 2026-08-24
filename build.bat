@echo off
echo ============================================================
echo                    USBUnlocker Build
echo ============================================================
echo.

:: Check for .NET SDK or MSBuild
where dotnet >nul 2>&1
if %errorlevel%==0 (
    echo Using .NET CLI...
    echo.
    
    :: Clean
    echo Cleaning previous build...
    dotnet clean USBUnlocker.csproj -c Release -v q
    
    :: Build
    echo Building USBUnlocker...
    dotnet publish USBUnlocker.csproj -c Release -r win-x64 --self-contained true -o dist
    
    if %errorlevel%==0 (
        echo.
        echo ============================================================
        echo BUILD SUCCESSFUL
        echo ============================================================
        echo.
        echo Output: dist\USBUnlocker.exe
        echo.
    ) else (
        echo.
        echo BUILD FAILED
        echo.
    )
) else (
    echo .NET CLI not found. Trying MSBuild...
    echo.
    
    where msbuild >nul 2>&1
    if %errorlevel%==0 (
        msbuild USBUnlocker.csproj /p:Configuration=Release /p:OutputPath=dist
    ) else (
        echo.
        echo ERROR: No build tool found.
        echo.
        echo Please install:
        echo   - .NET SDK 6.0+ (recommended)
        echo   - Or Visual Studio with .NET desktop development
        echo.
    )
)

echo.
echo Press any key to exit...
pause >nul
