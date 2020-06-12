#!/bin/bash

docker cp covidapi-codelifter-data:/var/opt/mssql/data/backups/Covid19.bak ./Bak/Covid19.bak
docker cp covidapi-codelifter-data:/var/opt/mssql/data/backups/Covid19.bak covidapi-codelifter-data:/var/opt/mssql/data/Covid19.bak