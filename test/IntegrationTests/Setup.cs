using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [SetUpFixture]
    public class Setup
    {
        public const string ConnectionStringName = "DefaultConnection";

        public static SetupApi Api { get; private set; } = default!;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            Api = new SetupApi();
            await MigrateDb(Api.ServiceProvider);
        }

        private static async Task MigrateDb(IServiceProvider serviceProvider)
        {
            var dbCreator = serviceProvider.GetService<DatabaseCreator>();
            await dbCreator.DropAndCreateDb();
            dbCreator.Migrate();
        }

        [OneTimeTearDown]
        public ValueTask OneTimeTearDown() => Api.DisposeAsync();
    }
}
