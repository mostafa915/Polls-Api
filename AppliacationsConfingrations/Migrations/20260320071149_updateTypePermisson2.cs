using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.AppliacationsConfingrations.Migrations
{
    /// <inheritdoc />
    public partial class updateTypePermisson2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ9klxHM2ne4fEvGZEoqv1r3YpNezdEVfApyWzRf5MOggMtIO57sde7VL9uY/8oiQw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENfJtgG20k8UhL6WCrL6EV2rEDuiIGWN1g/AqzzXBN6dzvesOawtms3r1PeuW3JPCw==");
        }
    }
}
