@echo off
echo ==============================
echo 🔄 Cleaning bin and obj folders...
echo ==============================

:: Delete bin and obj folders
for /d /r %%i in (bin obj) do (
    if exist "%%i" (
        echo Deleting %%i
        rmdir /s /q "%%i"
    )
)

echo ==============================
echo 📦 Restoring NuGet packages...
echo ==============================
dotnet restore

echo ==============================
echo 🛠️ Building project...
echo ==============================
dotnet build

echo ==============================
echo ✅ Done!
echo ==============================
pause
