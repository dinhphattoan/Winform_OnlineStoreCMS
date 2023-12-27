using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Models;

#nullable disable

namespace OnlineStore.DataAccess.Migrations
{
	public partial class init : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Desc = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
					Image = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					CivilianId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
					PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
					Role = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Orders",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ClerkID = table.Column<int>(type: "int", nullable: false),
					CustomerID = table.Column<int>(type: "int", nullable: false),
					CreateAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Orders", x => x.Id);
					table.ForeignKey(
						name: "FK_Orders_Users_ClerkID",
						column: x => x.ClerkID,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Orders_Users_CustomerID",
						column: x => x.CustomerID,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Thumbnail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					UnitPrice = table.Column<double>(type: "float", nullable: false),
					CreateByID = table.Column<int>(type: "int", nullable: false),
					CreatAt = table.Column<DateTime>(type: "datetime", nullable: false),
					CategoryID = table.Column<int>(type: "int", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
					table.ForeignKey(
						name: "FK_Products_Categories_CategoryID",
						column: x => x.CategoryID,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Products_Users_CreateByID",
						column: x => x.CreateByID,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "OrderDetails",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					OrderID = table.Column<int>(type: "int", nullable: false),
					ProductID = table.Column<int>(type: "int", nullable: false),
					Quantity = table.Column<int>(type: "int", nullable: false),
					UnitPrice = table.Column<double>(type: "float", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderDetails", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderDetails_Orders_OrderID",
						column: x => x.OrderID,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_OrderDetails_Products_ProductID",
						column: x => x.ProductID,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ProductImages",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Order = table.Column<int>(type: "int", nullable: false),
					Path = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					ProductID = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProductImages", x => x.Id);
					table.ForeignKey(
						name: "FK_ProductImages_Products_ProductID",
						column: x => x.ProductID,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Stocks",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Quantity = table.Column<int>(type: "int", nullable: false),
					ProductID = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Stocks", x => x.Id);
					table.ForeignKey(
						name: "FK_Stocks_Products_ProductID",
						column: x => x.ProductID,
						principalTable: "Products",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "StockEvents",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					StockID = table.Column<int>(type: "int", nullable: false),
					Type = table.Column<int>(type: "int", nullable: false),
					Reason = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
					Quantity = table.Column<int>(type: "int", nullable: false),
					CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_StockEvents", x => x.Id);
					table.ForeignKey(
						name: "FK_StockEvents_Stocks_StockID",
						column: x => x.StockID,
						principalTable: "Stocks",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_OrderDetails_OrderID",
				table: "OrderDetails",
				column: "OrderID");

			migrationBuilder.CreateIndex(
				name: "IX_OrderDetails_ProductID",
				table: "OrderDetails",
				column: "ProductID");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_ClerkID",
				table: "Orders",
				column: "ClerkID");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_CustomerID",
				table: "Orders",
				column: "CustomerID");

			migrationBuilder.CreateIndex(
				name: "IX_ProductImages_ProductID",
				table: "ProductImages",
				column: "ProductID");

			migrationBuilder.CreateIndex(
				name: "IX_Products_CategoryID",
				table: "Products",
				column: "CategoryID");

			migrationBuilder.CreateIndex(
				name: "IX_Products_CreateByID",
				table: "Products",
				column: "CreateByID");

			migrationBuilder.CreateIndex(
				name: "IX_StockEvents_StockID",
				table: "StockEvents",
				column: "StockID");

			migrationBuilder.CreateIndex(
				name: "IX_Stocks_ProductID",
				table: "Stocks",
				column: "ProductID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Users_Email",
				table: "Users",
				column: "Email",
				unique: true);
			Randomizer.Seed = new Random(8675309);

			var fakeCategories = new Faker<Category>()
				.RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
				.RuleFor(c => c.Desc, f => f.Lorem.Paragraph(1))
				.RuleFor(c => c.Image, f => f.Image.PicsumUrl())
				.RuleFor(c => c.IsDeleted, f => f.Random.Bool(0.2f));
			var fakeUsers = new Faker<User>()
				.RuleFor(u => u.FirstName, f => f.Name.FirstName())
				.RuleFor(u => u.LastName, f => f.Name.LastName())
				.RuleFor(u => u.CivilianId, f => f.Random.Replace("#########"))
				.RuleFor(u => u.Email, f => f.Internet.Email())
				.RuleFor(u => u.Password, f => f.Internet.Password())
				.RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("0#########"))
				.RuleFor(u => u.DateOfBirth, f => f.Date.Past(18))
				.RuleFor(u => u.Role, f => f.PickRandom<Role>())
				.RuleFor(u => u.IsDeleted, f => f.Random.Bool(0.2f));

			var fakeProducts = new Faker<Product>()
				.RuleFor(p => p.Name, f => f.Commerce.ProductName())
				.RuleFor(p => p.Description, f => f.Lorem.Paragraph(1))
				.RuleFor(p => p.Thumbnail, f => f.Image.PicsumUrl())
				.RuleFor(p => p.UnitPrice, f => f.Random.Float(1, 1000f))
				.RuleFor(p => p.CreateByID, f => f.Random.Int(1, 10))
				.RuleFor(p => p.CreatAt, f => f.Date.Past(1))
				.RuleFor(p => p.CategoryID, f => f.Random.Int(1, 10))
				.RuleFor(p => p.IsDeleted, f => f.Random.Bool(0.2f));
			var fakeProductImages = new Faker<ProductImage>()
				.RuleFor(pi => pi.Order, f => f.Random.Int(1, 10))
				.RuleFor(pi => pi.Path, f => f.Image.PicsumUrl())
				.RuleFor(pi => pi.ProductID, f => f.Random.Int(1, 50));
			for (int i = 0; i <= 10; i++)
			{
				Category category = fakeCategories.Generate();
				migrationBuilder.InsertData(
					table: "Categories",
					columns: new[] { "Name", "Desc", "Image", "IsDeleted" },
					values: new object[] { category.Name, category.Desc, category.Image, category.IsDeleted }
				);
			}
			for (int i = 0; i <= 10; i++)
			{
				User user = fakeUsers.Generate();
				migrationBuilder.InsertData(
					table: "Users",
					columns: new[] { "FirstName", "Lastname", "CivilianId", "Email", "Password", "PhoneNumber", "DateOfBirth", "Role", "IsDeleted" },
					values: new object[] { user.FirstName, user.LastName, user.CivilianId, user.Email, user.Password, user.PhoneNumber, user.DateOfBirth, (int)user.Role, user.IsDeleted }
				);
			}
			for (int i = 0; i <= 50; i++)
			{
				Product products = fakeProducts.Generate();
				migrationBuilder.InsertData(
					table: "Products",
					columns: new[] { "Name", "Description", "Thumbnail", "UnitPrice", "CreateByID", "CreatAt", "CategoryID", "IsDeleted" },
					values: new object[] { products.Name, products.Description, products.Thumbnail, products.UnitPrice, products.CreateByID, products.CreatAt, products.CategoryID, products.IsDeleted }
				);
			}
			for (int i = 0; i <= 50; i++)
			{
				ProductImage productImages = fakeProductImages.Generate();
				migrationBuilder.InsertData(
					table: "ProductImages",
					columns: new[] { "Order", "Path", "ProductID" },
					values: new object[] { productImages.Order, productImages.Path, productImages.ProductID }
				);
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "OrderDetails");

			migrationBuilder.DropTable(
				name: "ProductImages");

			migrationBuilder.DropTable(
				name: "StockEvents");

			migrationBuilder.DropTable(
				name: "Orders");

			migrationBuilder.DropTable(
				name: "Stocks");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
