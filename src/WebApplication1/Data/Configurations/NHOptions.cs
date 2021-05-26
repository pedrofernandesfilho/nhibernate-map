using NHibernate.Cfg.Loquacious;
using System;

namespace WebApplication1.Data.Configurations
{
    public class NHOptions
    {
        public Action<IDbIntegrationConfigurationProperties>? ConfigureDbIntegrationConfigurationProperties { get; private set; }

        public NHOptions DataBaseIntegration(Action<IDbIntegrationConfigurationProperties> configureDbIntegrationConfigurationProperties)
        {
            ConfigureDbIntegrationConfigurationProperties = configureDbIntegrationConfigurationProperties;
            return this;
        }
    }
}
