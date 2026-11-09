using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GovSpend.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAgencyIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AgencyRecords_FederalAgencyId",
                table: "AgencyRecords",
                column: "FederalAgencyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AgencyRecords_FederalAgencyId",
                table: "AgencyRecords");
        }
    }
}
