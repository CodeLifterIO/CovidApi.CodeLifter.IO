#!/bin/bash
# Create a new migration
# dotnet ef migrations add {Migration Name}

echo '--- Please enter a migration name:'
read migration_name

echo "Creating migration named ${migration_name}"

dotnet ef migrations add $migration_name

echo "Migration named ${migration_name} created"