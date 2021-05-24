using FluentMigrator.Runner.Exceptions;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace Migration
{
    class Program
    {
        // exit codes vao de 64 a 113. http://tldp.org/LDP/abs/html/exitcodes.html
        static int Main(string[] args)
        {
            var optionConnectionString = new Option(new string[] { "--connectionString", "-s" }, "Database connection string")
            {
                Required = true,
                Argument = new Argument<string> { Arity = ArgumentArity.ExactlyOne }
            };

            var optionVersionUp = new Option(new string[] { "--version-up", "-u" }, "Desired version")
            {
                Argument = new Argument<long?> { Arity = ArgumentArity.ZeroOrOne }
            };

            var optionVersionDown = new Option(new string[] { "--version-down", "-d" }, "Desired version")
            {
                Argument = new Argument<long> { Arity = ArgumentArity.ExactlyOne }
            };

            var upCommand = new CommandBuilder(UpCommand())
                .AddOption(optionVersionUp)
                .AddOption(optionConnectionString)
                .Command;

            var downCommand = new CommandBuilder(DownCommand())
                            .AddOption(optionVersionDown)
                            .AddOption(optionConnectionString)
                            .Command;

            var migrationCommand = new CommandLineBuilder(new RootCommand("Command to execute migrations."))
                .UseDefaults()
                .AddCommand(upCommand)
                .AddCommand(downCommand)
                .Build();

            return migrationCommand.Invoke(args);
        }

        static Command UpCommand() => new Command("up", "Migrate up")
        {
            Handler = CommandHandler.Create<string, long?>((connectionString, version) =>
            {
                try
                {
                    var migratorUtility = new MigratorUtility(connectionString);
                    if (version.HasValue)
                        migratorUtility.Up(version.Value);
                    else
                        migratorUtility.Up();
                }
                catch (MissingMigrationsException)
                {
                    return 64;
                }
                return 0;
            })
        };

        static Command DownCommand() => new Command("down", "Migrate down")
        {
            Handler = CommandHandler.Create<string, long>((connectionString, versao) =>
            {
                try
                {
                    new MigratorUtility(connectionString).Down(versao);
                }
                catch (MissingMigrationsException)
                {
                    return 64;
                }
                return 0;
            })
        };
    }
}
