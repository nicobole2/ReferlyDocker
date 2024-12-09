
sleep 20s

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P YourStrongPassword123! -d master -i setup.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P YourStrongPassword123! -d Referly -i data.sql
