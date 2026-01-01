using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModeifyPhotoColInMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5ExGEHlPHckD3YbxH6e4kr25Ho2X4NifiQA&s",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5ExGEHlPHckD3YbxH6e4kr25Ho2X4NifiQA&s");
        }
    }
}
