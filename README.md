If you want to use the admin features and download new data from github you will need to populate .env and add your github auth token and a GUID to ADMIN_AUTH_TOKEN

--------
# .env

SQLSERVER_SA_PASSWORD=Str0ngP@ssw0rd
SQL_CONNECTION_STRING=Data Source=db;Initial Catalog=Covid19;trusted_connection=False;User Id=sa;Password=Str0ngP@ssw0rd
GITHUB_AUTH_TOKEN=
ADMIN_AUTH_TOKEN=