using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aircraft_project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserImagesProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Users",
                newName: "UserImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserImages",
                table: "Users",
                newName: "ImagePath");
        }
    }
}
