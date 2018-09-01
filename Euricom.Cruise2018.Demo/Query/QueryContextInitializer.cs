using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Euricom.Cruise2018.Demo.Query
{
    public class QueryContextInitializer : DropCreateDatabaseIfModelChanges<QueryContext>
    {
        protected override void Seed(QueryContext context)
        {
            base.Seed(context);

            var demoPath = AppDomain.CurrentDomain.BaseDirectory.Split(new[] { "Euricom.Cruise2018.Demo" }, StringSplitOptions.None)[0];

            var scriptPath = string.Concat(demoPath, @"Euricom.Cruise2018.Demo\Database");

            var scriptFiles = Directory.GetFiles(scriptPath, "*.sql", SearchOption.AllDirectories).OrderBy(x => x);

            var schemaCreationScript = scriptFiles.FirstOrDefault(x => x.Contains("EventSchema"));
            var tableCreationScript = scriptFiles.FirstOrDefault(x => x.Contains("EventTables"));

            if (schemaCreationScript != null && tableCreationScript != null)
            {
                var schemaSql = File.ReadAllText(schemaCreationScript);
                var schemaSqlCommands = schemaSql.Split(new[] { "GO" }, StringSplitOptions.None);

                ExecuteSqlCommands(schemaSqlCommands, context);

                var tablesSql = File.ReadAllText(tableCreationScript);
                var tablesSqlCommands = tablesSql.Split(new[] { "GO" }, StringSplitOptions.None);

                ExecuteSqlCommands(tablesSqlCommands, context);
            }
        }

        private void ExecuteSqlCommands(string[] sqlCommands, DbContext context)
        {
            foreach (var cmd in sqlCommands)
            {
                if (string.IsNullOrEmpty(cmd))
                    continue;

                var cleanedCmd = cmd.Replace(Environment.NewLine, string.Empty);

                context.Database.ExecuteSqlCommand(cleanedCmd);
            }
        }
    }
}
