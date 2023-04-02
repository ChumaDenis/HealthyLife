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
                name: "Token",
                columns: table => new
                {
                    TokenID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 128, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", unicode: false, nullable: false),
                    UserId = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.TokenID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(64)", unicode: false, maxLength: 64, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(128)", unicode: false, maxLength: 128, nullable: false),
                    VerifacationToken = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", unicode: false, nullable: true),
                    PasswordRessetToken = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    PasswordRessetExpires = table.Column<DateTime>(type: "datetime2", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
