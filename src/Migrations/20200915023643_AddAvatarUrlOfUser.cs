using Microsoft.EntityFrameworkCore.Migrations;

namespace CommentsApi.Migrations
{
    public partial class AddAvatarUrlOfUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GravatarUrl",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GravatarUrl",
                table: "Users");
        }
    }
}
