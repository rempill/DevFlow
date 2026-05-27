using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DevFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSnapshotAfterRemovingUmlAndCommits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Commits_CommitSha",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Commits");

            migrationBuilder.DropTable(
                name: "UMLDiagrams");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CommitSha",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commits",
                columns: table => new
                {
                    Sha = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    VersionMajor = table.Column<int>(type: "integer", nullable: false),
                    VersionMinor = table.Column<int>(type: "integer", nullable: false),
                    VersionPatch = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.Sha);
                    table.ForeignKey(
                        name: "FK_Commits_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UMLDiagrams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LastSha = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UMLDiagrams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UMLDiagrams_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CommitSha",
                table: "Reviews",
                column: "CommitSha");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_TaskId",
                table: "Commits",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UMLDiagrams_ProjectId",
                table: "UMLDiagrams",
                column: "ProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Commits_CommitSha",
                table: "Reviews",
                column: "CommitSha",
                principalTable: "Commits",
                principalColumn: "Sha",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
