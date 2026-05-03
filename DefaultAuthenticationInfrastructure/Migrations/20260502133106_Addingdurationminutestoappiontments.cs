using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicProjectInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addingdurationminutestoappiontments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Appointments");
        }
    }
}
