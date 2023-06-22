using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WweebbAapppp.Migrations
{
    public partial class AddIdentityTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BodyTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Makes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "QuoteRules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FeatureValue = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PriceModifier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteRules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MakeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Models_Makes_MakeID",
                        column: x => x.MakeID,
                        principalTable: "Makes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelStyles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelID = table.Column<int>(type: "int", nullable: false),
                    BodyTypeID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelStyles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelStyles_BodyTypes_BodyTypeID",
                        column: x => x.BodyTypeID,
                        principalTable: "BodyTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelStyles_Models_ModelID",
                        column: x => x.ModelID,
                        principalTable: "Models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelStyles_Sizes_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Sizes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelStyleYears",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModelStyleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelStyleYears", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelStyleYears_ModelStyles_ModelStyleID",
                        column: x => x.ModelStyleID,
                        principalTable: "ModelStyles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuoteOverides",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelStyleYearID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteOverides", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuoteOverides_ModelStyleYears_ModelStyleYearID",
                        column: x => x.ModelStyleYearID,
                        principalTable: "ModelStyleYears",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelStyleYearID = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyTypeID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false),
                    ItMoves = table.Column<bool>(type: "bit", nullable: false),
                    HasAllWheels = table.Column<bool>(type: "bit", nullable: false),
                    HasAlloyWheels = table.Column<bool>(type: "bit", nullable: false),
                    HasAllTires = table.Column<bool>(type: "bit", nullable: false),
                    HasKey = table.Column<bool>(type: "bit", nullable: false),
                    HasTitle = table.Column<bool>(type: "bit", nullable: false),
                    RequiresPickup = table.Column<bool>(type: "bit", nullable: false),
                    HasEngine = table.Column<bool>(type: "bit", nullable: false),
                    HasTransmission = table.Column<bool>(type: "bit", nullable: false),
                    HasCompleteInterior = table.Column<bool>(type: "bit", nullable: false),
                    OfferedQuote = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Quotes_BodyTypes_BodyTypeID",
                        column: x => x.BodyTypeID,
                        principalTable: "BodyTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotes_ModelStyleYears_ModelStyleYearID",
                        column: x => x.ModelStyleYearID,
                        principalTable: "ModelStyleYears",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Quotes_Sizes_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Sizes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BodyTypes",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Coupe" },
                    { 2, "Sedan" },
                    { 3, "Hatchback" },
                    { 4, "Wagon" },
                    { 5, "Convertible" },
                    { 6, "SUV" },
                    { 7, "Truck" },
                    { 8, "Mini Van" },
                    { 9, "Roadster" }
                });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Subcompact" },
                    { 2, "Compact" },
                    { 3, "Mid Size" },
                    { 5, "Full Size" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTypes_Name",
                table: "BodyTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Makes_Name",
                table: "Makes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeID",
                table: "Models",
                column: "MakeID");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name_MakeID",
                table: "Models",
                columns: new[] { "Name", "MakeID" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStyles_BodyTypeID",
                table: "ModelStyles",
                column: "BodyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStyles_ModelID_BodyTypeID_SizeID",
                table: "ModelStyles",
                columns: new[] { "ModelID", "BodyTypeID", "SizeID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelStyles_SizeID",
                table: "ModelStyles",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStyleYears_ModelStyleID",
                table: "ModelStyleYears",
                column: "ModelStyleID");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStyleYears_Year_ModelStyleID",
                table: "ModelStyleYears",
                columns: new[] { "Year", "ModelStyleID" },
                unique: true,
                filter: "[Year] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteOverides_ModelStyleYearID",
                table: "QuoteOverides",
                column: "ModelStyleYearID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuoteRules_FeatureType_FeatureValue",
                table: "QuoteRules",
                columns: new[] { "FeatureType", "FeatureValue" },
                unique: true,
                filter: "[FeatureType] IS NOT NULL AND [FeatureValue] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_BodyTypeID",
                table: "Quotes",
                column: "BodyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_ModelStyleYearID",
                table: "Quotes",
                column: "ModelStyleYearID");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_SizeID",
                table: "Quotes",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Name",
                table: "Sizes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "QuoteOverides");

            migrationBuilder.DropTable(
                name: "QuoteRules");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ModelStyleYears");

            migrationBuilder.DropTable(
                name: "ModelStyles");

            migrationBuilder.DropTable(
                name: "BodyTypes");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "Makes");
        }
    }
}
