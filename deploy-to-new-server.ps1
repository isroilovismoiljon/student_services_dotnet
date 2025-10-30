# Deploy StudentServicesWebApi to New Server
# Target Server: 89.223.126.116

$SERVER_IP = "89.223.126.116"
$SERVER_USER = "root"
$APP_DIR = "/var/www/talabajon"
$SERVICE_NAME = "talabajon"

Write-Host "================================" -ForegroundColor Cyan
Write-Host "Deploying to: $SERVER_IP" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan

# Step 1: Build and Publish the project
Write-Host "`n[1/7] Building and publishing project..." -ForegroundColor Yellow
dotnet publish StudentServicesWebApi/StudentServicesWebApi.csproj -c Release -o publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✓ Build completed successfully" -ForegroundColor Green

# Step 2: Create deployment package
Write-Host "`n[2/7] Creating deployment package..." -ForegroundColor Yellow
if (Test-Path "deploy.zip") {
    Remove-Item "deploy.zip"
}
Compress-Archive -Path "publish\*" -DestinationPath "deploy.zip"
Write-Host "✓ Deployment package created" -ForegroundColor Green

# Step 3: Upload files to server
Write-Host "`n[3/7] Uploading files to server..." -ForegroundColor Yellow
Write-Host "Run these commands manually:" -ForegroundColor Cyan
Write-Host ""
Write-Host "scp deploy.zip ${SERVER_USER}@${SERVER_IP}:/tmp/" -ForegroundColor White
Write-Host "scp talabajon.service ${SERVER_USER}@${SERVER_IP}:/tmp/" -ForegroundColor White
Write-Host ""
Write-Host "Press Enter when upload is complete..." -ForegroundColor Yellow
Read-Host

# Display server setup commands
Write-Host "`n[4/7] Server Setup Commands" -ForegroundColor Yellow
Write-Host "Connect to server and run these commands:" -ForegroundColor Cyan
Write-Host ""
Write-Host "ssh ${SERVER_USER}@${SERVER_IP}" -ForegroundColor White
Write-Host ""

$serverCommands = @"
# Install .NET 8 Runtime (if not installed)
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0 --runtime aspnetcore
export PATH="$HOME/.dotnet:$PATH"
echo 'export PATH="$HOME/.dotnet:$PATH"' >> ~/.bashrc

# Or use apt (recommended)
apt-get update
apt-get install -y aspnetcore-runtime-8.0

# Create application directory
mkdir -p $APP_DIR
mkdir -p /var/log/$SERVICE_NAME

# Extract application files
cd $APP_DIR
unzip -o /tmp/deploy.zip

# Set permissions
chown -R www-data:www-data $APP_DIR
chown -R www-data:www-data /var/log/$SERVICE_NAME
chmod +x $APP_DIR/StudentServicesWebApi

# Install PostgreSQL (if not installed)
apt-get install -y postgresql postgresql-contrib

# Configure PostgreSQL
sudo -u postgres psql << EOF
CREATE DATABASE TalabajonApp;
CREATE USER postgres WITH ENCRYPTED PASSWORD 'Ismoiljon4515';
GRANT ALL PRIVILEGES ON DATABASE TalabajonApp TO postgres;
\q
EOF

# Update PostgreSQL to allow local connections
echo "host    all             all             127.0.0.1/32            md5" | sudo tee -a /etc/postgresql/*/main/pg_hba.conf
systemctl restart postgresql

# Install and configure Nginx
apt-get install -y nginx

# Create Nginx configuration
cat > /etc/nginx/sites-available/$SERVICE_NAME << 'NGINX_EOF'
server {
    listen 80;
    server_name 89.223.126.116;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_set_header X-Real-IP \$remote_addr;
        client_max_body_size 100M;
    }
}
NGINX_EOF

# Enable Nginx site
ln -sf /etc/nginx/sites-available/$SERVICE_NAME /etc/nginx/sites-enabled/
rm -f /etc/nginx/sites-enabled/default
nginx -t
systemctl restart nginx

# Install systemd service
cp /tmp/talabajon.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable $SERVICE_NAME.service
systemctl start $SERVICE_NAME.service

# Configure firewall
ufw allow 22/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw --force enable

# Check status
systemctl status $SERVICE_NAME.service
"@

Write-Host $serverCommands -ForegroundColor White

Write-Host "`n[5/7] Saving server commands to file..." -ForegroundColor Yellow
$serverCommands | Out-File -FilePath "server-setup-commands.sh" -Encoding UTF8
Write-Host "✓ Commands saved to: server-setup-commands.sh" -ForegroundColor Green

Write-Host "`n[6/7] Quick Deployment Commands" -ForegroundColor Yellow
Write-Host "Copy and paste these on the server:" -ForegroundColor Cyan
Write-Host ""
Write-Host "cd /var/www/talabajon; unzip -o /tmp/deploy.zip; systemctl restart talabajon.service" -ForegroundColor White
Write-Host ""

Write-Host "`n[7/7] Verification Commands" -ForegroundColor Yellow
Write-Host "After deployment, verify with:" -ForegroundColor Cyan
Write-Host ""
Write-Host "# Check service status" -ForegroundColor Gray
Write-Host "systemctl status $SERVICE_NAME.service" -ForegroundColor White
Write-Host ""
Write-Host "# View logs" -ForegroundColor Gray
Write-Host "tail -f /var/log/$SERVICE_NAME/stdout.log" -ForegroundColor White
Write-Host "tail -f /var/log/$SERVICE_NAME/stderr.log" -ForegroundColor White
Write-Host ""
Write-Host "# Test API" -ForegroundColor Gray
Write-Host "curl http://localhost:5000/api/health" -ForegroundColor White
Write-Host "curl http://$SERVER_IP" -ForegroundColor White
Write-Host ""

Write-Host "`n================================" -ForegroundColor Cyan
Write-Host "Deployment package ready!" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Cyan
Write-Host "`nFiles created:" -ForegroundColor Yellow
Write-Host "  - deploy.zip" -ForegroundColor White
Write-Host "  - talabajon.service" -ForegroundColor White
Write-Host "  - server-setup-commands.sh" -ForegroundColor White
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Upload files: scp deploy.zip talabajon.service root@89.223.126.116:/tmp/" -ForegroundColor White
Write-Host "  2. SSH to server: ssh root@89.223.126.116" -ForegroundColor White
Write-Host "  3. Run the commands from server-setup-commands.sh" -ForegroundColor White
Write-Host ""
