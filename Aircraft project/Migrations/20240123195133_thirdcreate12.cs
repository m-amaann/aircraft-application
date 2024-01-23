using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aircraft_project.Migrations
{
    /// <inheritdoc />
    public partial class thirdcreate12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingStatus",
                table: "Orders");
        }
    }
}
