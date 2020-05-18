Make sure you adjust your SA Password in the readme before running 

--------
# .env

SQLSERVER_SA_PASSWORD=Str0ngP@ssw0rd
SQL_CONNECTION_STRING=Data Source=db;Initial Catalog=Covid19;trusted_connection=False;User Id=sa;Password=Str0ngP@ssw0rd





# Sample API Calls:

## global stats
- /global
- /global/timesseries

## national stats. {'us' is the slug built for the United States}
- /countries
- /country/us
- /country/us/timeseries
- /country/us/provinces

## state-provincial stats {'wa' is the slug built for the state of Washington in the United States}
- /provinces
- /province/washington
- /province/washington/timeseries
- /province/districts

## district-county stats (Still seems to have a few bugs). {'king' is the slug built for the county of King in the state of Washington in the United States}
- /districts
- /district/king
- /district/king/timeseries
