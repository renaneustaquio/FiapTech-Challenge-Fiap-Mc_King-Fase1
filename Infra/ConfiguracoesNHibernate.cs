using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System.Reflection;

namespace Infra
{
    internal static class ConfiguracoesNHibernate
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services)
        {

            services.AddSingleton<ISessionFactory>(factory =>
            {
                return Fluently.Configure().Database(() =>
                {
                    string connectionString = "Server=postgres; Port=5432; Username=postgres; Password=postgres; Database=mc_king";

                    return PostgreSQLConfiguration.Standard
                            .FormatSql()
                            .ShowSql()
                            .ConnectionString(connectionString);
                })
                   .Mappings(c => c.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                   .CurrentSessionContext("call")
                   .BuildSessionFactory();
            });

            services.AddScoped<ISession>(factory =>
            {
                return factory.GetService<ISessionFactory>().OpenSession();
            });

            return services;
        }
    }
}