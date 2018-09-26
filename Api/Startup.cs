using System;
using System.Configuration;
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
            
            var mongoConnectionString = ConfigurationManager.ConnectionStrings["mongo"].ConnectionString;
            var containerBuilder = AutofacConfig.Configure(webConfig, mongoConnectionString);
            var container = containerBuilder.Build();

            var config = WebApiConfigurator.ConfigureWebApi(container);
            app.UseWebApi(config);
        }
    }
}