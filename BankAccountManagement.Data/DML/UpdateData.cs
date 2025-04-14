using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Data.DML
{
    [DbContext(typeof(BankAccountContext))]
    [Migration("20250415235511_UpdateData")]
    internal class UpdateData: Migration
    {
        private const string DMLFolderPath = "DML";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dmlFiles = MigrationHelper.GetSQLFiles(DMLFolderPath);

            foreach (var dmlFile in dmlFiles)
            {
                var text = MigrationHelper.GetSQLFileText(DMLFolderPath, dmlFile);
                if (!string.IsNullOrEmpty(text))
                    migrationBuilder.Sql(text);
            }
        }
    }
}
