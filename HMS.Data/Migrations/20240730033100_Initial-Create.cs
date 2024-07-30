using System;
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
                    STM_PK = table.Column<Guid>(type: "TEXT", nullable: false),
                    STM_HasSeeded = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StmData", x => x.STM_PK);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    USR_PK = table.Column<Guid>(type: "TEXT", nullable: false),
                    USR_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    USR_Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    USR_FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    USR_LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    USR_PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    USR_Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    USR_Address_State = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    USR_Address_Postcode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    USR_Address_Line1 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    USR_Address_Line2 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.USR_PK);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    ADM_PK = table.Column<Guid>(type: "TEXT", nullable: false),
                    ADM_USR_ID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.ADM_PK);
                    table.ForeignKey(
                        name: "FK_Administrators_Users_ADM_USR_ID",
                        column: x => x.ADM_USR_ID,
                        principalTable: "Users",
                        principalColumn: "USR_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DCT_PK = table.Column<Guid>(type: "TEXT", nullable: false),
                    DCT_USR_ID = table.Column<Guid>(type: "TEXT", nullable: false)
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
                    PAT_PK = table.Column<Guid>(type: "TEXT", nullable: false),
                    PAT_USR_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PAT_DCT_ID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PAT_PK);
                    table.ForeignKey(
                        name: "FK_Patients_Doctors_PAT_DCT_ID",
                        column: x => x.PAT_DCT_ID,
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
                    APT_PK = table.Column<Guid>(type: "TEXT", nullable: false),
                    APT_AppointmentTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    APT_DCT_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    APT_PAT_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    APT_Description = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false)
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
                name: "IX_Administrators_ADM_USR_ID",
                table: "Administrators",
                column: "ADM_USR_ID");

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
                name: "IX_Patients_PAT_DCT_ID",
                table: "Patients",
                column: "PAT_DCT_ID");

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
