using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class RegisterDataAnnotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93614adb-a5ba-49fd-a516-ffe70925071f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e66e486-f60a-4186-ab02-d511e58f5993", "AQAAAAIAAYagAAAAEHd7OAqtnE06jG31UZdcQP6wXcQ7LLoSBVDz5uQrnI762MoRncRA7boS+Jr3RSvCvg==", "ce9d6d14-c541-4ce8-ba10-eff522f17f56" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93614adb-a5ba-49fd-a516-ffe70925071f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b78941ed-3047-4570-95bf-d35bc775032c", "AQAAAAIAAYagAAAAEPSsXIGXV8Ko3D6OAvgh355hHvyOjGrCVBzXPB7uiHcNGmjFWbWScMrXwazYZu1YcQ==", "9fbfbf2f-c46d-4e37-96fe-3377733800b6" });
        }
    }
}
