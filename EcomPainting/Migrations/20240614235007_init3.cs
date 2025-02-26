﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomPainting.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCertificated",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsCertificated",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
