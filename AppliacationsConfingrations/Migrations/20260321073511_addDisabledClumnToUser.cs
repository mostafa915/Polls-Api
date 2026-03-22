using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.AppliacationsConfingrations.Migrations
{
    /// <inheritdoc />
    public partial class addDisabledClumnToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDiabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431",
                columns: new[] { "IsDiabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEJOu5e1C95d9r3/ydfoeTA88M1uQ66yjKjnAgMPS1Doy325mVn7/R3B1iNHCiTRGgA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJNryNd40FCiPr62M7l09mHC4P0hEwN92EtqFWPidU7bFX2P7Tro2pItF6lHPcIvpw==");
        }
    }
}
