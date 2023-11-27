using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "Provider",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Provider_PaymentTypeId",
                table: "Provider",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Provider_PaymentType_PaymentTypeId",
                table: "Provider",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provider_PaymentType_PaymentTypeId",
                table: "Provider");

            migrationBuilder.DropIndex(
                name: "IX_Provider_PaymentTypeId",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "Provider");
        }
    }
}
