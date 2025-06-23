using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSim.Migrations
{
    /// <inheritdoc />
    public partial class iliskilikayit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "MultipleChoiceQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "ClassicQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestions_QuestionId",
                table: "MultipleChoiceQuestions",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassicQuestions_QuestionId",
                table: "ClassicQuestions",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassicQuestions_Questions_QuestionId",
                table: "ClassicQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceQuestions_Questions_QuestionId",
                table: "MultipleChoiceQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassicQuestions_Questions_QuestionId",
                table: "ClassicQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceQuestions_Questions_QuestionId",
                table: "MultipleChoiceQuestions");

            migrationBuilder.DropIndex(
                name: "IX_MultipleChoiceQuestions_QuestionId",
                table: "MultipleChoiceQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ClassicQuestions_QuestionId",
                table: "ClassicQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "MultipleChoiceQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "ClassicQuestions");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
