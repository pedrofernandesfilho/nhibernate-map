using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApplication1;

namespace IntegrationTests
{
    public class SetupApi : IAsyncDisposable
    {
        private const string testEnvironment = "Test";
        private readonly WebApplicationFactory<Startup> factory;
        private readonly IServiceScope serviceScope;

        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public string ConnectionString => Configuration.GetConnectionString(Setup.ConnectionStringName);

        public SetupApi()
        {
            factory = new WebApplicationFactory<Startup>();
            factory = factory.WithWebHostBuilder(builder =>
                            builder.UseEnvironment(testEnvironment)
                            .ConfigureTestServices(services => services.AddSingleton<DatabaseCreator>() )
                            .ConfigureAppConfiguration((context, configuration) =>
                            {
                                configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.Integration.json"));
                                configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.Integration.User.json"), optional: true);
                            }));

            serviceScope = factory.Services.CreateScope();
            ServiceProvider = serviceScope.ServiceProvider;
            Configuration = ServiceProvider.GetService<IConfiguration>()!;
        }

        public async ValueTask DisposeAsync()
        {
            if (serviceScope is IAsyncDisposable asyncDisposableScope)
                await asyncDisposableScope.DisposeAsync();
            else
                serviceScope.Dispose();
            factory.Dispose();
        }
    }
}
