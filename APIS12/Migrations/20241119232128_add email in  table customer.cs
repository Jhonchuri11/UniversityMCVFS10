﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIS12.Migrations
{
    /// <inheritdoc />
    public partial class addemailintablecustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Customers");
        }
    }
}
