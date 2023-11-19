using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactService.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Company", "Name", "Surname" },
                values: new object[] { new Guid("64734e2b-bc91-45dd-ab07-652e77c161e9"), "A Company", "John", "Black" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("64734e2b-bc91-45dd-ab07-652e77c161e9"));
        }
    }
}
