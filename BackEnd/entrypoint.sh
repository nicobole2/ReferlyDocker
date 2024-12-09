#!/bin/bash
set -e

# Wait for SQL Server to be ready
sleep 30

# Initialize the database
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "Referly2023!" -i /docker-entrypoint-initdb.d/Referly-1.sql

# Start SQL Server
/opt/mssql/bin/sqlservr
