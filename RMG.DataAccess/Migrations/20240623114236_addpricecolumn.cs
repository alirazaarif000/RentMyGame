using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMG.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addpricecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Subscriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Subscriptions");
        }
    }
}
