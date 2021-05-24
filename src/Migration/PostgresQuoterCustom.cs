using FluentMigrator.Runner.Generators.Postgres;
using FluentMigrator.Runner.Processors.Postgres;

namespace Migration
{
    class PostgresQuoterCustom : PostgresQuoter
    {
        public PostgresQuoterCustom(PostgresOptions options) 
            : base(options) { } 

        protected override bool ShouldQuote(string name) => false;
    }
}
