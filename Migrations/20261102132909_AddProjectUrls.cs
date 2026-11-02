using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubUrl",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Projects",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Projects");
        }
    }
}
