# Deployment Guide

## Files Created
- `deploy_20251101_173925.zip` - Ready to deploy

## Deployment Steps

### Option 1: Using WinSCP (Recommended for Windows)

1. **Open WinSCP** and connect to:
   - Host: `89.223.126.116`
   - User: `root`
   - Password: (your server password)

2. **Upload the file:**
   - Navigate to `/tmp/` on the server
   - Upload `deploy_20251101_173925.zip`

3. **Connect via PuTTY/SSH** to the server:
   ```bash
   ssh root@89.223.126.116
   ```

4. **Run deployment commands:**
   ```bash
   # Stop the service
   sudo systemctl stop talabajon
   
   # Backup current version (optional)
   sudo mv /var/www/talabajon /var/www/talabajon_backup_$(date +%Y%m%d_%H%M%S)
   
   # Create directory
   sudo mkdir -p /var/www/talabajon
   
   # Extract new version
   sudo unzip -o /tmp/deploy_20251101_173925.zip -d /var/www/talabajon
   
   # Set permissions
   sudo chown -R www-data:www-data /var/www/talabajon
   sudo chmod -R 755 /var/www/talabajon
   
   # Ensure upload directories exist
   sudo mkdir -p /var/www/talabajon/wwwroot/uploads/payment-receipts
   sudo mkdir -p /var/www/talabajon/wwwroot/uploads/presentation-files
   sudo chown -R www-data:www-data /var/www/talabajon/wwwroot
   
   # Start the service
   sudo systemctl start talabajon
   
   # Check status
   sudo systemctl status talabajon
   
   # View logs if needed
   sudo journalctl -u talabajon -f
   ```

### Option 2: Direct SSH Upload

If you have SSH key authentication set up:

```powershell
# Upload
scp deploy_20251101_173925.zip root@89.223.126.116:/tmp/

# Deploy
ssh root@89.223.126.116 "
  sudo systemctl stop talabajon && \
  sudo mkdir -p /var/www/talabajon && \
  sudo unzip -o /tmp/deploy_20251101_173925.zip -d /var/www/talabajon && \
  sudo chown -R www-data:www-data /var/www/talabajon && \
  sudo systemctl start talabajon && \
  sudo systemctl status talabajon
"
```

## Verify Deployment

After deployment, verify the application is running:

1. Check service status:
   ```bash
   sudo systemctl status talabajon
   ```

2. Check logs:
   ```bash
   sudo journalctl -u talabajon -n 50
   ```

3. Test the API:
   ```bash
   curl http://89.223.126.116:5000/swagger
   ```
   Or visit in browser: `http://89.223.126.116:5000/swagger`

## Rollback (if needed)

If something goes wrong:

```bash
# Stop current version
sudo systemctl stop talabajon

# Restore backup
sudo rm -rf /var/www/talabajon
sudo mv /var/www/talabajon_backup_YYYYMMDD_HHMMSS /var/www/talabajon

# Start service
sudo systemctl start talabajon
```

## Common Issues

### Service won't start
```bash
# Check logs
sudo journalctl -u talabajon -n 100

# Check if port is in use
sudo netstat -tlnp | grep :5000

# Check file permissions
ls -la /var/www/talabajon/
```

### Database connection issues
```bash
# Check PostgreSQL is running
sudo systemctl status postgresql

# Test connection
sudo -u postgres psql -c "\l"
```

### File upload issues
```bash
# Ensure upload directories have correct permissions
sudo chown -R www-data:www-data /var/www/talabajon/wwwroot
sudo chmod -R 755 /var/www/talabajon/wwwroot
```

## Configuration Notes

Make sure the following are configured on the server:

1. **appsettings.Production.json** - Database connection, JWT secret, Telegram bot token
2. **Service file** - `/etc/systemd/system/talabajon.service`
3. **Nginx** - Reverse proxy configuration (if used)
4. **PostgreSQL** - Database must be running and migrations applied

## Post-Deployment

After successful deployment:

1. Run database migrations if needed:
   ```bash
   cd /var/www/talabajon
   sudo -u www-data dotnet StudentServicesWebApi.dll --migrate
   ```

2. Clear any cached data if applicable

3. Notify users of any downtime

## Files Included in Deployment

The deployment package includes:
- StudentServicesWebApi.dll and dependencies
- appsettings.json and appsettings.Production.json
- wwwroot folder structure
- All required .NET runtime files
