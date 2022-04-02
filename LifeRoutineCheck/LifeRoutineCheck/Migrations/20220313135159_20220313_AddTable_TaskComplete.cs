using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LifeRoutineCheck.Migrations
{
    public partial class _20220313_AddTable_TaskComplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskCompletes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Task_Id = table.Column<int>(type: "int", nullable: false),
                    TaskComplete_Flg = table.Column<bool>(type: "bit", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCompletes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskCompletes");
        }
    }
}
