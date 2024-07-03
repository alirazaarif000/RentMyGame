using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMG.DAL.Migrations
{
    /// <inheritdoc />
    public partial class priceColumnSubscriptionH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePaid",
                table: "SubscriptionsHistory",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePaid",
                table: "SubscriptionsHistory");
        }
    }
}
