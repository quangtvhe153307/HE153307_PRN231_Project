using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class addmovieView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieView_MovieEpisodes_EpisodeId",
                table: "MovieView");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieView_Users_UserId",
                table: "MovieView");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieView",
                table: "MovieView");

            migrationBuilder.RenameTable(
                name: "MovieView",
                newName: "MovieViews");

            migrationBuilder.RenameIndex(
                name: "IX_MovieView_UserId",
                table: "MovieViews",
                newName: "IX_MovieViews_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieViews",
                table: "MovieViews",
                columns: new[] { "EpisodeId", "UserId", "ViewedDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieViews_MovieEpisodes_EpisodeId",
                table: "MovieViews",
                column: "EpisodeId",
                principalTable: "MovieEpisodes",
                principalColumn: "EpisodeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieViews_Users_UserId",
                table: "MovieViews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieViews_MovieEpisodes_EpisodeId",
                table: "MovieViews");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieViews_Users_UserId",
                table: "MovieViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieViews",
                table: "MovieViews");

            migrationBuilder.RenameTable(
                name: "MovieViews",
                newName: "MovieView");

            migrationBuilder.RenameIndex(
                name: "IX_MovieViews_UserId",
                table: "MovieView",
                newName: "IX_MovieView_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieView",
                table: "MovieView",
                columns: new[] { "EpisodeId", "UserId", "ViewedDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieView_MovieEpisodes_EpisodeId",
                table: "MovieView",
                column: "EpisodeId",
                principalTable: "MovieEpisodes",
                principalColumn: "EpisodeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieView_Users_UserId",
                table: "MovieView",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
