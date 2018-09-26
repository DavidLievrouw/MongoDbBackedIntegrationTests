using System;
using System.Configuration;
using Autofac;
using Autofac.Integration.WebApi;
using MongoDB.Driver;

namespace DavidLievrouw.Api
{
    public static class AutofacConfig
    {
        public static ContainerBuilder Configure(Configuration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var builder = new ContainerBuilder();
            builder.RegisterInstance(new AppSettingsReader(config)).AsSelf();

            builder.Register(ctx =>
            {
                var settingsReader = ctx.Resolve<AppSettingsReader>();
                var mongoUrl = new MongoUrl(settingsReader.ReadConnectionString("mongo").ConnectionString);
                var databaseName = settingsReader.ReadAppSetting("DatabaseName");
                return new MongoConnectionInformation(mongoUrl, databaseName);
            }).AsSelf().SingleInstance();

            builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);

            return builder;
        }
    }
}