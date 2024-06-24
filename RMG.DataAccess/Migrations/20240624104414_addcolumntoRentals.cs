using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMG.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcolumntoRentals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionHistoryId",
                table: "Rentals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_SubscriptionHistoryId",
                table: "Rentals",
                column: "SubscriptionHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_SubscriptionsHistory_SubscriptionHistoryId",
                table: "Rentals",
                column: "SubscriptionHistoryId",
                principalTable: "SubscriptionsHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_SubscriptionsHistory_SubscriptionHistoryId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_SubscriptionHistoryId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "SubscriptionHistoryId",
                table: "Rentals");
        }
    }
}
