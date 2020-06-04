using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class SummaryDataStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Update_Summary_On_Country 
                                    @sourceFile varchar(10)
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

                                UPDATE C	
                                SET
                                    C.Confirmed = S.Confirmed,
                                    C.Deaths = S.Deaths, 
                                    C.Recovered = S.Recovered, 
                                    C.Active = S.Active
                                FROM
                                    Countries C
                                    INNER JOIN (SELECT SUM(dp.Confirmed) AS Confirmed,
                                                SUM(dp.Deaths) AS Deaths,
                                                SUM(dp.Recovered) AS Recovered,
                                                SUM(dp.Active) AS Active,
                                                dp.CountryId AS Id
                                            FROM dbo.DataPoints dp
                                            WHERE dp.SourceFile = @sourceFile
                                            GROUP BY dp.CountryId) S
                                        ON C.Id = S.Id
                                END
                                GO
                                ");

                        migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Update_Summary_On_Province 
                                    @sourceFile varchar(10)
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

                                UPDATE P	
                                SET
                                    P.Confirmed = S.Confirmed,
                                    P.Deaths = S.Deaths, 
                                    P.Recovered = S.Recovered, 
                                    P.Active = S.Active
                                FROM
                                    Provinces P
                                    INNER JOIN (SELECT SUM(dp.Confirmed) AS Confirmed,
                                                SUM(dp.Deaths) AS Deaths,
                                                SUM(dp.Recovered) AS Recovered,
                                                SUM(dp.Active) AS Active,
                                                dp.ProvinceId AS Id
                                            FROM dbo.DataPoints dp
                                            WHERE dp.SourceFile = @sourceFile
                                            GROUP BY dp.ProvinceId) S
                                        ON P.Id = S.Id
                                END
                                GO
                                ");

                        migrationBuilder.Sql(@"
                                SET ANSI_NULLS ON
                                GO
                                SET QUOTED_IDENTIFIER ON
                                GO
                                CREATE PROCEDURE SP_Update_Summary_On_District 
                                    @sourceFile varchar(10)
                                AS
                                BEGIN
                                    -- SET NOCOUNT ON added to prevent extra result sets from
                                    -- interfering with SELECT statements.
                                    SET NOCOUNT ON;

                                UPDATE D	
                                SET
                                    D.Confirmed = S.Confirmed,
                                    D.Deaths = S.Deaths, 
                                    D.Recovered = S.Recovered, 
                                    D.Active = S.Active
                                FROM
                                    Districts D
                                    INNER JOIN (SELECT SUM(dp.Confirmed) AS Confirmed,
                                                SUM(dp.Deaths) AS Deaths,
                                                SUM(dp.Recovered) AS Recovered,
                                                SUM(dp.Active) AS Active,
                                                dp.DistrictId AS Id
                                            FROM dbo.DataPoints dp
                                            WHERE dp.SourceFile = @sourceFile
                                            GROUP BY dp.DistrictId) S
                                        ON D.Id = S.Id
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
