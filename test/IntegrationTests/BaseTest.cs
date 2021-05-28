using ConsoleApp1.Domain;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class BaseTest<T> where T : BaseEntity
    {

        public IServiceScope Scope { get; private set; } = default!;
        public IServiceProvider ServiceProvider { get; private set; } = default!;
        public ISession Session { get; private set; } = default!;

        protected IBaseRepository<T> Repository { get; private set; } = default!;

        protected virtual bool CleanDbBetweenTests => true;
        protected virtual bool CleanDbWhenStartingTests => false;

        [SetUp]
        public virtual async Task SetUpAsync()
        {
            Session.Clear();
            if (CleanDbBetweenTests)
                await Database.CleanAsync();
        }

        [OneTimeSetUp]
        public virtual async Task OneTimeSetUpAsync()
        {
            if (CleanDbWhenStartingTests && !CleanDbBetweenTests)
                await Database.CleanAsync();
            Scope = Setup.Api.ServiceProvider.CreateScope();
            ServiceProvider = Scope.ServiceProvider;
            Session = ServiceProvider.GetRequiredService<ISession>();
            Repository = ServiceProvider.GetService<IBaseRepository<T>>();
        }

        [OneTimeTearDown]
        public virtual async Task OneTimeTearDownAsync()
        {
            if (Scope is IAsyncDisposable asyncDisposableScope)
                await asyncDisposableScope.DisposeAsync();
            else
                Scope.Dispose();
        }
    }
}
