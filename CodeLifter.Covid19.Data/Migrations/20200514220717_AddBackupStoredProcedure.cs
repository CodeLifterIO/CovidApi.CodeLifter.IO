using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class AddBackupStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_Backup_Database
									AS
									BEGIN
										DECLARE @backupDate varchar(10);
												DECLARE @backupTime varchar(16);
												DECLARE @backupFileName varchar(68);
												DECLARE @databaseName varchar(8);
												DECLARE @fullPath varchar(68);
												DECLARE @constantPath varchar(68);

												SELECT @backupDate = MAX(SourceFile) FROM DataPoints;
												SELECT @backupTime = CONVERT(time, CURRENT_TIMESTAMP);
												SELECT @databaseName = DB_NAME();
												SELECT @backupFileName = CONCAT(@databaseName, '.bak');
												SELECT @fullPath = CONCAT('/var/opt/mssql/data/backups/', @backupFileName);
												SELECT @constantPath = CONCAT('/var/opt/mssql/data/backups/', @databaseName, '.bak');

												BACKUP DATABASE @databaseName
												TO DISK = @fullPath
										   WITH FORMAT,
											  MEDIANAME = 'SQLServerBackups',
											  NAME = @backupFileName;

												BACKUP DATABASE @databaseName
												TO DISK = @constantPath
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
