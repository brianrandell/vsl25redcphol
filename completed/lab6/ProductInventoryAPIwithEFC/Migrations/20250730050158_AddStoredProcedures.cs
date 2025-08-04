using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductInventoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create stored procedure to get products by category with pagination
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetProductsByCategory
                    @CategoryId INT,
                    @PageNumber INT,
                    @PageSize INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                    
                    SELECT p.Id, p.Name, p.Description, p.Price, p.StockQuantity, 
                           p.Category, p.CategoryId, p.IsActive
                    FROM Products p
                    WHERE p.CategoryId = @CategoryId AND p.IsActive = 1
                    ORDER BY p.Name
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                END
            ");

            // Create stored procedure for inventory report
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetInventoryReport
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    SELECT 
                        COUNT(*) as TotalProducts,
                        SUM(p.Price * p.StockQuantity) as TotalInventoryValue,
                        SUM(CASE WHEN p.StockQuantity < 10 THEN 1 ELSE 0 END) as LowStockItemsCount,
                        (SELECT TOP 1 c.Name 
                         FROM Categories c 
                         INNER JOIN Products p2 ON c.Id = p2.CategoryId 
                         WHERE p2.IsActive = 1 
                         GROUP BY c.Name 
                         ORDER BY COUNT(*) DESC) as TopCategory
                    FROM Products p
                    WHERE p.IsActive = 1;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetProductsByCategory");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetInventoryReport");
        }
    }
}
