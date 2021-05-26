using Microsoft.Extensions.Configuration;
using Migration;
using Npgsql;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace IntegrationTests
{
    class DatabaseCreator
    {
        private readonly string connectionString;
        private readonly bool shouldDropAndCreate;
        private readonly MigratorUtility migrator;

        public DatabaseCreator(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString(Setup.ConnectionStringName);
            shouldDropAndCreate = configuration.GetValue<bool>("DropAndCreateDatabase");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string cannot be null.");
            migrator = new MigratorUtility(connectionString);
        }

        public async Task DropAndCreateDb()
        {
            if (!shouldDropAndCreate)
                return;

            var connectionInfo = ConnectionInfo.Parse(connectionString);
            var databaseName = connectionInfo.DatabaseName;

            using (var conn = new NpgsqlConnection(connectionInfo.NewDatabaseName("postgres").ToString()))
            {
                conn.Open();

                var existsDatabase = await PostgreSQLCommand.ExistsDatabase(conn, databaseName);

                if (existsDatabase)
                    await PostgreSQLCommand.DropDatabase(conn, databaseName);

                await PostgreSQLCommand.CreateDatabase(conn, databaseName);

                conn.Close();
            }

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                conn.Close();
            }
        }

        public void Migrate() => migrator.Up();

        private class ConnectionInfo
        {
            public string Server { get; set; } = "";
            public string DatabaseName { get; set; } = "";
            public string Login { get; set; } = "";
            public string Password { get; set; } = "";
            public bool IntegratedSecurity { get; set; }

            public override string ToString() =>
                IntegratedSecurity
                    ? $"Server={Server};Port=5432;Database={DatabaseName};Trusted_Connection=True;"
                    : $"Server={Server};Port=5432;Database={DatabaseName};Username={Login};Password={Password};";

            public ConnectionInfo NewDatabaseName(string databaseName)
            {
                var copy = (ConnectionInfo)MemberwiseClone();
                copy.DatabaseName = databaseName;
                return copy;
            }

            public static ConnectionInfo Parse(string connectionString)
            {
                var connectionInfo = new ConnectionInfo();

                var builder = new NpgsqlConnectionStringBuilder(connectionString);
                connectionInfo.Server = builder.Host ?? string.Empty;
                connectionInfo.DatabaseName = builder.Database ?? string.Empty;
                if (builder.IntegratedSecurity)
                {
                    connectionInfo.IntegratedSecurity = true;
                }
                else
                {
                    connectionInfo.Login = builder.Username ?? string.Empty;
                    connectionInfo.Password = builder.Password ?? string.Empty;
                }
                return connectionInfo;
            }
        }

        private class PostgreSQLCommand
        {
            public static async Task CreateDatabase(DbConnection conn, string databaseName)
            {
                var sql = $"CREATE DATABASE {databaseName};";

                using var command = conn.CreateCommand();
                command.CommandText = sql;

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            public static async Task DropDatabase(DbConnection conn, string databaseName)
            {
                var sql = $"DROP DATABASE {databaseName} WITH (FORCE);";

                using var command = conn.CreateCommand();
                command.CommandText = sql;

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            public static async Task<bool> ExistsDatabase(DbConnection conn, string databaseName)
            {
                var sql = $"SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{databaseName}');";

                using var command = conn.CreateCommand();
                command.CommandText = sql;

                try
                {
                    var result = await command.ExecuteScalarAsync();
                    return result != null;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
