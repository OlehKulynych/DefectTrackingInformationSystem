using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefectTrackingInformationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFieldDefectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageString",
                table: "Defectes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageString",
                table: "Defectes");
        }
    }
}
