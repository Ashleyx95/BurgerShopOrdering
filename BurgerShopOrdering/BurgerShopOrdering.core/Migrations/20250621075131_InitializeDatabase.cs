using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BurgerShopOrdering.core.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
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
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "money", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateOrdered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDelivered = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryProduct",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProduct", x => new { x.CategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000004", null, "Admin", "ADMIN" },
                    { "00000000-0000-0000-0000-000000000005", null, "Client", "CLIENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000001", 0, "4ab68b3d-1647-46ce-b662-2013ea820e65", "ashleysfritje@hotmail.com", true, "Ashleys", "Fritje", false, null, "ASHLEYSFRITJE@HOTMAIL.COM", "ASHLEYSFRITJE@HOTMAIL.COM", "AQAAAAIAAYagAAAAEAOACCkwZgHlk4hAwJTVShhms0I2f47uPlM3UtieIMbz5vJkvuZ57D6inPCdsga0bg==", null, false, "e3108fff-79c7-46e5-b3bb-f9a3bfc57c22", false, "ashleysfritje@hotmail.com" },
                    { "00000000-0000-0000-0000-000000000002", 0, "82d0b33d-b0e4-4caf-82af-2eae3b6a3361", "MarieFranck@hotmail.com", true, "Marie", "Franck", false, null, "MARIEFRANCK@HOTMAIL.COM", "MARIEFRANCK@HOTMAIL.COM", "AQAAAAIAAYagAAAAEN7JGArYpOjwGl0DHxw+y2Eqvwn4HtlXnFUfdw7DagJ9ki5dG7qJtNLVwXOrEk4k7w==", null, false, "ef6084d7-b1fb-4f1e-99ee-9966ac242892", false, "MarieFranck@hotmail.com" },
                    { "00000000-0000-0000-0000-000000000003", 0, "4c77dfb2-33e9-4caf-aeaf-efb2e7d0fce4", "lucy.mol@hotmail.com", true, "Lucy", "Mol", false, null, "LUCY.MOL@HOTMAIL.COM", "LUCY.MOL@HOTMAIL.COM", "AQAAAAIAAYagAAAAEMG9FOP4BUwzhylT8m+i1FOnaB1sqYOt/CeyavtVOujI5c4z49SGAJcPLPsag1XCsw==", null, false, "5aecf751-2160-47ce-b65c-cfb8fd327fd5", false, "lucy.mol@hotmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsVisible", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000010"), true, "Frieten" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), true, "Burgers" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), true, "Vlees" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), true, "Vis" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), true, "Kaas" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), true, "Kinderen" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), true, "Koude Sauzen" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), true, "Warme Sauzen" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), true, "Drinken" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), true, "Vegetarisch" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Image", "IsVisible", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000025"), "defaultproduct.jpg", true, "Mini friet", 2.40m },
                    { new Guid("00000000-0000-0000-0000-000000000026"), "defaultproduct.jpg", true, "Kleine friet", 2.80m },
                    { new Guid("00000000-0000-0000-0000-000000000027"), "defaultproduct.jpg", true, "Medium friet", 3.40m },
                    { new Guid("00000000-0000-0000-0000-000000000028"), "defaultproduct.jpg", true, "Grote friet", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000029"), "defaultproduct.jpg", true, "Familie pak", 8.70m },
                    { new Guid("00000000-0000-0000-0000-000000000030"), "defaultproduct.jpg", true, "Bicky Burger", 4.40m },
                    { new Guid("00000000-0000-0000-0000-000000000031"), "defaultproduct.jpg", true, "Bicky Cheese", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000032"), "defaultproduct.jpg", true, "Bicky Fish", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000033"), "defaultproduct.jpg", true, "Bicky Chicken", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000034"), "defaultproduct.jpg", true, "Bicky Vegi", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000035"), "defaultproduct.jpg", true, "Berenpoot", 3.70m },
                    { new Guid("00000000-0000-0000-0000-000000000036"), "defaultproduct.jpg", true, "Stoofvlees", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000037"), "defaultproduct.jpg", true, "Bitterballen", 2.70m },
                    { new Guid("00000000-0000-0000-0000-000000000038"), "defaultproduct.jpg", true, "Twijfelaar", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000039"), "defaultproduct.jpg", true, "Zigeunerstick", 3.20m },
                    { new Guid("00000000-0000-0000-0000-000000000040"), "defaultproduct.jpg", true, "Boulet", 3.00m },
                    { new Guid("00000000-0000-0000-0000-000000000041"), "defaultproduct.jpg", true, "Cervela", 3.20m },
                    { new Guid("00000000-0000-0000-0000-000000000042"), "defaultproduct.jpg", true, "Curryworst", 2.40m },
                    { new Guid("00000000-0000-0000-0000-000000000043"), "defaultproduct.jpg", true, "Curryworst Speciaal", 3.40m },
                    { new Guid("00000000-0000-0000-0000-000000000044"), "defaultproduct.jpg", true, "Goulash kroket", 3.00m },
                    { new Guid("00000000-0000-0000-0000-000000000045"), "defaultproduct.jpg", true, "Kipcorn", 3.20m },
                    { new Guid("00000000-0000-0000-0000-000000000046"), "defaultproduct.jpg", true, "Kippen nuggets", 4.00m },
                    { new Guid("00000000-0000-0000-0000-000000000047"), "defaultproduct.jpg", true, "Kipfingers", 4.00m },
                    { new Guid("00000000-0000-0000-0000-000000000048"), "defaultproduct.jpg", true, "Loempia", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000049"), "defaultproduct.jpg", true, "Viandel", 3.00m },
                    { new Guid("00000000-0000-0000-0000-000000000050"), "defaultproduct.jpg", true, "Sate", 4.40m },
                    { new Guid("00000000-0000-0000-0000-000000000051"), "defaultproduct.jpg", true, "Visstick", 4.70m },
                    { new Guid("00000000-0000-0000-0000-000000000052"), "defaultproduct.jpg", true, "Kaaskroket", 2.20m },
                    { new Guid("00000000-0000-0000-0000-000000000053"), "defaultproduct.jpg", true, "Kaassouffle", 2.80m },
                    { new Guid("00000000-0000-0000-0000-000000000054"), "defaultproduct.jpg", true, "Mozarella vingers", 4.20m },
                    { new Guid("00000000-0000-0000-0000-000000000055"), "defaultproduct.jpg", true, "Boy box", 6.50m },
                    { new Guid("00000000-0000-0000-0000-000000000056"), "defaultproduct.jpg", true, "Girl box", 6.50m },
                    { new Guid("00000000-0000-0000-0000-000000000057"), "defaultproduct.jpg", true, "Mayonaise", 1.20m },
                    { new Guid("00000000-0000-0000-0000-000000000058"), "defaultproduct.jpg", true, "Tomaten ketchup", 1.20m },
                    { new Guid("00000000-0000-0000-0000-000000000059"), "defaultproduct.jpg", true, "Curry ketchup", 1.20m },
                    { new Guid("00000000-0000-0000-0000-000000000060"), "defaultproduct.jpg", true, "Andalouse", 1.40m },
                    { new Guid("00000000-0000-0000-0000-000000000061"), "defaultproduct.jpg", true, "Americain", 1.40m },
                    { new Guid("00000000-0000-0000-0000-000000000062"), "defaultproduct.jpg", true, "Tartaar", 1.40m },
                    { new Guid("00000000-0000-0000-0000-000000000063"), "defaultproduct.jpg", true, "Looksaus", 1.40m },
                    { new Guid("00000000-0000-0000-0000-000000000064"), "defaultproduct.jpg", true, "Pindasaus", 2.20m },
                    { new Guid("00000000-0000-0000-0000-000000000065"), "defaultproduct.jpg", true, "Stoofvleessaus", 2.20m },
                    { new Guid("00000000-0000-0000-0000-000000000066"), "defaultproduct.jpg", true, "Currysaus", 2.20m },
                    { new Guid("00000000-0000-0000-0000-000000000067"), "defaultproduct.jpg", true, "Coca cola", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000068"), "defaultproduct.jpg", true, "Coca cola zero", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000069"), "defaultproduct.jpg", true, "Sprite", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000070"), "defaultproduct.jpg", true, "Fanta", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000071"), "defaultproduct.jpg", true, "Ice tea", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000072"), "defaultproduct.jpg", true, "Jupiler", 2.20m },
                    { new Guid("00000000-0000-0000-0000-000000000073"), "defaultproduct.jpg", true, "Water plat", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000074"), "defaultproduct.jpg", true, "Water bruis", 2.00m },
                    { new Guid("00000000-0000-0000-0000-000000000075"), "defaultproduct.jpg", true, "Vegi bitterballen", 3.00m },
                    { new Guid("00000000-0000-0000-0000-000000000076"), "defaultproduct.jpg", true, "Nasi schijf", 3.20m },
                    { new Guid("00000000-0000-0000-0000-000000000077"), "defaultproduct.jpg", true, "Mini loempia", 4.20m },
                    { new Guid("00000000-0000-0000-0000-000000000078"), "defaultproduct.jpg", true, "Vegi curryworst", 3.70m }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000001" },
                    { "00000000-0000-0000-0000-000000000005", "00000000-0000-0000-0000-000000000002" },
                    { "00000000-0000-0000-0000-000000000005", "00000000-0000-0000-0000-000000000003" }
                });

            migrationBuilder.InsertData(
                table: "CategoryProduct",
                columns: new[] { "CategoriesId", "ProductsId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000025") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000026") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000027") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000028") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000029") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000030") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000031") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000032") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000033") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000034") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000030") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000031") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000033") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000035") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000036") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000037") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000038") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000039") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000040") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000041") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000042") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000043") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000044") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000045") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000046") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000047") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000048") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000049") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000050") },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000032") },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000051") },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000052") },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000053") },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000054") },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new Guid("00000000-0000-0000-0000-000000000055") },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new Guid("00000000-0000-0000-0000-000000000056") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000057") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000058") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000059") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000060") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000061") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000062") },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new Guid("00000000-0000-0000-0000-000000000063") },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new Guid("00000000-0000-0000-0000-000000000064") },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new Guid("00000000-0000-0000-0000-000000000065") },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new Guid("00000000-0000-0000-0000-000000000066") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000067") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000068") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000069") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000070") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000071") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000072") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000073") },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new Guid("00000000-0000-0000-0000-000000000074") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000052") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000053") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000054") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000075") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000076") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000077") },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new Guid("00000000-0000-0000-0000-000000000078") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

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
                name: "IX_CategoryProduct_ProductsId",
                table: "CategoryProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CategoryProduct");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
