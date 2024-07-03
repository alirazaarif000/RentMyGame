using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMG.DAL.Migrations
{
    /// <inheritdoc />
    public partial class subscribedDateandMonthColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemainingMonths",
                table: "SubscriptionsHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscribedDate",
                table: "SubscriptionsHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingMonths",
                table: "SubscriptionsHistory");

            migrationBuilder.DropColumn(
                name: "SubscribedDate",
                table: "SubscriptionsHistory");
        }
    }
}
