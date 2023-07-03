using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class adddbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMovie_Category_CategoriesCategoryId",
                table: "CategoryMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMovie_Movie_MoviesMovieId",
                table: "CategoryMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Movie_MovieId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieEpisode_MovieSeason_MovieSeasonId",
                table: "MovieEpisode");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePurchased_Movie_MovieId",
                table: "MoviePurchased");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePurchased_Users_UserId",
                table: "MoviePurchased");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRated_Movie_MovieId",
                table: "MovieRated");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRated_Users_UserId",
                table: "MovieRated");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieSeason_Movie_MovieId",
                table: "MovieSeason");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSeason",
                table: "MovieSeason");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieRated",
                table: "MovieRated");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviePurchased",
                table: "MoviePurchased");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieEpisode",
                table: "MovieEpisode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie",
                table: "Movie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "MovieSeason",
                newName: "MovieSeasons");

            migrationBuilder.RenameTable(
                name: "MovieRated",
                newName: "MovieRateds");

            migrationBuilder.RenameTable(
                name: "MoviePurchased",
                newName: "MoviePurchaseds");

            migrationBuilder.RenameTable(
                name: "MovieEpisode",
                newName: "MovieEpisodes");

            migrationBuilder.RenameTable(
                name: "Movie",
                newName: "Movies");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_MovieSeason_MovieId",
                table: "MovieSeasons",
                newName: "IX_MovieSeasons_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieRated_UserId",
                table: "MovieRateds",
                newName: "IX_MovieRateds_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviePurchased_UserId",
                table: "MoviePurchaseds",
                newName: "IX_MoviePurchaseds_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieEpisode_MovieSeasonId",
                table: "MovieEpisodes",
                newName: "IX_MovieEpisodes_MovieSeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSeasons",
                table: "MovieSeasons",
                column: "MovieSeasonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieRateds",
                table: "MovieRateds",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviePurchaseds",
                table: "MoviePurchaseds",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieEpisodes",
                table: "MovieEpisodes",
                column: "EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                columns: new[] { "MovieId", "UserId", "CommentedDate" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMovie_Categories_CategoriesCategoryId",
                table: "CategoryMovie",
                column: "CategoriesCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMovie_Movies_MoviesMovieId",
                table: "CategoryMovie",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Movies_MovieId",
                table: "Comments",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieEpisodes_MovieSeasons_MovieSeasonId",
                table: "MovieEpisodes",
                column: "MovieSeasonId",
                principalTable: "MovieSeasons",
                principalColumn: "MovieSeasonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePurchaseds_Movies_MovieId",
                table: "MoviePurchaseds",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePurchaseds_Users_UserId",
                table: "MoviePurchaseds",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRateds_Movies_MovieId",
                table: "MovieRateds",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRateds_Users_UserId",
                table: "MovieRateds",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSeasons_Movies_MovieId",
                table: "MovieSeasons",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMovie_Categories_CategoriesCategoryId",
                table: "CategoryMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMovie_Movies_MoviesMovieId",
                table: "CategoryMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Movies_MovieId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieEpisodes_MovieSeasons_MovieSeasonId",
                table: "MovieEpisodes");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePurchaseds_Movies_MovieId",
                table: "MoviePurchaseds");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePurchaseds_Users_UserId",
                table: "MoviePurchaseds");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRateds_Movies_MovieId",
                table: "MovieRateds");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRateds_Users_UserId",
                table: "MovieRateds");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieSeasons_Movies_MovieId",
                table: "MovieSeasons");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSeasons",
                table: "MovieSeasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieRateds",
                table: "MovieRateds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviePurchaseds",
                table: "MoviePurchaseds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieEpisodes",
                table: "MovieEpisodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "MovieSeasons",
                newName: "MovieSeason");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "Movie");

            migrationBuilder.RenameTable(
                name: "MovieRateds",
                newName: "MovieRated");

            migrationBuilder.RenameTable(
                name: "MoviePurchaseds",
                newName: "MoviePurchased");

            migrationBuilder.RenameTable(
                name: "MovieEpisodes",
                newName: "MovieEpisode");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_MovieSeasons_MovieId",
                table: "MovieSeason",
                newName: "IX_MovieSeason_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieRateds_UserId",
                table: "MovieRated",
                newName: "IX_MovieRated_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviePurchaseds_UserId",
                table: "MoviePurchased",
                newName: "IX_MoviePurchased_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieEpisodes_MovieSeasonId",
                table: "MovieEpisode",
                newName: "IX_MovieEpisode_MovieSeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSeason",
                table: "MovieSeason",
                column: "MovieSeasonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie",
                table: "Movie",
                column: "MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieRated",
                table: "MovieRated",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviePurchased",
                table: "MoviePurchased",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieEpisode",
                table: "MovieEpisode",
                column: "EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                columns: new[] { "MovieId", "UserId", "CommentedDate" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMovie_Category_CategoriesCategoryId",
                table: "CategoryMovie",
                column: "CategoriesCategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMovie_Movie_MoviesMovieId",
                table: "CategoryMovie",
                column: "MoviesMovieId",
                principalTable: "Movie",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Movie_MovieId",
                table: "Comment",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieEpisode_MovieSeason_MovieSeasonId",
                table: "MovieEpisode",
                column: "MovieSeasonId",
                principalTable: "MovieSeason",
                principalColumn: "MovieSeasonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePurchased_Movie_MovieId",
                table: "MoviePurchased",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePurchased_Users_UserId",
                table: "MoviePurchased",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRated_Movie_MovieId",
                table: "MovieRated",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRated_Users_UserId",
                table: "MovieRated",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSeason_Movie_MovieId",
                table: "MovieSeason",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
