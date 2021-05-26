using Npgsql;
using Respawn;
using System.Threading.Tasks;

namespace IntegrationTests
{
    class Database
    {
        private static readonly Checkpoint checkpoint = new Checkpoint
        {
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { "version_info" },
            DbAdapter = DbAdapter.Postgres
        };

        public static async Task CleanAsync()
        {
            using var conn = new NpgsqlConnection(Setup.Api.ConnectionString);
            await conn.OpenAsync();
            await checkpoint.Reset(conn);
        }
    }
}
