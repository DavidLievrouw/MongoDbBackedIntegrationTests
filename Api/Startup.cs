using System;
using System.Configuration;
using System.Web.Http;
using Autofac.Integration.WebApi;
using DavidLievrouw.Api;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DavidLievrouw.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webConfig = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
                }, ConfigurationUserLevel.None);

            var containerBuilder = AutofacConfig.Configure(webConfig);
            var container = containerBuilder.Build();

            var config = new HttpConfiguration();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}