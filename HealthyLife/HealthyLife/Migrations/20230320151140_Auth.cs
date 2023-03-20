using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyLife.Migrations
{
    /// <inheritdoc />
    public partial class Auth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", unicode: false, nullable: false),
                    UserId = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Mail = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(50)", unicode: false, maxLength: 50, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
