@echo off
echo ========================================
echo MANUAL DEPLOYMENT SCRIPT
echo ========================================
echo.
echo Package: deploy_20251101_173925.zip
echo Server: 89.223.126.116
echo.
echo STEP 1: Upload Package
echo ------------------------
echo Running upload command...
echo You will be prompted for the server password.
echo.
scp deploy_20251101_173925.zip root@89.223.126.116:/tmp/deploy_latest.zip
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Upload failed! Please check:
    echo - Server is accessible
    echo - Password is correct
    echo - SSH service is running on server
    pause
    exit /b 1
)

echo.
echo Upload successful!
echo.
echo STEP 2: Deploy on Server
echo ------------------------
echo Connecting to server to deploy...
echo.

ssh root@89.223.126.116 "sudo systemctl stop talabajon && cd /tmp && sudo rm -rf /var/www/talabajon/* && sudo unzip -o deploy_latest.zip -d /var/www/talabajon && sudo chown -R www-data:www-data /var/www/talabajon && sudo chmod -R 755 /var/www/talabajon && sudo mkdir -p /var/www/talabajon/wwwroot/uploads/payment-receipts && sudo mkdir -p /var/www/talabajon/wwwroot/uploads/presentation-files && sudo chown -R www-data:www-data /var/www/talabajon/wwwroot && sudo systemctl start talabajon && sleep 2 && sudo systemctl status talabajon --no-pager"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Deployment had issues! Check the output above.
    pause
    exit /b 1
)

echo.
echo ========================================
echo DEPLOYMENT COMPLETED!
echo ========================================
echo.
echo Service should be running now.
echo Check logs with: ssh root@89.223.126.116 "sudo journalctl -u talabajon -n 50"
echo.
pause
