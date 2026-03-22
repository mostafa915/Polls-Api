using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.AppliacationsConfingrations.Migrations
{
    /// <inheritdoc />
    public partial class updateTypePermisson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENfJtgG20k8UhL6WCrL6EV2rEDuiIGWN1g/AqzzXBN6dzvesOawtms3r1PeuW3JPCw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKQVqLPVzqfcoSRg95LJfoT4zHqZWLhWMKklWYgHJdWutKMaxwAc/F8g0GyCc1qHVQ==");
        }
    }
}
