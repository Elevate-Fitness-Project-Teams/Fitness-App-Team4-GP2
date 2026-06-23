using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCEService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatedMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Bmr = table.Column<double>(type: "float", nullable: false),
                    Tdee = table.Column<double>(type: "float", nullable: false),
                    CalorieTarget = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FitnessPlanConfigs",
                columns: table => new
                {
                    PlanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MinCalorie = table.Column<double>(type: "float", nullable: false),
                    MaxCalorie = table.Column<double>(type: "float", nullable: false),
                    EstimatedDuration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkoutsPerWeek = table.Column<int>(type: "int", nullable: false),
                    ProgramType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessPlanConfigs", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "UserFitnessStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActivityLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFitnessStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPlanHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReasonForChange = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAssignedPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignedPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssignedPlans_FitnessPlanConfigs_PlanId",
                        column: x => x.PlanId,
                        principalTable: "FitnessPlanConfigs",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedMetrics_UserId",
                table: "CalculatedMetrics",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignedPlans_PlanId",
                table: "UserAssignedPlans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignedPlans_UserId",
                table: "UserAssignedPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFitnessStats_UserId",
                table: "UserFitnessStats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanHistories_UserId",
                table: "UserPlanHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatedMetrics");

            migrationBuilder.DropTable(
                name: "UserAssignedPlans");

            migrationBuilder.DropTable(
                name: "UserFitnessStats");

            migrationBuilder.DropTable(
                name: "UserPlanHistories");

            migrationBuilder.DropTable(
                name: "FitnessPlanConfigs");
        }
    }
}
