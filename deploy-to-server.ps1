# Deploy to Server Script
# Server: 89.223.126.116
# Folder: talabajon

param(
    [string]$ServerIP = "89.223.126.116",
    [string]$ServerUser = "root",
    [string]$RemoteFolder = "/var/www/talabajon",
    [string]$ServiceName = "talabajon"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deploying StudentServicesWebApi" -ForegroundColor Cyan
Write-Host "Server: $ServerIP" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# Step 1: Clean previous publish
Write-Host "`n[1/6] Cleaning previous publish..." -ForegroundColor Yellow
if (Test-Path ".\publish") {
    Remove-Item -Path ".\publish" -Recurse -Force
}

# Step 2: Build and publish the application
Write-Host "`n[2/6] Building and publishing application..." -ForegroundColor Yellow
dotnet publish StudentServicesWebApi/StudentServicesWebApi.csproj `
    -c Release `
    -o publish `
    --self-contained false `
    -p:PublishSingleFile=false

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Build successful!" -ForegroundColor Green

# Step 3: Create deployment package
Write-Host "`n[3/6] Creating deployment package..." -ForegroundColor Yellow
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$archiveName = "deploy_$timestamp.zip"

Compress-Archive -Path ".\publish\*" -DestinationPath $archiveName -Force
Write-Host "Package created: $archiveName" -ForegroundColor Green

# Step 4: Upload to server
Write-Host "`n[4/6] Uploading to server..." -ForegroundColor Yellow
Write-Host "You will need to enter the server password" -ForegroundColor Cyan

# Upload the archive
scp $archiveName "${ServerUser}@${ServerIP}:/tmp/"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Upload failed!" -ForegroundColor Red
    exit 1
}

# Step 5: Deploy on server
Write-Host "`n[5/6] Deploying on server..." -ForegroundColor Yellow

$deployScript = @"
#!/bin/bash
set -e

echo 'Stopping service...'
sudo systemctl stop $ServiceName || true

echo 'Backing up current deployment...'
if [ -d '$RemoteFolder' ]; then
    sudo mv $RemoteFolder ${RemoteFolder}_backup_$timestamp || true
fi

echo 'Creating directory...'
sudo mkdir -p $RemoteFolder

echo 'Extracting new version...'
sudo unzip -o /tmp/$archiveName -d $RemoteFolder

echo 'Setting permissions...'
sudo chown -R www-data:www-data $RemoteFolder
sudo chmod -R 755 $RemoteFolder

echo 'Creating uploads directories...'
sudo mkdir -p $RemoteFolder/wwwroot/uploads/payment-receipts
sudo mkdir -p $RemoteFolder/wwwroot/uploads/presentation-files
sudo chown -R www-data:www-data $RemoteFolder/wwwroot
sudo chmod -R 755 $RemoteFolder/wwwroot

echo 'Starting service...'
sudo systemctl start $ServiceName

echo 'Checking service status...'
sudo systemctl status $ServiceName --no-pager

echo 'Cleaning up...'
rm /tmp/$archiveName

echo 'Deployment completed successfully!'
"@

$deployScript | ssh "${ServerUser}@${ServerIP}" "cat > /tmp/deploy.sh && chmod +x /tmp/deploy.sh && /tmp/deploy.sh"

# Step 6: Verify deployment
Write-Host "`n[6/6] Verifying deployment..." -ForegroundColor Yellow
Start-Sleep -Seconds 3

Write-Host "`nChecking service status..." -ForegroundColor Cyan
ssh "${ServerUser}@${ServerIP}" "sudo systemctl status $ServiceName --no-pager | head -n 20"

# Cleanup local files
Write-Host "`n[Cleanup] Removing local deployment files..." -ForegroundColor Yellow
Remove-Item $archiveName -Force

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "`nYour application should be running on the server." -ForegroundColor Cyan
Write-Host "Check logs with: ssh ${ServerUser}@${ServerIP} 'sudo journalctl -u $ServiceName -f'" -ForegroundColor Cyan
