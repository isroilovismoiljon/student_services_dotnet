# Quick Deploy Script
Write-Host "Building application..." -ForegroundColor Cyan

# Stop local app
Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue

# Clean and build
if (Test-Path ".\publish") { Remove-Item -Path ".\publish" -Recurse -Force }

# Publish
dotnet publish StudentServicesWebApi/StudentServicesWebApi.csproj -c Release -o publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Create archive
$archiveName = "deploy_$(Get-Date -Format 'yyyyMMdd_HHmmss').zip"
Compress-Archive -Path ".\publish\*" -DestinationPath $archiveName -Force

Write-Host "`nDeployment package created: $archiveName" -ForegroundColor Green
Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "1. Upload to server:" -ForegroundColor White
Write-Host "   scp $archiveName root@89.223.126.116:/tmp/" -ForegroundColor Cyan
Write-Host "`n2. SSH to server:" -ForegroundColor White
Write-Host "   ssh root@89.223.126.116" -ForegroundColor Cyan
Write-Host "`n3. Run on server:" -ForegroundColor White
Write-Host @"
   sudo systemctl stop talabajon
   sudo rm -rf /var/www/talabajon/*
   sudo unzip -o /tmp/$archiveName -d /var/www/talabajon
   sudo chown -R www-data:www-data /var/www/talabajon
   sudo systemctl start talabajon
   sudo systemctl status talabajon
"@ -ForegroundColor Cyan
