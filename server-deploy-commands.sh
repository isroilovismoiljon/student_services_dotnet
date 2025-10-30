#!/bin/bash
# Quick Deployment Script for 89.223.126.116
# Run this on the server after uploading deploy.zip and talabajon.service

set -e

echo "================================"
echo "Talabajon Deployment Script"
echo "================================"

# Install prerequisites
echo "[1/8] Installing prerequisites..."
apt-get update
apt-get install -y aspnetcore-runtime-8.0 postgresql postgresql-contrib nginx unzip

# Configure PostgreSQL
echo "[2/8] Configuring PostgreSQL..."
sudo -u postgres psql << 'EOF'
CREATE DATABASE TalabajonApp;
ALTER USER postgres WITH ENCRYPTED PASSWORD 'Ismoiljon4515';
GRANT ALL PRIVILEGES ON DATABASE TalabajonApp TO postgres;
\q
EOF

echo "host    all             all             127.0.0.1/32            md5" | tee -a /etc/postgresql/*/main/pg_hba.conf
systemctl restart postgresql

# Create directories
echo "[3/8] Creating application directories..."
mkdir -p /var/www/talabajon
mkdir -p /var/log/talabajon

# Extract application
echo "[4/8] Extracting application files..."
cd /var/www/talabajon
unzip -o /tmp/deploy.zip

# Set permissions
echo "[5/8] Setting permissions..."
chown -R www-data:www-data /var/www/talabajon
chown -R www-data:www-data /var/log/talabajon
chmod +x /var/www/talabajon/StudentServicesWebApi

# Configure Nginx
echo "[6/8] Configuring Nginx..."
cat > /etc/nginx/sites-available/talabajon << 'EOF'
server {
    listen 80;
    server_name 89.223.126.116;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        client_max_body_size 100M;
    }
}
EOF

ln -sf /etc/nginx/sites-available/talabajon /etc/nginx/sites-enabled/
rm -f /etc/nginx/sites-enabled/default
nginx -t
systemctl restart nginx
systemctl enable nginx

# Install and start service
echo "[7/8] Installing systemd service..."
cp /tmp/talabajon.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable talabajon.service
systemctl start talabajon.service

# Configure firewall
echo "[8/8] Configuring firewall..."
ufw allow 22/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw --force enable

echo "================================"
echo "Deployment Complete!"
echo "================================"
echo ""
echo "Service Status:"
systemctl status talabajon.service --no-pager
echo ""
echo "Access your application at: http://89.223.126.116"
echo ""
echo "Useful commands:"
echo "  View logs:    journalctl -u talabajon.service -f"
echo "  Restart:      systemctl restart talabajon.service"
echo "  Stop:         systemctl stop talabajon.service"
echo "  Start:        systemctl start talabajon.service"
