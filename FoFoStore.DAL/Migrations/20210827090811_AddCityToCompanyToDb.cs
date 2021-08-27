using Microsoft.EntityFrameworkCore.Migrations;

namespace FoFoStore.DAL.Migrations
{
    public partial class AddCityToCompanyToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "companies");
        }
    }
}
