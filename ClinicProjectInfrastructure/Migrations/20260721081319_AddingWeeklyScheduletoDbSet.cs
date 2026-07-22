using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicProjectInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingWeeklyScheduletoDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedule_Doctors_DoctorId",
                table: "WeeklySchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeeklySchedule",
                table: "WeeklySchedule");

            migrationBuilder.RenameTable(
                name: "WeeklySchedule",
                newName: "WeeklySchedules");

            migrationBuilder.RenameIndex(
                name: "IX_WeeklySchedule_DoctorId",
                table: "WeeklySchedules",
                newName: "IX_WeeklySchedules_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeeklySchedules",
                table: "WeeklySchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Doctors_DoctorId",
                table: "WeeklySchedules",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Doctors_DoctorId",
                table: "WeeklySchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeeklySchedules",
                table: "WeeklySchedules");

            migrationBuilder.RenameTable(
                name: "WeeklySchedules",
                newName: "WeeklySchedule");

            migrationBuilder.RenameIndex(
                name: "IX_WeeklySchedules_DoctorId",
                table: "WeeklySchedule",
                newName: "IX_WeeklySchedule_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeeklySchedule",
                table: "WeeklySchedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedule_Doctors_DoctorId",
                table: "WeeklySchedule",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
