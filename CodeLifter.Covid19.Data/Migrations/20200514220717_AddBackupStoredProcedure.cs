using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class AddBackupStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE SP_Backup_Database
AS
BEGIN
	DECLARE @databaseName varchar(7);
	DECLARE @backupFileName varchar(11);
	DECLARE @fullPath varchar(68);

	SELECT @databaseName = DB_NAME();
	SELECT @backupFileName = CONCAT(@databaseName, '.bak');
	SELECT @fullPath = CONCAT('/var/opt/mssql/data/', @backupFileName);

	BACKUP DATABASE @databaseName
		TO DISK = @fullPath
	WITH FORMAT,
		MEDIANAME = 'SQLServerBackups',
		NAME = @backupFileName;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP PROCEDURE SP_Backup_Database;");
        }
    }
}
