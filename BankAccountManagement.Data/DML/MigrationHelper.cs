using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Data.DML
{
    internal static class MigrationHelper
    {
        public static string GetSQLFileText(string resourceSubPath, string fileName)
        {
            var scripts = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceSubPath), "*.sql");
            var file = scripts.FirstOrDefault(d => d.Contains(fileName));
            return file != null ? File.ReadAllText(file) : string.Empty;
        }

        public static string[] GetSQLFiles(string resourceSubPath) =>
            Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceSubPath), "*.sql");
    }
}
