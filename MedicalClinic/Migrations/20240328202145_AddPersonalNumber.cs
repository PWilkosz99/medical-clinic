using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalClinic.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalNumber",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalNumber",
                table: "Patients");
        }
    }
}
