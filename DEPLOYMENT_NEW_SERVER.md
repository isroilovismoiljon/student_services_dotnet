# Deployment Guide - New Server (89.223.126.116)

## 📦 Files Ready for Deployment

- ✅ `deploy.zip` - Application files
- ✅ `talabajon.service` - Systemd service file
- ✅ This deployment guide

## 🚀 Step-by-Step Deployment

### Step 1: Upload Files to Server

```powershell
# From your Windows machine, run:
scp deploy.zip root@89.223.126.116:/tmp/
scp talabajon.service root@89.223.126.116:/tmp/
```

### Step 2: Connect to Server

```bash
ssh root@89.223.126.116
```

### Step 3: Install Prerequisites

```bash
# Update system
apt-get update

# Install .NET 8 Runtime
apt-get install -y aspnetcore-runtime-8.0

# Install PostgreSQL
apt-get install -y postgresql postgresql-contrib

# Install Nginx
apt-get install -y nginx

# Install unzip (if not available)
apt-get install -y unzip
```

### Step 4: Configure PostgreSQL

```bash
# Switch to postgres user and create database
sudo -u postgres psql << 'EOF'
CREATE DATABASE TalabajonApp;
ALTER USER postgres WITH ENCRYPTED PASSWORD 'Ismoiljon4515';
GRANT ALL PRIVILEGES ON DATABASE TalabajonApp TO postgres;
\q
EOF

# Allow local connections
echo "host    all             all             127.0.0.1/32            md5" | sudo tee -a /etc/postgresql/*/main/pg_hba.conf
systemctl restart postgresql

# Verify PostgreSQL is running
systemctl status postgresql
```

### Step 5: Set Up Application Directory

```bash
# Create directories
mkdir -p /var/www/talabajon
mkdir -p /var/log/talabajon

# Extract application files
cd /var/www/talabajon
unzip -o /tmp/deploy.zip

# Set permissions
chown -R www-data:www-data /var/www/talabajon
chown -R www-data:www-data /var/log/talabajon
chmod +x /var/www/talabajon/StudentServicesWebApi
```

### Step 6: Configure Nginx

```bash
# Create Nginx configuration
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

# Enable site and restart Nginx
ln -sf /etc/nginx/sites-available/talabajon /etc/nginx/sites-enabled/
rm -f /etc/nginx/sites-enabled/default
nginx -t
systemctl restart nginx
systemctl enable nginx
```

### Step 7: Install and Start Service

```bash
# Copy service file
cp /tmp/talabajon.service /etc/systemd/system/

# Reload systemd, enable and start service
systemctl daemon-reload
systemctl enable talabajon.service
systemctl start talabajon.service

# Check service status
systemctl status talabajon.service
```

### Step 8: Configure Firewall

```bash
# Allow necessary ports
ufw allow 22/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw --force enable

# Check firewall status
ufw status
```

## ✅ Verification

### Check Service Status

```bash
systemctl status talabajon.service
```

### View Logs

```bash
# Standard output
tail -f /var/log/talabajon/stdout.log

# Error output
tail -f /var/log/talabajon/stderr.log

# Or use journalctl
journalctl -u talabajon.service -f
```

### Test API

```bash
# Test from server
curl http://localhost:5000

# Test from external (replace with actual endpoint)
curl http://89.223.126.116
```

## 🔄 Common Management Commands

### Restart Service

```bash
systemctl restart talabajon.service
```

### Stop Service

```bash
systemctl stop talabajon.service
```

### Start Service

```bash
systemctl start talabajon.service
```

### View Service Logs

```bash
# Follow logs in real-time
journalctl -u talabajon.service -f

# View last 100 lines
journalctl -u talabajon.service -n 100

# View logs since boot
journalctl -u talabajon.service -b
```

### Check Database Connection

```bash
sudo -u postgres psql -d TalabajonApp -c "SELECT version();"
```

## 🔄 Updating the Application

When you need to deploy updates:

```bash
# On Windows machine:
# 1. Build new version
dotnet publish StudentServicesWebApi/StudentServicesWebApi.csproj -c Release -o publish
Compress-Archive -Path "publish\*" -DestinationPath "deploy.zip" -Force

# 2. Upload to server
scp deploy.zip root@89.223.126.116:/tmp/

# On Server:
# 3. Stop service
systemctl stop talabajon.service

# 4. Backup current version (optional)
cp -r /var/www/talabajon /var/www/talabajon.backup

# 5. Extract new version
cd /var/www/talabajon
unzip -o /tmp/deploy.zip

# 6. Set permissions
chown -R www-data:www-data /var/www/talabajon

# 7. Start service
systemctl start talabajon.service

# 8. Verify
systemctl status talabajon.service
```

## 🔧 Configuration Files

### Application Settings
Location: `/var/www/talabajon/appsettings.json`

Key settings:
- Database connection string
- JWT secret key
- Telegram bot token
- File upload paths

### Service File
Location: `/etc/systemd/system/talabajon.service`

### Nginx Configuration
Location: `/etc/nginx/sites-available/talabajon`

## 📊 Monitoring

### Check Disk Space

```bash
df -h
```

### Check Memory Usage

```bash
free -h
```

### Check Process

```bash
ps aux | grep StudentServicesWebApi
```

### Check Network Ports

```bash
netstat -tlnp | grep :5000
netstat -tlnp | grep :80
```

## 🐛 Troubleshooting

### Service Won't Start

```bash
# Check detailed error logs
journalctl -u talabajon.service -n 50 --no-pager

# Check file permissions
ls -la /var/www/talabajon

# Check if port 5000 is in use
netstat -tlnp | grep :5000
```

### Database Connection Issues

```bash
# Test PostgreSQL connection
sudo -u postgres psql -d TalabajonApp

# Check PostgreSQL status
systemctl status postgresql

# View PostgreSQL logs
tail -f /var/log/postgresql/postgresql-*-main.log
```

### Nginx Issues

```bash
# Test Nginx configuration
nginx -t

# Check Nginx logs
tail -f /var/log/nginx/error.log
tail -f /var/log/nginx/access.log

# Restart Nginx
systemctl restart nginx
```

## 🔒 Security Notes

1. **Database Password**: Currently using `Ismoiljon4515` - consider changing in production
2. **JWT Secret**: Update in `appsettings.json` for production
3. **Firewall**: Only ports 22, 80, 443 are open
4. **SSL/HTTPS**: Consider adding SSL certificate using Let's Encrypt:

```bash
apt-get install -y certbot python3-certbot-nginx
certbot --nginx -d yourdomain.com
```

## 📝 Important Paths

| Component | Path |
|-----------|------|
| Application | `/var/www/talabajon/` |
| Logs | `/var/log/talabajon/` |
| Service File | `/etc/systemd/system/talabajon.service` |
| Nginx Config | `/etc/nginx/sites-available/talabajon` |
| Database Data | `/var/lib/postgresql/` |

## 🌐 Access Information

- **Server IP**: 89.223.126.116
- **HTTP Port**: 80
- **Application Port** (internal): 5000
- **Access URL**: http://89.223.126.116

## ✨ Post-Deployment Checklist

- [ ] Service is running (`systemctl status talabajon.service`)
- [ ] Nginx is configured and running
- [ ] Database is created and accessible
- [ ] Firewall rules are configured
- [ ] Logs are being written to `/var/log/talabajon/`
- [ ] API is accessible via http://89.223.126.116
- [ ] Telegram bot is connecting successfully
- [ ] File upload directories exist and are writable
- [ ] Service auto-starts on reboot (`systemctl is-enabled talabajon.service`)
