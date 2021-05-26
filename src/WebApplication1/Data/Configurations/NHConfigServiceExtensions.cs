using ConsoleApp1.Domain;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Linq;
using WebApplication1.Data.Extensions;

namespace WebApplication1.Data.Configurations
{
    public static class NHConfigServiceExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, Action<NHOptions> nhOptionsConfigurer)
        {
            var nhOptions = new NHOptions();
            nhOptionsConfigurer(nhOptions);
            services.AddSingleton(p => p.CreateSessionFactory(nhOptions));
            services.AddScoped(CreateSession);
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            return services;
        }

        private static ISession CreateSession(this IServiceProvider serviceProvider)
        {
            var sessionFactory = serviceProvider.GetRequiredService<ISessionFactory>();
            return sessionFactory.OpenSession();
        }

        public static ISessionFactory CreateSessionFactory(this IServiceProvider serviceProvider, NHOptions nhOptions)
        {
            var mapper = new ModelMapper();
            var mappings = (from t in typeof(NHConfigServiceExtensions).Assembly.GetExportedTypes()
                            where t.IsClass && t.IsAssignableToGenericType(typeof(ClassMapping<>)) && !t.IsAbstract
                            select t).ToArray();
            mapper.AddMappings(mappings);
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            mapping.autoimport = false;
            var configuration = new Configuration();
            if (nhOptions.ConfigureDbIntegrationConfigurationProperties == null)
                throw new ArgumentException("Database integration configuration missing.", nameof(nhOptions));
            configuration.DataBaseIntegration(nhOptions.ConfigureDbIntegrationConfigurationProperties);
            configuration.AddDeserializedMapping(mapping, "mapping");
            var dialect = Dialect.GetDialect(configuration.Properties);
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration, dialect);
            var sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory;
        }
    }
}
