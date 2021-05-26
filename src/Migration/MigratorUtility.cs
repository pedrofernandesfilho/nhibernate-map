using FluentMigrator.Runner;
using FluentMigrator.Runner.Generators.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Migration
{
    public class MigratorUtility
    {
        private readonly string connectionString;

        public MigratorUtility(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            this.connectionString = connectionString;
        }

        private IServiceProvider CreateServiceProvider() =>
            new ServiceCollection()
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                    builder
                        .AddPostgres()
                        .WithVersionTable(new VersionTable())
                        .WithGlobalConnectionString(connectionString)
                        .WithMigrationsIn(typeof(MigratorUtility).Assembly))
                .AddScoped<PostgresQuoter, PostgresQuoterCustom>()
                .BuildServiceProvider();

        public void Up() => Execute((runner) => runner.MigrateUp());

        public void Up(long version) => Execute((runner) => runner.MigrateUp(version));

        public void Down(long version) => Execute((runner) => runner.MigrateDown(version));

        private void Execute(Action<IMigrationRunner> action)
        {
            using var escopo = CreateServiceProvider().CreateScope();
            var runner = escopo.ServiceProvider.GetRequiredService<IMigrationRunner>();
            action(runner);
        }
    }
}
