﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIS12.Migrations
{
    /// <inheritdoc />
    public partial class V2semana13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "Customers",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Customers",
                newName: "email");
        }
    }
}
