﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StmData",
                columns: table => new
                {
                    STM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    STM_HasSeeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StmData", x => x.STM_PK);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    USR_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    USR_ID = table.Column<int>(type: "int", nullable: false),
                    USR_Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USR_FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USR_LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USR_PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    USR_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USR_Address_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USR_Address_Postcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    USR_Address_Line1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USR_Address_Line2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.USR_PK);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    ADM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AMD_USR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ADM_USR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.ADM_PK);
                    table.ForeignKey(
                        name: "FK_Administrators_Users_AMD_USR_ID",
                        column: x => x.AMD_USR_ID,
                        principalTable: "Users",
                        principalColumn: "USR_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DCT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DCT_USR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DCT_PK);
                    table.ForeignKey(
                        name: "FK_Doctors_Users_DCT_USR_ID",
                        column: x => x.DCT_USR_ID,
                        principalTable: "Users",
                        principalColumn: "USR_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PAT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PAT_USR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DCT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PAT_PK);
                    table.ForeignKey(
                        name: "FK_Patients_Doctors_DCT_PK",
                        column: x => x.DCT_PK,
                        principalTable: "Doctors",
                        principalColumn: "DCT_PK");
                    table.ForeignKey(
                        name: "FK_Patients_Users_PAT_USR_ID",
                        column: x => x.PAT_USR_ID,
                        principalTable: "Users",
                        principalColumn: "USR_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentModels",
                columns: table => new
                {
                    APT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APT_AppointmentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APT_DCT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APT_PAT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APT_Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentModels", x => x.APT_PK);
                    table.ForeignKey(
                        name: "FK_AppointmentModels_Doctors_APT_DCT_ID",
                        column: x => x.APT_DCT_ID,
                        principalTable: "Doctors",
                        principalColumn: "DCT_PK");
                    table.ForeignKey(
                        name: "FK_AppointmentModels_Patients_APT_PAT_ID",
                        column: x => x.APT_PAT_ID,
                        principalTable: "Patients",
                        principalColumn: "PAT_PK");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_AMD_USR_ID",
                table: "Administrators",
                column: "AMD_USR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentModels_APT_DCT_ID",
                table: "AppointmentModels",
                column: "APT_DCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentModels_APT_PAT_ID",
                table: "AppointmentModels",
                column: "APT_PAT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DCT_USR_ID",
                table: "Doctors",
                column: "DCT_USR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DCT_PK",
                table: "Patients",
                column: "DCT_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PAT_USR_ID",
                table: "Patients",
                column: "PAT_USR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "AppointmentModels");

            migrationBuilder.DropTable(
                name: "StmData");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}