using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.AppliacationsConfingrations.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultValuesInTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2D856FD7-D6F2-420D-AC2D-359A1B2DDBFC", "F56E4618-7689-4269-805B-E713849403F9", true, false, "Member", "MEMBER" },
                    { "3887F997-BD0A-4077-AC23-7DBC0634325F", "2DA26A92-26E4-4B8B-97DE-3898FAC9453E", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6B7421AC-AD74-40C2-AB7F-37913D34A431", 0, "087E7182-EE73-46FD-AB2D-4E49537DED0C", "Admin@SurveyBasket.com", true, "Survey", "Amdin", false, null, "ADMIN@SURVEYBASKET.COM", "SURVEYAMDIN", "AQAAAAIAAYagAAAAEKQVqLPVzqfcoSRg95LJfoT4zHqZWLhWMKklWYgHJdWutKMaxwAc/F8g0GyCc1qHVQ==", null, false, "5ED6E9D981494584BEBFD0117B98CCDE", false, "SurveyAmdin" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "polls:read", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 2, "Permission", "polls:Add", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 3, "Permission", "polls:update", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 4, "Permission", "polls:delete", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 5, "Permission", "questions:read", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 6, "Permission", "questions:Add", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 7, "Permission", "questions:update", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 8, "Permission", "users:read", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 9, "Permission", "users:Add", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 10, "Permission", "users:update", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 11, "Permission", "roles:read", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 12, "Permission", "roles:Add", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 13, "Permission", "roles:update", "3887F997-BD0A-4077-AC23-7DBC0634325F" },
                    { 14, "Permission", "results.read", "3887F997-BD0A-4077-AC23-7DBC0634325F" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3887F997-BD0A-4077-AC23-7DBC0634325F", "6B7421AC-AD74-40C2-AB7F-37913D34A431" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2D856FD7-D6F2-420D-AC2D-359A1B2DDBFC");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3887F997-BD0A-4077-AC23-7DBC0634325F", "6B7421AC-AD74-40C2-AB7F-37913D34A431" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3887F997-BD0A-4077-AC23-7DBC0634325F");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6B7421AC-AD74-40C2-AB7F-37913D34A431");
        }
    }
}
