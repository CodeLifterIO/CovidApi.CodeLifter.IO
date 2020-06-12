using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class SummaryStoredProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Update_Summary_On_Country 
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

								DECLARE @sourceFile varchar(10);
								SELECT @sourceFile = (SELECT TOP (1) REPLACE([FileName], '.csv', '') 
													  FROM [Covid19].[dbo].[DataCollectionStatistics]
													  ORDER BY Id DESC);

								INSERT INTO Totals (CountryId, Count, Confirmed, Deaths, Recovered, Active, SourceFile)
								SELECT	
									dp.CountryId AS CountryId,
									COUNT(*) AS Count,
									SUM(dp.Confirmed) AS Confirmed,
                                    SUM(dp.Deaths) AS Deaths,
                                    SUM(dp.Recovered) AS Recovered,
                                    SUM(dp.Active) AS Active,
									dp.SourceFile AS SourceFile
                                FROM dbo.DataPoints dp
                                WHERE dp.SourceFile = @sourceFile
                                GROUP BY dp.CountryId, dp.SourceFile

                                END
                                GO
                                ");

            migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Update_Summary_On_Province 
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

								DECLARE @sourceFile varchar(10);
								SELECT @sourceFile = (SELECT TOP (1) REPLACE([FileName], '.csv', '') 
													  FROM [Covid19].[dbo].[DataCollectionStatistics]
													  ORDER BY Id DESC);

								INSERT INTO Totals (ProvinceId, Count, Confirmed, Deaths, Recovered, Active, SourceFile)
								SELECT	
									dp.ProvinceId AS CountryId,
									COUNT(*) AS Count,
									SUM(dp.Confirmed) AS Confirmed,
                                    SUM(dp.Deaths) AS Deaths,
                                    SUM(dp.Recovered) AS Recovered,
                                    SUM(dp.Active) AS Active,
									dp.SourceFile AS SourceFile
                                FROM dbo.DataPoints dp
                                WHERE dp.SourceFile = @sourceFile
                                GROUP BY dp.ProvinceId, dp.SourceFile

                                END
                                GO
                                ");

            migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Update_Summary_On_District 
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

								DECLARE @sourceFile varchar(10);
								SELECT @sourceFile = (SELECT TOP (1) REPLACE([FileName], '.csv', '') 
													  FROM [Covid19].[dbo].[DataCollectionStatistics]
													  ORDER BY Id DESC);

								INSERT INTO Totals (DistrictId, Count, Confirmed, Deaths, Recovered, Active, SourceFile)
								SELECT	
									dp.DistrictId AS CountryId,
									COUNT(*) AS Count,
									SUM(dp.Confirmed) AS Confirmed,
                                    SUM(dp.Deaths) AS Deaths,
                                    SUM(dp.Recovered) AS Recovered,
                                    SUM(dp.Active) AS Active,
									dp.SourceFile AS SourceFile
                                FROM dbo.DataPoints dp
                                WHERE dp.SourceFile = @sourceFile
                                GROUP BY dp.DistrictId, dp.SourceFile


                                END
                                GO
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE SP_Update_Summary_On_Country;");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_Update_Summary_On_Province;");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_Update_Summary_On_District;");
        }
    }
}
