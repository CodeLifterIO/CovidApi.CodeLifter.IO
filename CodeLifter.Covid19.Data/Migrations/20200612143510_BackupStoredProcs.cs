using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class BackupStoredProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Backup_Database 
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

								DECLARE @sourceFile varchar(14);
								SELECT @sourceFile = (SELECT TOP (1) FileName
													  FROM [Covid19].[dbo].[DataCollectionStatistics]
													  ORDER BY Id DESC);

                                DECLARE @fullPath varchar(100);
                                SELECT @fullPath = CONCAT('/var/opt/mssql/data/backups/', @sourceFile);

								DECLARE @databaseName varchar(8);
								SELECT @databaseName = DB_NAME();

                                BACKUP DATABASE @databaseName
                                TO DISK = @fullPath
                                WITH FORMAT,
                                MEDIANAME = 'SQLServerBackups',
                                NAME = @sourceFile;

                                END
                                GO
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE SP_Backup_Database;");
        }
    }
}
