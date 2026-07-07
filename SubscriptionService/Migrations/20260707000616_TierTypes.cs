using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionService.Migrations
{
    /// <inheritdoc />
    public partial class TierTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Tier",
                table: "UserSubscriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Free");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tier",
                table: "UserSubscriptions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Free",
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
