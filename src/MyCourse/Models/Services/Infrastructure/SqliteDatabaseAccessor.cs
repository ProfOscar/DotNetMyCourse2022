using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.ValueObjects;

namespace MyCourse.Models.Services.Infrastructure
{
    public class SqliteDatabaseAccessor : IDatabaseAccessor
    {
        private readonly IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions;
        private readonly ILogger<SqliteDatabaseAccessor> logger;

        public SqliteDatabaseAccessor(ILogger<SqliteDatabaseAccessor> logger, IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions)
        {
            this.logger = logger;
            this.connectionStringOptions = connectionStringOptions;
        }

        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            logger.LogInformation(formattableQuery.Format, formattableQuery.GetArguments());

            // Creiamo i SqliteParameter a partire dalla FormattableString per evitare eventuali problemi di SQL injection
            var queryArguments = formattableQuery.GetArguments();
            var sqliteParameters = new List<SqliteParameter>();
            for (var i = 0; i < queryArguments.Length; i++)
            {
                if (queryArguments[i] is Sql)
                {
                    continue;
                }
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]);
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@" + i;
            }
            string query = formattableQuery.ToString();

            string connectionString = connectionStringOptions.CurrentValue.Default;
            using (var conn = new SqliteConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddRange(sqliteParameters);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var dataSet = new DataSet();

                        // workaround per issue di Microsoft.Data.Sqlite 
                        dataSet.EnforceConstraints = false;

                        do
                        {
                            var dataTable = new DataTable();
                            dataSet.Tables.Add(dataTable);
                            dataTable.Load(reader);
                        } while (!reader.IsClosed);

                        return dataSet;
                    }
                }
            }
        }
    }
}